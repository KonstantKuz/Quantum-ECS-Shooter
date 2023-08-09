using System;
using System.Collections.Generic;
using Quantum.App.Combat.Animation;

namespace Quantum.App.Combat.Weapon
{
    public class WeaponSystem : SystemMainThread, ISignalOnWeaponFire
    {
        private unsafe delegate void FireHandler(Frame f, Quantum.Weapon* weapon, Aim aim);

        private static readonly unsafe Dictionary<WeaponType, FireHandler> _fireHandlers = new()
        {
            {WeaponType.Raycast, RaycastFire},
            {WeaponType.Projectile, ProjectileFire}
        };

        public unsafe void OnWeaponFire(Frame f, Quantum.Weapon* weapon, Aim aim)
        {
            if(!weapon->IsReady(f)) return;
            var data = f.FindAsset<WeaponData>(weapon->Data.Id);
            _fireHandlers[data.WeaponType].Invoke(f, weapon, aim);
        }

        private static unsafe void RaycastFire(Frame frame, Quantum.Weapon* weapon, Aim aim)
        {
            weapon->OnFire();
            if (!aim.HasHit) return;
            var weaponData = frame.FindAsset<WeaponData>(weapon->Data.Id);
            var damageInfo = new DamageInfo { Value = weaponData.Damage };
            frame.Signals.OnDamage(aim.CurrentHit.Entity, damageInfo);
        }

        private static unsafe void ProjectileFire(Frame frame, Quantum.Weapon* weapon, Aim aim)
        {
            throw new NotImplementedException();
        }
        
        public override void Update(Frame f)
        {
            UpdateWeaponTimers(f);
            ExecuteFireCommands(f);
        }

        private unsafe void UpdateWeaponTimers(Frame frame)
        {
            foreach (var weapon in frame.Unsafe.GetComponentBlockIterator<Quantum.Weapon>())
            {
                weapon.Component->FireTimer += frame.DeltaTime;
            }
        }

        private unsafe void ExecuteFireCommands(Frame frame)
        {
            for (int i = 0; i < frame.PlayerCount; i++)
            {
                var command = frame.GetPlayerCommand(i) as AttackCommand;
                if(command == null) continue;
                OnWeaponFire(frame, command.GetWeapon(frame), command.GetAim(frame));
            }
        }
    }
}