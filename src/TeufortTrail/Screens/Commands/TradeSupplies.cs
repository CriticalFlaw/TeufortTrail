using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeufortTrail.Entities.Item;
using TeufortTrail.Entities.Vehicle;
using TeufortTrail.Screens.Travel;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Commands
{
    /// <summary>
    /// Displays a randomly generated trade offer for the player.
    /// </summary>
    [ParentWindow(typeof(Travel.Travel))]
    public sealed class TradeSupplies : InputForm<TravelInfo>
    {
        /// <summary>
        /// Defines the index in the list of trade offers, which will be displayed to the player.
        /// </summary>
        private int _tradeIndex;

        /// <summary>
        /// Flags the player as being able to accept or deny a given trade offer.
        /// </summary>
        private bool _playerCanTrade;

        /// <summary>
        /// Defines all the generated trade offers that will be presented to the player.
        /// </summary>
        private List<TradeGenerator> _tradeOffers;

        /// <summary>
        /// Sets the kind of prompt response the player can give. Could be Yes, No or Press Any.
        /// </summary>
        protected override DialogType DialogType => (_tradeOffers is {Count: > 0} && _playerCanTrade) ? DialogType.YesNo : DialogType.Prompt;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="TradeSupplies" /> class.
        /// </summary>
        public TradeSupplies(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            // Increment the turn counter without advancing time.
            GameCore.Instance.TakeTurn();

            // Display the party's current resource supply.
            var tradeSupplies = new StringBuilder();
            //_tradeSupplies.AppendLine($"{Environment.NewLine}Your Supplies:{Environment.NewLine}");
            //_tradeSupplies.AppendLine(TravelInfo.PartySupplies);
            tradeSupplies.AppendLine();

            // Generate random trade offers for this location.
            GenerateTrades();

            // Check that there are trade offers from the towns folk for the player.
            if (_tradeOffers.Count <= 0)
                // Let the player know that there are no pending trade offers at this time.
                tradeSupplies.AppendLine($"Nobody wants to trade with you.{Environment.NewLine}");
            else
            {
                // Display a random trade from the list of trade offers.
                _tradeIndex = GameCore.Instance.Random.Next(_tradeOffers.Count);
                tradeSupplies.AppendLine($"You meet another trader who wants {_tradeOffers[_tradeIndex].WantedItem.Quantity:N0} {_tradeOffers[_tradeIndex].WantedItem.Name.ToLowerInvariant()} in exchange for {_tradeOffers[_tradeIndex].OfferedItem.Quantity:N0} {_tradeOffers[_tradeIndex].OfferedItem.Name.ToLowerInvariant()}.{Environment.NewLine}");

                // Display the prompt based on whether or not the player has the item the trader wants.
                _playerCanTrade = GameCore.Instance.Vehicle.HasInventoryItem(_tradeOffers[_tradeIndex].WantedItem);
                tradeSupplies.Append(_playerCanTrade
                    ? "Are you willing to trade? Y/N"
                    : "Unfortunately, you don't have this.");
            }
            return tradeSupplies.ToString();
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            if (response == DialogResponse.Yes)
            {
                // Subtract the item quantity from the party inventory and add the quantity of the item the player traded for.
                GameCore.Instance.Vehicle.Inventory[_tradeOffers[_tradeIndex].WantedItem.Category].SubtractQuantity(_tradeOffers[_tradeIndex].WantedItem.Quantity);
                GameCore.Instance.Vehicle.Inventory[_tradeOffers[_tradeIndex].OfferedItem.Category].AddQuantity(_tradeOffers[_tradeIndex].OfferedItem.Quantity);
            }
            ClearForm();
        }

        /// <summary>
        /// Generate a list of trade offers that will be presented to the player.
        /// </summary>
        private void GenerateTrades()
        {
            // Create a new list of trade offers.
            _tradeOffers = new List<TradeGenerator>();

            // Determine how many trade offers will be generated. Bail out if the number is zero.
            var totalTrades = GameCore.Instance.Random.Next(0, GameCore.Instance.Random.Next(1, 100));
            if (totalTrades <= 0) return;

            // Generate a given amount of trade offers.
            for (var x = 0; x < totalTrades; x++)
                _tradeOffers.Add(new TradeGenerator());

            // Cleanup the trade offers list, remove duplicates and errors.
            var tradeOffers = new List<TradeGenerator>(_tradeOffers);
            foreach (var trade in tradeOffers.Where(trade => (trade.WantedItem == null) || (trade.OfferedItem == null) || (trade.WantedItem.Category == trade.OfferedItem.Category)))
            {
                _tradeOffers.Remove(trade);
            }
        }
    }

    /// <summary>
    /// Creates a trade offer, by pulling a resource item from the schema of random type and quantity.
    /// </summary>
    public sealed class TradeGenerator
    {
        public Item WantedItem { get; }
        public Item OfferedItem { get; }

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="TradeGenerator" /> class.
        /// </summary>
        public TradeGenerator()
        {
            WantedItem = Vehicle.CreateRandomItem();
            OfferedItem = Vehicle.CreateRandomItem();
        }
    }
}