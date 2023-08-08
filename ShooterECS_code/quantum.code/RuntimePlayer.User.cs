using Photon.Deterministic;

namespace Quantum {
  partial class RuntimePlayer
  {
    public AssetRefEntityPrototype CharacterPrototype;
    public AssetRefEntityPrototype Camera;
    public AssetRefWeaponData[] Weapons;

    public void SerializeUserData(BitStream stream)
    {
      stream.Serialize(ref CharacterPrototype);
      stream.Serialize(ref Camera);
      stream.SerializeArray(ref Weapons, delegate(ref AssetRefWeaponData it) { SerializeWeaponData(stream, ref it); });
    }

    public void SerializeWeaponData(BitStream stream, ref AssetRefWeaponData weaponData)
    {
      stream.Serialize(ref weaponData);
    }
}
}
