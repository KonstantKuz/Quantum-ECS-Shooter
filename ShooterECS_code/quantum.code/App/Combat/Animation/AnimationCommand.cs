using Photon.Deterministic;

namespace Quantum.App.Combat.Animation
{
    public class AnimationCommand : DeterministicCommand
    {
        public EntityRef Sender;
        public override void Serialize(BitStream stream)
        {
            stream.Serialize(ref Sender);
        }
    }
}