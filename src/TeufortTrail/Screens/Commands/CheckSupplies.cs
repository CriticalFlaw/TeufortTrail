using System;
using System.Collections.Generic;
using System.Text;
using TeufortTrail.Entities.Item;
using WolfCurses.Window;
using WolfCurses.Window.Control;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.Location
{
    [ParentWindow(typeof(Travel))]
    public sealed class CheckSupplies : InputForm<TravelInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.Location.CheckSupplies" /> class.
        /// </summary>
        public CheckSupplies(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the screen needs a prompt to be displayed to the player.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            // Build up representation of supplies once in constructor and then reference when asked for render.
            GameCore.Instance.TakeTurn(true);
            var _checkSupplies = new StringBuilder();

            // Build up a list with tuple in it to hold our data about supplies.
            var suppliesList = new List<Tuple<string, string>>();

            // Loop through every inventory item in the vehicle.
            foreach (var item in GameCore.Instance.Vehicle.Inventory)
            {
                switch (item.Key)
                {
                    case Types.Food:
                        suppliesList.Add(new Tuple<string, string>("Food", item.Value.TotalWeight.ToString("N0")));
                        break;

                    case Types.Hats:
                        suppliesList.Add(new Tuple<string, string>("Hats", item.Value.Quantity.ToString("N0")));
                        break;

                    case Types.Ammo:
                        suppliesList.Add(new Tuple<string, string>("Bullets", item.Value.Quantity.ToString("N0")));
                        break;

                    case Types.Money:
                        suppliesList.Add(new Tuple<string, string>("Money", item.Value.TotalValue.ToString("C")));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Generate the formatted table of supplies we will show to user.
            var supplyTable = suppliesList.ToStringTable(new[] { "Item Name", "Amount" }, u => u.Item1, u => u.Item2);

            // Return the generated tables.
            _checkSupplies.AppendLine($"{Environment.NewLine}Your Party:{Environment.NewLine}");
            _checkSupplies.AppendLine(TravelInfo.PartyStatus);
            _checkSupplies.AppendLine($"Your Supplies:{Environment.NewLine}");
            _checkSupplies.AppendLine(supplyTable);
            return _checkSupplies.ToString();
        }

        /// <summary>
        /// Process the player's response to the prompt message.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            ClearForm();
        }
    }
}