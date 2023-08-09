namespace Quantum.App.Combat.Weapon
{
    public class WeaponFactory
    {
        public static EntityRef CreateWeapon(Frame frame, AssetRefWeaponData data)
        {
            var weaponData = frame.FindAsset<WeaponData>(data.Id);
            var weaponEntity = frame.Create();
            var weapon = new Quantum.Weapon {Data = weaponData};
            frame.Add(weaponEntity, weapon);
            return weaponEntity;
        }
    }
}