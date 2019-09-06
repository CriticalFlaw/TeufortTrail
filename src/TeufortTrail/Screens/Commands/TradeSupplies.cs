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
    [ParentWindow(typeof(Travel))]
    public sealed class TradeSupplies : InputForm<TravelInfo>
    {
        #region VARIABLES

        private StringBuilder _tradeSupplies;
        private List<TradeGenerator> TradeOffers;
        private int TradeIndex;
        private bool PlayerCanTrade;
        protected override DialogType DialogType => ((TradeOffers != null) && (TradeOffers.Count > 0) && PlayerCanTrade) ? DialogType.YesNo : DialogType.Prompt;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.Commands.TradeSupplies" /> class.
        /// </summary>
        public TradeSupplies(IWindow window) : base(window)
        {
            _tradeSupplies = new StringBuilder();
        }

        /// <summary>
        /// Called when the screen needs a prompt to be displayed to the player.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            GameCore.Instance.TakeTurn(false);
            _tradeSupplies.Clear();

            // Display the player's current supply.
            _tradeSupplies.AppendLine($"{Environment.NewLine}Your Supplies:{Environment.NewLine}");
            _tradeSupplies.AppendLine(TravelInfo.PartySupplies);

            // Randomly generate the trades for this location.
            GenerateTrades();

            // Check that there are trade offers from the towns folk for the player.
            if (TradeOffers.Count > 0)
            {
                // Display a random trade from the list of trade offers.
                TradeIndex = GameCore.Instance.Random.Next(TradeOffers.Count);
                _tradeSupplies.AppendLine($"You meet another trader who wants {TradeOffers[TradeIndex].WantedItem.Quantity:N0} {TradeOffers[TradeIndex].WantedItem.Name.ToLowerInvariant()} in exchange for {TradeOffers[TradeIndex].OfferedItem.Quantity:N0} {TradeOffers[TradeIndex].OfferedItem.Name.ToLowerInvariant()}.{Environment.NewLine}");

                // Display the last trade message based on whether or not the player has the item the trader wants.
                PlayerCanTrade = GameCore.Instance.Vehicle.HasInventoryItem(TradeOffers[TradeIndex].WantedItem);
                _tradeSupplies.Append(PlayerCanTrade
                    ? $"Are you willing to trade? Y/N{Environment.NewLine}"
                    : $"Unfortunatly, you don't have this.{Environment.NewLine}");
            }
            else
                // Let the player know that there are no pending trades at this time.
                _tradeSupplies.AppendLine($"Nobody wants to trade with you.{Environment.NewLine}");
            return _tradeSupplies.ToString();
        }

        /// <summary>
        /// Process the player's response to the prompt message.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            switch (response)
            {
                case DialogResponse.Yes:
                    // Remove the quantity of item from the vehicle inventory the trader wants and give the player the item the traded for.
                    GameCore.Instance.Vehicle.Inventory[TradeOffers[TradeIndex].WantedItem.Category].SubtractQuantity(TradeOffers[TradeIndex].WantedItem.Quantity);
                    GameCore.Instance.Vehicle.Inventory[TradeOffers[TradeIndex].OfferedItem.Category].AddQuantity(TradeOffers[TradeIndex].OfferedItem.Quantity);
                    break;

                case DialogResponse.Custom:
                case DialogResponse.No:
                default:
                    break;
            }
            ClearForm();
            return;
        }

        private void GenerateTrades()
        {
            // Create a new list of trade offers.
            TradeOffers = new List<TradeGenerator>();

            // Determine how many trade offers will be generated. Drop out if the number is zero.
            var totalTrades = GameCore.Instance.Random.Next(0, GameCore.Instance.Random.Next(1, 100));
            if (totalTrades <= 0) return;

            // Generate a given amount of trade offers.
            for (var x = 0; x < totalTrades; x++)
                TradeOffers.Add(new TradeGenerator());

            // Cleanup the list of trade offers, removing duplicates and errors.
            var _TradeOffers = new List<TradeGenerator>(TradeOffers);
            foreach (var trade in _TradeOffers)
            {
                // Remove trades that are the same item twice.
                if ((trade.WantedItem != null) && (trade.OfferedItem != null) && (trade.WantedItem.Category != trade.OfferedItem.Category)) continue;
                TradeOffers.Remove(trade);
            }
        }
    }

    public sealed class TradeGenerator
    {
        #region VARIABLES

        public Item WantedItem { get; }
        public Item OfferedItem { get; }

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.TradeGenerator" /> class.
        /// </summary>
        public TradeGenerator()
        {
            WantedItem = Vehicle.GetRandomItem();
            OfferedItem = Vehicle.GetRandomItem();
        }
    }
}