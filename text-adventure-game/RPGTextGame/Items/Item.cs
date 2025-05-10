namespace Items
{
    class Item
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Price { get; private set; }

        public Item(string name, string description, int price)
        {
            Name = name;
            Description = description;
            Price = price;
        }
    }
}
