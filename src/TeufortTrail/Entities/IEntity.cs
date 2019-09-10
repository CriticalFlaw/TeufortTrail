using WolfCurses;

namespace TeufortTrail.Entities
{
    /// <summary>
    /// Base interface for all game entities, this is primarily by the event director.
    /// </summary>
    /// <see cref="Events.Director.EventDirector"/>
    public interface IEntity : ITick
    {
    }
}