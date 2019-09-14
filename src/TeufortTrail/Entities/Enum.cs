using WolfCurses.Utility;

namespace TeufortTrail.Entities
{
    /// <summary>
    /// Defines all possible item types.
    /// </summary>
    public enum ItemTypes
    {
        /// <summary>
        /// Food purchased in store or acquired through trade. Consumed by the party during travel.
        /// </summary>
        Food = 1,

        /// <summary>
        /// Clothing worn by the party to keep them warm and avoid the risk of contracting an illness.
        /// </summary>
        [Description("Hats")] Clothing = 2,

        /// <summary>
        /// Ammunition used fighting off the robot army during travel. Can be purchased in-store or traded with towns folks.
        /// </summary>
        Ammo = 3,

        /// <summary>
        /// Money that can be exchanged for goods and service during the playthrough.
        /// </summary>
        Money = 4
    }

    /// <summary>
    /// Defines the different types of river crossings options.
    /// </summary>
    public enum RiverOptions
    {
        None = 0,
        [Description("Ford across the river.")] Float = 1,
        [Description("Take a ferry across.")] Ferry = 2,
        [Description("Ask locals for help.")] Help = 3
    }

    /// <summary>
    /// Defines the status of the location in relation to player's trail progression.
    /// </summary>
    public enum LocationStatus
    {
        Unreached = 0,
        Arrived = 1,
        Departed = 2
    }

    /// <summary>
    /// Defines all the playable classes.
    /// </summary>
    /// <remarks>TODO: Add descriptions and unique stats to each class.</remarks>
    public enum Classes
    {
        Scout = 1,
        Soldier = 2,
        Pyro = 3,
        Demoman = 4,
        Heavy = 5,
        Medic = 6,
        Engineer = 7,
        Sniper = 8,
        Spy = 9
    }

    /// <summary>
    /// Defines all party member health values in numeric form.
    /// </summary>
    public enum HealthStatus
    {
        Great = 500,
        Good = 400,
        Fair = 300,
        Poor = 200,
        Dead = 0
    }

    /// <summary>
    /// Defines the status of the vehicle.
    /// </summary>
    public enum VehicleStatus
    {
        Stopped = 0,
        Moving = 1,
        Disabled = 2
    }

    /// <summary>
    /// Defines the rate at which resources will be consumed on the trail.
    /// </summary>
    public enum RationLevel
    {
        [Description("Filling - meals are large and generous.")] Filling = 3,
        [Description("Meager - meals are small, but adequate.")] Meager = 2,
        [Description("Bare Bones - meals are very small, everyone stays hungry.")] Bare = 1
    }

    /// <summary>
    /// Defines the event type (Vehicle, Party, Weather etc.).
    /// </summary>
    public enum EventCategory
    {
        Animal,
        Person,
        River,
        Special,
        Vehicle,
        Weather,
        Wild
    }

    /// <summary>
    /// Defines whether the event is called manually or randomly.
    /// </summary>
    public enum EventExecution
    {
        RandomOrManual = 0,
        ManualOnly = 1
    }
}