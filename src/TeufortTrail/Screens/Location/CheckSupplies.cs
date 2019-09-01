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

        protected override string OnDialogPrompt()
        {
            // Build up representation of supplies once in constructor and then reference when asked for render.
            GameCore.Instance.TakeTurn(true);
            var _checkSupplies = new StringBuilder();
            _checkSupplies.AppendLine($"{Environment.NewLine}Your Supplies{Environment.NewLine}");

            // Build up a list with tuple in it to hold our data about supplies.
            var suppliesList = new List<Tuple<string, string>>();

            // Loop through every inventory item in the vehicle.
            foreach (var item in GameCore.Instance.Vehicle.Inventory)
            {
                // Apply number formatting to quantities so they have thousand separators.
                var formattedQuantity = item.Value.Quantity.ToString("N0");
                switch (item.Key)
                {
                    case Types.Food:
                        suppliesList.Add(new Tuple<string, string>("pounds of food", item.Value.TotalWeight.ToString("N0")));
                        break;

                    case Types.Hats:
                        suppliesList.Add(new Tuple<string, string>("hats", formattedQuantity));
                        break;

                    case Types.Ammo:
                        suppliesList.Add(new Tuple<string, string>("bullets", formattedQuantity));
                        break;

                    case Types.Money:
                        suppliesList.Add(new Tuple<string, string>("money left", item.Value.TotalValue.ToString("C")));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Generate the formatted table of supplies we will show to user.
            var supplyTable = suppliesList.ToStringTable(new[] { "Item Name", "Amount" }, u => u.Item1, u => u.Item2);
            _checkSupplies.AppendLine(supplyTable);
            return _checkSupplies.ToString();
        }

        protected override void OnDialogResponse(DialogResponse reponse)
        {
            ClearForm();
        }
    }
}