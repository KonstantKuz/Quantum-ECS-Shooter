using Photon.Deterministic;

namespace Quantum {
  partial class RuntimePlayer
  {
    public AssetRefEntityPrototype CharacterPrototype;
    public AssetRefEntityPrototype Camera;
    partial void SerializeUserData(BitStream stream)
    {
      stream.Serialize(ref CharacterPrototype);
      stream.Serialize(ref Camera);
    }
  }
}
