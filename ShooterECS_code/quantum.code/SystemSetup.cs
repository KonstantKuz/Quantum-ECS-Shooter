using Quantum.App.Combat.Damageable;
using Quantum.App.Combat.Weapon;
using Quantum.App.Entity;
using Quantum.App.Inventory;
using Quantum.App.Player;
using Quantum.App.Player.Camera;

namespace Quantum {
  public static class SystemSetup {
    public static SystemBase[] CreateSystems(RuntimeConfig gameConfig, SimulationConfig simulationConfig) {
      return new SystemBase[] {
        // pre-defined core systems
        new Core.CullingSystem3D(),
        
        new Core.PhysicsSystem3D(),

        Core.DebugCommand.CreateSystem(),

        new Core.NavigationSystem(),
        new Core.EntityPrototypeSystem(),
        new Core.PlayerConnectedSystem(),

        // user systems go here 
        new NestedEntitySystem(),
        new PlayerSpawnSystem(),
        new PlayerMovementSystem(),
        new CameraRotationSystem(),
        new PlayerAimSystem(),
        new InventorySystem(),
        new WeaponSystem(),
        new PlayerAttackSystem(),
        new DamageSystem(),
      };
    }
  }
}
