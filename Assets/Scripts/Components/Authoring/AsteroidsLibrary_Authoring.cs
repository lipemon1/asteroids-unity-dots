using System.Collections.Generic;
using Asteroids.Initialization;
using Unity.Entities;
using UnityEngine;

namespace Asteroids.Components.Authoring
{
    public class AsteroidsLibrary_Authoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        [SerializeField] Bootstrap _bootstrap;
        [SerializeField] GameObject[] _asteroidsPrefabs;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            DynamicBuffer<EntityBufferElement> buffer = dstManager.AddBuffer<EntityBufferElement>(entity);

            foreach (GameObject prefab in _asteroidsPrefabs)
            {
                buffer.Add(new EntityBufferElement()
                {
                    Entity = conversionSystem.GetPrimaryEntity(prefab)
                });
            }
            
            _bootstrap.AsteroidSpawner.Library = entity;
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.AddRange(_asteroidsPrefabs);
        }
    }

    public struct EntityBufferElement : IBufferElementData
    {
        public Entity Entity;
    }
}
