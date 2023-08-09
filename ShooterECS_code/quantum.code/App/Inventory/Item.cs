namespace Quantum
{
    public partial struct Item
    {
        public static Item None => new();
        public override bool Equals(object obj)
        {
            return obj is Item item && item.Id == Id;
        }
    }
}