using Photon.Deterministic;

namespace Quantum
{
    public class WallRunSystem : SystemMainThreadFilter<WallRunSystem.WallRunningCharacterFilter>
    {
        public unsafe struct WallRunningCharacterFilter
        {
            public EntityRef EntityRef;
            public Transform3D* Transform3D;
            public CharacterController3D* CharacterController3D;
            public WallRun* WallRun;
        }

        public override unsafe void Update(Frame f, ref WallRunningCharacterFilter filter)
        {
            var character = filter.CharacterController3D;
            var transform = filter.Transform3D;
            var wallRun = filter.WallRun;
            if(character->Grounded || character->Velocity.Magnitude <= FP._0) return;
            if (character->Jumped)
            {
                wallRun->JumpWaitTimer = wallRun->JumpWaitTime;
            }

            if (wallRun->JumpWaitTimer > FP._0)
            {
                wallRun->JumpWaitTimer -= f.DeltaTime;
                return;
            }
            
            var sphere = Shape3D.CreateSphere(filter.WallRun->CheckDistance);
            var hits = f.Physics3D.OverlapShape(transform->Position, FPQuaternion.Identity, sphere, 1 << filter.WallRun->Layer);
            if(hits.Count == 0) return;
            hits.SortCastDistance();
            var hitNormal = hits[0].Normal;
            character->Velocity.Y = 0;
            character->Velocity += hitNormal * filter.WallRun->SnapForce;
        }
    }
}