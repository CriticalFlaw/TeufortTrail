namespace TeufortTrail.Entities.Location
{
    public sealed class Town : Location
    {
        public bool ShoppingAllowed => true;
        public bool TalkingAllowed => true;

        public Town(string name) : base(name)
        {
        }
    }
}