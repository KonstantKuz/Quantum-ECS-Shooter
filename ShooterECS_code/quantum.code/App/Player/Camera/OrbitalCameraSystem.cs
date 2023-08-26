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

            filter.PlayerCamera->PitchAngle -= input->LookInput.Y * data.RotationSpeed;
            filter.PlayerCamera->PitchAngle = FPMath.Clamp(filter.PlayerCamera->PitchAngle, -MAX_ANGLE, MAX_ANGLE);
            filter.PlayerCamera->YawAngle += input->LookInput.X * data.RotationSpeed;

            var cameraTransform = f.Unsafe.GetPointer<Transform3D>(filter.Entity);
            var playerTransform = f.Unsafe.GetPointer<Transform3D>(filter.PlayerCamera->PlayerEntity);
                
            var rotation = 
                FPQuaternion.AngleAxis(filter.PlayerCamera->PitchAngle, playerTransform->Right) * FPQuaternion.AngleAxis(filter.PlayerCamera->YawAngle, playerTransform->Up);
            var desiredPosition = playerTransform->Position + rotation * filter.PlayerCamera->Offset;
            var checkStart = playerTransform->Position + filter.PlayerCamera->Offset.XY.XYO;
            var fixedPosition = FixPositionIfHits(f, checkStart, desiredPosition);
            cameraTransform->Position = 
                FPVector3.Lerp(cameraTransform->Position, fixedPosition, f.DeltaTime * SMOOTH_STEP);
            cameraTransform->Rotation = 
                FPQuaternion.Lerp(cameraTransform->Rotation, rotation, f.DeltaTime * SMOOTH_STEP);
        }

        private FPVector3 FixPositionIfHits(Frame frame, FPVector3 checkStart, FPVector3 desiredPosition)
        {
            var lineCast = frame.Physics3D.Linecast(checkStart, desiredPosition);
            return lineCast?.Point ?? desiredPosition;
        }
    }
}