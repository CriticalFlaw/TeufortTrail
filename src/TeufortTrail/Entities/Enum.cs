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
    public enum Classes
    {
        [Description("You've selected the Scout.")] Scout = 1,
        [Description("You've selected the Soldier.")] Soldier = 2,
        [Description("You've selected the Pyro.")] Pyro = 3,
        [Description("You've selected the Demoman.")] Demoman = 4,
        [Description("You've selected the Heavy.")] Heavy = 5,
        [Description("You've selected the Medic.")] Medic = 6,
        [Description("You've selected the Engineer.")] Engineer = 7,
        [Description("You've selected the Sniper.")] Sniper = 8,
        [Description("You've selected the Spy.")] Spy = 9
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
        [Description("Meager - meals are small but adequate.")] Meager = 2,
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

    /// <summary>
    /// Defines the words the player needs to type out when fighting robots
    /// </summary>
    public enum HuntingWord
    {
        None = 0,
        Bam = 1,
        Bang = 2,
        Bonk = 3,
        Clank = 4,
        Kapow = 5,
        Klonk = 6,
        Pow = 7,
        Thunk = 8,
        Whack = 9,
        Wham = 10
    }
}