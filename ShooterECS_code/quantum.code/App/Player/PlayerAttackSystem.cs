
namespace Quantum.App.Player
{
    public class PlayerAttackSystem : SystemMainThreadFilter<PlayerAttackSystem.PlayerFilter>
    {
        public unsafe struct PlayerFilter
        {
            public EntityRef EntityRef;
            public Aim* Aim;
            public PlayerTag* PlayerTag;
            public Quantum.Inventory* Inventory;
        }
        
        public override unsafe void Update(Frame f, ref PlayerFilter filter)
        {
            var input = f.GetPlayerInput(filter.PlayerTag->Player);
            if(!input->Fire.WasPressed) return;
            if (!f.Unsafe.TryGetPointer(filter.Inventory->ActiveItem.EntityRef, out Weapon* weapon)
            || !weapon->IsReady(f)) return;
            
            f.Signals.OnWeaponFire(weapon, filter.Aim);
        }
    }
}