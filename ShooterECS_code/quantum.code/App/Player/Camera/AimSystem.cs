using Photon.Deterministic;

namespace Quantum.App.Player.Camera
{
    public class AimSystem : SystemMainThreadFilter<PlayerMovementSystem.PlayerFilter>
    {
        public override unsafe void Update(Frame f, ref PlayerMovementSystem.PlayerFilter filter)
        {
            var cameras = f.Unsafe.GetComponentBlockIterator<PlayerCamera>().GetEnumerator();
            while (cameras.MoveNext())
            {
                var camera = f.Unsafe.GetPointer<Transform3D>(cameras.Current.Entity);
                var playerAim = f.Unsafe.GetPointer<Aim>(cameras.Current.Component->PlayerEntity);
                var hitPoint = f.Physics3D.Raycast(camera->Position, camera->Forward, FP.UseableMax);
                playerAim->CurrentAim = hitPoint?.Point ?? camera->Position + camera->Forward * FP.UseableMax;
                playerAim->HasHit = hitPoint.HasValue;
                if (hitPoint.HasValue)
                {
                    playerAim->CurrentHit = hitPoint.Value;
                }
            }
        }
    }
}