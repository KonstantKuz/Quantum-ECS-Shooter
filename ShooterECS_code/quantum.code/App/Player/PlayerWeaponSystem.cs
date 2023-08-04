namespace Quantum.App.Player
{
    public class PlayerWeaponSystem : SystemMainThreadFilter<PlayerWeaponSystem.PlayerWeaponFilter>
    {
        public unsafe struct PlayerWeaponFilter
        {
            public EntityRef EntityRef;
            public Aim* Aim;
            public PlayerTag* PlayerTag;
            public CharacterInventory* Inventory;
        }
        
        public override unsafe void Update(Frame f, ref PlayerWeaponFilter filter)
        {
            var input = f.GetPlayerInput(filter.PlayerTag->Player);
            if(!input->Fire.WasPressed || !filter.Aim->HasHit) return;
            var weaponData = f.FindAsset<WeaponData>(filter.Inventory->Weapon.Data.Id);
            var damageInfo = new DamageInfo { Value = weaponData.Damage };
            f.Signals.OnDamage(filter.EntityRef, filter.Aim->CurrentHit.Entity, damageInfo);
        }
    }
}