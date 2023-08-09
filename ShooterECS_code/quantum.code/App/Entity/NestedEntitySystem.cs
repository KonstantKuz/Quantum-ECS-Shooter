using Photon.Deterministic;

namespace Quantum.App.Entity
{
    public class NestedEntitySystem : SystemMainThreadFilter<NestedEntitySystem.NestedEntityFilter>
    {
        public unsafe struct NestedEntityFilter
        {
            public EntityRef EntityRef;
            public NestedEntity* NestedEntity;
        }
        
        public override unsafe void Update(Frame f, ref NestedEntityFilter filter)
        {
            var transform = f.Unsafe.GetPointer<Transform3D>(filter.EntityRef);
            var localPosition = filter.NestedEntity->LocalPosition;
            var localRotation = FPQuaternion.Euler(filter.NestedEntity->LocalRotation);
            if (f.Exists(filter.NestedEntity->Parent))
            {
                var parent = f.Unsafe.GetPointer<Transform3D>(filter.NestedEntity->Parent);
                transform->Position = parent->TransformPoint(localPosition);
                transform->Rotation = parent->Rotation * localRotation;
            }
        }
    }
}