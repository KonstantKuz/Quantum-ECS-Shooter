using System;
using System.Collections.Generic;
using System.Linq;
using Quantum.App.Combat.Weapon;
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
        }

        private static void InitInventory(Frame frame, EntityRef player, RuntimePlayer runtimeData)
        {
            var inventory = frame.Unsafe.GetPointer<Quantum.Inventory>(player);
            foreach (var data in runtimeData.Weapons)
            {
                var weapon = WeaponFactory.CreateWeapon(frame, data);
                var item = new Item {Id = data.Id.Value, EntityRef = weapon};
                frame.Add(weapon, item);
                (*inventory).Add(frame, item);
                if (inventory->ActiveItem.Equals(Item.None))
                {
                    inventory->ActiveItem = item;
                }
            }
        }

        private void PlacePlayer(Frame frame, EntityRef player)
        {
            var emptyPoint = GetSpawnPoints(frame)
                .First(it => !it.Component.Data.IsBusy);
            if (frame.Unsafe.TryGetPointer<Transform3D>(player, out var transform)
                && frame.Unsafe.TryGetPointer<Transform3D>(emptyPoint.Entity, out var spawnPoint))
            {
                emptyPoint.Component.Data.IsBusy = true;
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