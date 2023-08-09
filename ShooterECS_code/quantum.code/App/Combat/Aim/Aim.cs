using Quantum.Physics3D;

namespace Quantum
{
    public partial struct Aim
    {
        public bool HasHit => !CurrentHit.Equals(default(Hit3D));
    }
}