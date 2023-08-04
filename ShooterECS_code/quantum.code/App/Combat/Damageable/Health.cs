using Photon.Deterministic;

namespace Quantum
{
    public partial struct Health
    {
        public bool IsDead => Current <= FP._0;
    }
}