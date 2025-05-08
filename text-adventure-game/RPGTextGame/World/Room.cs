namespace World
{
    class Room
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Dictionary<string, Room> Exits { get; private set; }
        public bool IsHostile { get; private set; }

        public Room(string name, string description, bool isHostile = false)
        {
            Name = name;
            Description = description;
            IsHostile = isHostile;
        }

        public Room(string name, string description, Dictionary<string, Room> exits, bool isHostile = false) 
        {
            Name = name;
            Description = description;
            Exits = exits;
            IsHostile = isHostile;
        }

        public void SetExits(Dictionary<string, Room> exits)
        {
            Exits = exits;
        }
    }
}
