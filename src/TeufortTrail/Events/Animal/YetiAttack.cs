using System.Collections.Generic;
using TeufortTrail.Entities;
using TeufortTrail.Events.Director;
using TeufortTrail.Events.Prefab;

namespace TeufortTrail.Events.Animal
{
    /// <summary>
    /// A yeti raids the camp, wrecking supplies and mauling whoever gets in its way.
    /// </summary>
    [DirectorEvent(EventCategory.Animal)]
    public sealed class YetiAttack : ItemDestroyer
    {
        /// <summary>
        /// Called before the event has been executed.
        /// </summary>
        protected override string OnPreDestroyItems()
        {
            return "A wild yeti attacked your camp in the night!";
        }

        /// <summary>
        /// Called after the event has been executed.
        /// </summary>
        protected override string OnPostDestroyItems(IDictionary<ItemTypes, int> destroyedItems)
        {
            return TryHurtPassengers(25, 60, 160, "mauled");
        }
    }
}
