using Photon.Deterministic;

namespace Quantum.App.Player.Camera
{
    public unsafe class OrbitalCameraSystem : SystemMainThreadFilter<PlayerCameraFilter>
    {
        private const int SMOOTH_STEP = 20;
        private const int MAX_ANGLE = 40;

        public override void Update(Frame f, ref PlayerCameraFilter filter)
        {
            var input = f.GetPlayerInput(filter.PlayerCamera->PlayerRef);
            var configId = f.Unsafe.GetPointer<CharacterConfig>(filter.PlayerCamera->PlayerEntity)->Data.Id;
            var data = f.FindAsset<CharacterConfigData>(configId);

            filter.PlayerCamera->PitchAngle -= input->MouseInput.Y * data.RotationSpeed;
            filter.PlayerCamera->PitchAngle = FPMath.Clamp(filter.PlayerCamera->PitchAngle, -MAX_ANGLE, MAX_ANGLE);
            filter.PlayerCamera->YawAngle += input->MouseInput.X * data.RotationSpeed;

            var cameraTransform = f.Unsafe.GetPointer<Transform3D>(filter.Entity);
            var playerTransform = f.Unsafe.GetPointer<Transform3D>(filter.PlayerCamera->PlayerEntity);
                
            var rotation = 
                FPQuaternion.AngleAxis(filter.PlayerCamera->PitchAngle, playerTransform->Right) * FPQuaternion.AngleAxis(filter.PlayerCamera->YawAngle, playerTransform->Up);
            cameraTransform->Position = 
                FPVector3.Lerp(cameraTransform->Position, playerTransform->Position + rotation * filter.PlayerCamera->Offset, f.DeltaTime * SMOOTH_STEP);
            cameraTransform->Rotation = 
                FPQuaternion.Lerp(cameraTransform->Rotation, rotation, f.DeltaTime * SMOOTH_STEP);
        }
    }
}