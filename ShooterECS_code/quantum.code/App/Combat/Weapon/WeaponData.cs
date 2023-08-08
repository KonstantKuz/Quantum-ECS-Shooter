using Photon.Deterministic;

namespace Quantum
{
    partial class WeaponData
    {
        public string Name;
        public FP Damage;
        public FP FireRate;

        public int Id => Name.GetHashCode();
    }
}