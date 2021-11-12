using System.Collections.Generic;
using Asteroids.Initialization;
using Unity.Entities;
using UnityEngine;

namespace Asteroids.Components.Authoring
{
    public class UFOLibrary_Authoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        [SerializeField] Bootstrap _bootstrap;
        [SerializeField] GameObject[] _ufoPrefab;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            DynamicBuffer<EntityBufferElement> buffer = dstManager.AddBuffer<EntityBufferElement>(entity);

            foreach (GameObject prefab in _ufoPrefab)
            {
                buffer.Add(new EntityBufferElement()
                {
                    Entity = conversionSystem.GetPrimaryEntity(prefab)
                });
            }
            
            _bootstrap.UFOSpawner.Library = entity;
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.AddRange(_ufoPrefab);
        }
    }
}
