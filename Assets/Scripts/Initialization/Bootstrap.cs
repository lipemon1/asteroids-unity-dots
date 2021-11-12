using Asteroids.Components;
using Asteroids.Components.Authoring;
using Asteroids.Components.Tags;
using Asteroids.Spawners;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.Initialization
{
    public class Bootstrap : MonoBehaviour
    {
        public static Bootstrap Instance { get; private set; }
        
        EntityManager _entityManager;

        public Entity PlayerSpaceshipEntity;

        ValidateSpawnPositionJob _validateSpawnPositionJob;
        JobHandle _jobHandle;
    
        public AsteroidSpawner AsteroidSpawner;
        public UFOSpawner UFOSpawner;
        public PowerUpSpawner PowerUpSpawner;
        void Awake()
        {
            Instance = this;
            
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        void Start()
        {
            _jobHandle = new JobHandle();
            _validateSpawnPositionJob = new ValidateSpawnPositionJob();
            
            _entityManager.CreateEntity(typeof(InputComponentData));
            SpawnSpaceshipAtPosition(Vector3.zero);
        }

        public void LookForPlayerSpawnPosition()
        {
            bool lookingForPosition = true;
            EntityQuery screenInfoQuery = _entityManager.CreateEntityQuery(typeof(ScreenInfoComponentData));
            Entity screenInfoEntity = screenInfoQuery.GetSingletonEntity();
            ScreenInfoComponentData screenInfoComponent = _entityManager.GetComponentData<ScreenInfoComponentData>(screenInfoEntity);

            float screenHalfWidth = screenInfoComponent.Width * .5f;
            float screenHalfHeight = screenInfoComponent.Height * .5f;

            EntityQuery asteroidsQuery =
                _entityManager.CreateEntityQuery(typeof(AsteroidTagComponent), ComponentType.ReadOnly<Translation>());
            NativeArray<Translation> translationComponentsOfAllAsteroids =
                asteroidsQuery.ToComponentDataArray<Translation>(Allocator.TempJob);

            NativeArray<bool> isSpawnPositionValid = new NativeArray<bool>(1, Allocator.TempJob);
            
            Vector3 possibleSpawnPosition = new Vector3(Random.Range(-screenHalfWidth, screenHalfWidth),
                Random.Range(-screenHalfHeight, screenHalfHeight), 0);

            while (lookingForPosition)
            {
                _validateSpawnPositionJob.Translations = translationComponentsOfAllAsteroids;
                _validateSpawnPositionJob.PossibleSpawnPosition = possibleSpawnPosition;
                _validateSpawnPositionJob.MinimalSpawnDistance = 5f;
                _validateSpawnPositionJob.Result = isSpawnPositionValid;

                _jobHandle = _validateSpawnPositionJob.Schedule();
                _jobHandle.Complete();

                if (isSpawnPositionValid[0])
                    lookingForPosition = false;
                else
                    possibleSpawnPosition = new Vector3(Random.Range(-screenHalfWidth, screenHalfWidth),
                        Random.Range(-screenHalfHeight, screenHalfHeight), 0);
            }

            isSpawnPositionValid.Dispose();
            translationComponentsOfAllAsteroids.Dispose();
            
            SpawnSpaceshipAtPosition(possibleSpawnPosition);
        }

        void SpawnSpaceshipAtPosition(Vector3 spawnPosition)
        {
            SpaceshipLibraryElementComponentData shipLibraryReference =
                _entityManager.GetComponentData<SpaceshipLibraryElementComponentData>(PlayerSpaceshipEntity);
            Entity playerShip = _entityManager.Instantiate(shipLibraryReference.Spaceship);
            
            _entityManager.SetComponentData(playerShip, new Translation
            {
                Value = spawnPosition
            });
        }

        public EntityManager GetEntityManager()
        {
            return _entityManager;
        }
    }
    
    public struct ValidateSpawnPositionJob : IJob
    {
        public NativeArray<Translation> Translations;
        public float3 PossibleSpawnPosition;
        public float MinimalSpawnDistance;
        public NativeArray<bool> Result;
        
        public void Execute()
        {
            bool result = true;

            foreach (Translation translation in Translations)
            {
                if (math.distancesq(translation.Value, PossibleSpawnPosition) <
                    MinimalSpawnDistance * MinimalSpawnDistance)
                {
                    result = false;
                    break;
                }
            }

            Result[0] = result;
        }
    }
}