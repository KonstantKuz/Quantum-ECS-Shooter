
namespace Quantum.App.Player
{
    public unsafe class PlayerMovementSystem : SystemMainThreadFilter<PlayerMovementSystem.PlayerFilter>
    {
        public struct PlayerFilter
        {
            public EntityRef Entity;
            public PlayerTag* PlayerTag;
            public CharacterConfig* CharacterConfig;
            public CharacterController3D* CharacterController;
        }

        public override void Update(Frame f, ref PlayerFilter filter)
        {
            var input = f.GetPlayerInput(filter.PlayerTag->Player);
            if (input->Jump.WasPressed) filter.CharacterController->Jump(f);
            var playerTransform = f.Unsafe.GetPointer<Transform3D>(filter.Entity);
            var data = f.FindAsset<CharacterConfigData>(filter.CharacterConfig->Data.Id);
            playerTransform->Rotate(playerTransform->Up, input->LookInput.X * data.RotationSpeed);
            filter.CharacterController->Move(f, filter.Entity, playerTransform->Rotation * input->MoveInput.XOY);
        }
    }
}