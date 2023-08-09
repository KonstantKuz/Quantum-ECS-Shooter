
namespace Quantum.App.Combat.Animation
{
    public class AttackCommand : AnimationCommand
    {
        public unsafe Quantum.Weapon* GetWeapon(Frame frame)
        {
            var inventory = frame.Unsafe.GetPointer<Quantum.Inventory>(Sender);
            var weapon = frame.Unsafe.GetPointer<Quantum.Weapon>(inventory->ActiveItem.EntityRef);
            return weapon;
        }

        public Aim GetAim(Frame frame)
        {
            return frame.Get<Aim>(Sender);
        }
    }
}