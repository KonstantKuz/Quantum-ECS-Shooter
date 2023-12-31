﻿namespace Quantum.App.Combat.Damageable
{
    public class DamageSystem : SystemSignalsOnly, ISignalOnDamage
    {
        public unsafe void OnDamage(Frame f, EntityRef target, DamageInfo damageInfo)
        {
            if(!f.Has<Health>(target) || f.Get<Health>(target).IsDead) return;
            var targetHealth = f.Unsafe.GetPointer<Health>(target);
            targetHealth->Current -= damageInfo.Value;
            if (targetHealth->IsDead)
            {
                OnTargetIsDead(f, target);
            }
        }

        private void OnTargetIsDead(Frame frame, EntityRef target)
        {
            if (!frame.TryGet(target, out PlayerTag playerTag))
            {
                frame.Destroy(target);
                return;
            }

            frame.Remove<CharacterController3D>(target);
            frame.Events.EntityDead(target);
        }
    }
}