using Quantum.Collections;

namespace Quantum
{
    public partial struct Inventory
    {
        public void Add(Frame frame, Item item)
        {
            if(!TryGetItems(frame, out var items) 
               || items.Contains(item)) return;
            items.Add(item);
        }

        public void Remove(Frame frame, Item item)
        {
            if(!TryGetItems(frame, out var items) 
               || items.Contains(item)) return;
            items.Remove(item);
        }
        
        private bool TryGetItems(Frame frame, out QList<Item> items)
        {
            var result = frame.TryResolveList(Items, out var resolvedList);
            items = result ? resolvedList : default;
            return result;
        }
    }
}