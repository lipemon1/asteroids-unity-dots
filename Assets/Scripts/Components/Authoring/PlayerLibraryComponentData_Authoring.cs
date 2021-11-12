using System.Collections.Generic;
using Asteroids.Initialization;
using Unity.Entities;
using UnityEngine;

namespace Asteroids.Components.Authoring
{
    public class PlayerLibraryComponentData_Authoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        [SerializeField] Bootstrap _bootstrap;
        [SerializeField] GameObject _spaceshipPrefab;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new SpaceshipLibraryElementComponentData
            {
                Spaceship = conversionSystem.GetPrimaryEntity(_spaceshipPrefab)
            });

            _bootstrap.PlayerSpaceshipEntity = entity;
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(_spaceshipPrefab);
        }
    }

    public struct SpaceshipLibraryElementComponentData : IComponentData
    {
        public Entity Spaceship { get; set; }
    }
}
