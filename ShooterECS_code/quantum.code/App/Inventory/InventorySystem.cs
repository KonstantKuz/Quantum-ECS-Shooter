namespace Quantum.App.Inventory
{
    public class InventorySystem : SystemSignalsOnly, ISignalOnComponentAdded<Quantum.Inventory>, ISignalOnComponentRemoved<Quantum.Inventory>
    {
        public unsafe void OnAdded(Frame f, EntityRef entity, Quantum.Inventory* component)
        {
            component->ActiveItem = Item.None;
            if (!f.TryResolveList(component->Items, out var items))
            {
                component->Items = f.AllocateList<Item>();
            }
        }

        public unsafe void OnRemoved(Frame f, EntityRef entity, Quantum.Inventory* component)
        {
            f.TryFreeList(component->Items);
            component->Items = default;
        }
    }
}