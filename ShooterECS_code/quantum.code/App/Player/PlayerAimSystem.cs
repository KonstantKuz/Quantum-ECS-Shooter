using Photon.Deterministic;
using Quantum.App.Player.Camera;
using Quantum.Physics3D;

namespace Quantum.App.Player
{
    public class PlayerAimSystem : SystemMainThreadFilter<PlayerCameraFilter>
    {
        private static readonly FP AIM_DISTANCE = 100;
        
        public override unsafe void Update(Frame f, ref PlayerCameraFilter filter)
        {
            var camera = f.Unsafe.GetPointer<Transform3D>(filter.Entity);
            var playerAim = f.Unsafe.GetPointer<Aim>(filter.PlayerCamera->PlayerEntity);
            var hitPoint = f.Physics3D.Raycast(camera->Position, camera->Forward, AIM_DISTANCE);
            playerAim->CurrentAim = hitPoint?.Point ?? camera->Position + camera->Forward * FP.UseableMax;
            playerAim->CurrentHit = hitPoint ?? default(Hit3D);
        }
    }
}