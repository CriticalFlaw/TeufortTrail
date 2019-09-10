using System;
using System.Collections.Generic;
using System.Text;
using TeufortTrail.Entities.Item;
using TeufortTrail.Entities.Vehicle;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.Commands
{
    /// <summary>
    /// Displays a randomly generated trade offer for the player.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class TradeSupplies : InputForm<TravelInfo>
    {
        /// <summary>
        /// Defines the index in the list of trade offers, which will be displayed to the player.
        /// </summary>
        private int TradeIndex;

        /// <summary>
        /// Flags the player as being able to accept or deny a given trade offer.
        /// </summary>
        private bool PlayerCanTrade;

        /// <summary>
        /// Defines all the generated trade offers that will be presented to the player.
        /// </summary>
        private List<TradeGenerator> TradeOffers;

        /// <summary>
        /// Sets the kind of prompt response the player can give. Could be Yes, No or Press Any.
        /// </summary>
        protected override DialogType DialogType => ((TradeOffers != null) && (TradeOffers.Count > 0) && PlayerCanTrade) ? DialogType.YesNo : DialogType.Prompt;

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
            GameCore.Instance.TakeTurn(false);

            // Display the party's current resource supply.
            var _tradeSupplies = new StringBuilder();
            _tradeSupplies.AppendLine($"{Environment.NewLine}Your Supplies:{Environment.NewLine}");
            _tradeSupplies.AppendLine(TravelInfo.PartySupplies);

            // Generate random trade offers for this location.
            GenerateTrades();

            // Check that there are trade offers from the towns folk for the player.
            if (TradeOffers.Count <= 0)
                // Let the player know that there are no pending trade offers at this time.
                _tradeSupplies.AppendLine($"Nobody wants to trade with you.{Environment.NewLine}");
            else
            {
                // Display a random trade from the list of trade offers.
                TradeIndex = GameCore.Instance.Random.Next(TradeOffers.Count);
                _tradeSupplies.AppendLine($"You meet another trader who wants {TradeOffers[TradeIndex].WantedItem.Quantity:N0} {TradeOffers[TradeIndex].WantedItem.Name.ToLowerInvariant()} in exchange for {TradeOffers[TradeIndex].OfferedItem.Quantity:N0} {TradeOffers[TradeIndex].OfferedItem.Name.ToLowerInvariant()}.{Environment.NewLine}");

                // Display the prompt based on whether or not the player has the item the trader wants.
                PlayerCanTrade = GameCore.Instance.Vehicle.HasInventoryItem(TradeOffers[TradeIndex].WantedItem);
                _tradeSupplies.Append(PlayerCanTrade
                    ? $"Are you willing to trade? Y/N{Environment.NewLine}"
                    : $"Unfortunatly, you don't have this.{Environment.NewLine}");
            }
            return _tradeSupplies.ToString();
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            if (response == DialogResponse.Yes)
            {
                // Subtract the item quantity from the party inventory and add the quantity of the item the player traded for.
                GameCore.Instance.Vehicle.Inventory[TradeOffers[TradeIndex].WantedItem.Category].SubtractQuantity(TradeOffers[TradeIndex].WantedItem.Quantity);
                GameCore.Instance.Vehicle.Inventory[TradeOffers[TradeIndex].OfferedItem.Category].AddQuantity(TradeOffers[TradeIndex].OfferedItem.Quantity);
            }
            ClearForm();
            return;
        }

        /// <summary>
        /// Generate a list of trade offers that will be presented to the player.
        /// </summary>
        private void GenerateTrades()
        {
            // Create a new list of trade offers.
            TradeOffers = new List<TradeGenerator>();

            // Determine how many trade offers will be generated. Bail out if the number is zero.
            var totalTrades = GameCore.Instance.Random.Next(0, GameCore.Instance.Random.Next(1, 100));
            if (totalTrades <= 0) return;

            // Generate a given amount of trade offers.
            for (var x = 0; x < totalTrades; x++)
                TradeOffers.Add(new TradeGenerator());

            // Cleanup the trade offers list, remove duplicates and errors.
            var _tradeOffers = new List<TradeGenerator>(TradeOffers);
            foreach (var trade in _tradeOffers)
            {
                // Remove trades that are the same item twice.
                if ((trade.WantedItem != null) && (trade.OfferedItem != null) && (trade.WantedItem.Category != trade.OfferedItem.Category)) continue;
                TradeOffers.Remove(trade);
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
            WantedItem = Vehicle.GetRandomItem();
            OfferedItem = Vehicle.GetRandomItem();
        }
    }
}