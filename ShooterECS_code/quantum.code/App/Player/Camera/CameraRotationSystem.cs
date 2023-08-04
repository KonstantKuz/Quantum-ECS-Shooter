using Photon.Deterministic;

namespace Quantum.App.Player.Camera
{
    public unsafe class CameraRotationSystem : SystemMainThread
    {
        private const int SMOOTH_STEP = 100;
        private const int MAX_ANGLE = 40;
        public struct CameraFilter
        {
            public EntityRef Entity;
            public PlayerCamera* PlayerCamera;
            public NestedEntity* NestedEntity;
        }

        public override void Update(Frame f)
        {
            var cameras = f.Unsafe.FilterStruct<CameraFilter>();
            var current = default(CameraFilter);
            while (cameras.Next(&current))
            {
                Input* input = f.GetPlayerInput(current.PlayerCamera->PlayerRef);
                var configId = f.Unsafe.GetPointer<CharacterConfig>(current.PlayerCamera->PlayerEntity)->Data.Id;
                var data = f.FindAsset<CharacterConfigData>(configId);
                if(FPMath.Abs(input->MouseInput.Y) < FP._0_01) return;
                current.PlayerCamera->PitchAngle -= input->MouseInput.Y * data.RotationSpeed;
                current.PlayerCamera->PitchAngle = FPMath.Clamp(current.PlayerCamera->PitchAngle, -MAX_ANGLE, MAX_ANGLE);
                var rotation = FPQuaternion.AngleAxis(current.PlayerCamera->PitchAngle, FPVector3.Right);
                var interpolatedPosition = 
                    FPVector3.Lerp(current.NestedEntity->LocalPosition, rotation * current.PlayerCamera->Offset, f.DeltaTime * SMOOTH_STEP);
                current.NestedEntity->LocalPosition = interpolatedPosition;
                var interpolatedRotation = 
                    FPVector3.Lerp(current.NestedEntity->LocalRotation, rotation.AsEuler, f.DeltaTime * SMOOTH_STEP);
                current.NestedEntity->LocalRotation = interpolatedRotation;
            }
        }
    }
}