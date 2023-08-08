
namespace Quantum.App.Combat.Weapon
{
    public class WeaponSystem : SystemMainThread, ISignalOnWeaponFire
    {
        public unsafe void OnWeaponFire(Frame f, Quantum.Weapon* weapon, Aim* aim)
        {
            var weaponData = f.FindAsset<WeaponData>(weapon->Data.Id);
            if(!weapon->IsReady(f) || !aim->HasHit) return;
            weapon->OnFire();
            var damageInfo = new DamageInfo { Value = weaponData.Damage };
            f.Signals.OnDamage(aim->CurrentHit.Entity, damageInfo);
        }
        
        public override unsafe void Update(Frame f)
        {
            foreach (var weapon in f.Unsafe.GetComponentBlockIterator<Quantum.Weapon>())
            {
                weapon.Component->FireTimer += f.DeltaTime;
            }
        }
    }
}