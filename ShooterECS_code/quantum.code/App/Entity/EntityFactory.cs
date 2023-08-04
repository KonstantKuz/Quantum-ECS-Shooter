namespace Quantum.App.Entity
{
    public class EntityFactory
    {
        public static EntityRef Create(Frame frame, AssetGuid prototypeId)
        {
            // resolve the reference to the prototpye.
            var prototype = frame.FindAsset<EntityPrototype>(prototypeId);

            // Create a new entity for the player based on the prototype.
            var entity = frame.Create(prototype);
            
            return entity;
        }
    }
}