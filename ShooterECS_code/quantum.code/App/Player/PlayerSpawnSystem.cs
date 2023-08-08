using System;
using System.Collections.Generic;
using System.Linq;
using Quantum.App.Entity;

namespace Quantum.App.Player
{
    public unsafe class PlayerSpawnSystem : SystemSignalsOnly, ISignalOnPlayerDataSet
    {
        public void OnPlayerDataSet(Frame frame, PlayerRef player)
        {
            var data = frame.GetPlayerData(player);
            var entity = CreatePlayer(frame, player, data);
            SetupCamera(frame, player, entity, data);
            PlacePlayer(frame, entity);
            InitInventory(frame, entity, data);
        }

        private static EntityRef CreatePlayer(Frame frame, PlayerRef player, RuntimePlayer data)
        {
            var entity = EntityFactory.Create(frame, data.CharacterPrototype.Id);
            frame.Add(entity,  new PlayerTag {Player = player});
            return entity;
        }

        private static void SetupCamera(Frame frame, PlayerRef playerRef, EntityRef player, RuntimePlayer data)
        {
            var camera = EntityFactory.Create(frame, data.Camera.Id);
            var playerCamera = frame.Unsafe.GetPointer<PlayerCamera>(camera);
            playerCamera->PlayerRef = playerRef;
            playerCamera->PlayerEntity = player;
            frame.Unsafe.GetPointer<NestedEntity>(camera)->Parent = player;
        }

        private static void InitInventory(Frame frame, EntityRef player, RuntimePlayer runtimeData)
        {
            var inventory = frame.Unsafe.GetPointer<Quantum.Inventory>(player);
            foreach (var data in runtimeData.Weapons)
            {
                var weaponData = frame.FindAsset<WeaponData>(data.Id);
                var weaponEntity = frame.Create();
                var weapon = new Weapon {Data = weaponData};
                frame.Add(weaponEntity, weapon);
                var item = new Item {Id = weaponData.Id, EntityRef = weaponEntity};
                frame.Add(weaponEntity, item);
                (*inventory).Add(frame, item);
                if (inventory->ActiveItem.Equals(Item.None))
                {
                    inventory->ActiveItem = item;
                }
            }
        }

        private void PlacePlayer(Frame frame, EntityRef player)
        {
            var spawnPoints = GetSpawnPoints(frame)
                .Where(it => !it.Component.Data.IsBusy).ToArray();
            var rndIdx = new Random().Next(0, spawnPoints.Length - 1);
            var rndPoint = spawnPoints[rndIdx];
            // Offset the instantiated object in the world, based in its ID.
            if (frame.Unsafe.TryGetPointer<Transform3D>(player, out var transform)
                && frame.Unsafe.TryGetPointer<Transform3D>(rndPoint.Entity, out var spawnPoint))
            {
                rndPoint.Component.Data.IsBusy = true;
                transform->Position = spawnPoint->Position;
            }
        }

        private IEnumerable<EntityComponentPair<SpawnPoint>> GetSpawnPoints(Frame frame)
        {
            var iterator = frame.GetComponentIterator<SpawnPoint>().GetEnumerator();
            while (iterator.MoveNext())
            {
                yield return iterator.Current;
            }
        }
    }
}