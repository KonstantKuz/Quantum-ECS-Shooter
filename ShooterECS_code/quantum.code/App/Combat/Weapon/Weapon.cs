using Photon.Deterministic;

namespace Quantum
{
    public partial struct Weapon
    {
        public bool IsReady(Frame frame)
        {
            var data = frame.FindAsset<WeaponData>(Data.Id);
            return FireTimer >= data.FireRate;
        }

        public void OnFire()
        {
            FireTimer = FP._0;
        }
    }
}