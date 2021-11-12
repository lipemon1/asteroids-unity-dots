using Asteroids.Components;
using Asteroids.Components.Authoring;
using Asteroids.Initialization;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.Spawners
{
    public abstract class Spawner : MonoBehaviour
    {
        Bootstrap _bootstrap;
        public Entity Library;
        
        [SerializeField] Transform[] _spawnTransforms;
        Vector3[] _spawnPositions;
        
        float _currentTimer;
        [SerializeField] float _spawnFrequency = 2f;
        
        void Awake()
        {
            _spawnPositions = new Vector3[_spawnTransforms.Length];
            
            for (int i = 0; i < _spawnTransforms.Length; i++)
            {
                _spawnPositions[i] = _spawnTransforms[i].position;
            }
        }
        
        void Start()
        {
            _bootstrap = Bootstrap.Instance;
        }
        
        void Update()
        {
            _currentTimer += Time.deltaTime;

            if (_currentTimer > _spawnFrequency)
            {
                _currentTimer = 0;
                SpawnUFO();
            }
        }
        
        void SpawnUFO()
        {
            DynamicBuffer<EntityBufferElement> buffer =
                _bootstrap.GetEntityManager().GetBuffer<EntityBufferElement>(Library);
                 
            int lenghtOfBuffer = buffer.Length;
            int randomAsteroidIndex = Random.Range(0, lenghtOfBuffer);
            Entity newAsteroid = _bootstrap.GetEntityManager().Instantiate(buffer[randomAsteroidIndex].Entity);

            int randomSpawnPositionIndex = Random.Range(0, _spawnPositions.Length);
            Vector3 spawnPosition = _spawnPositions[randomSpawnPositionIndex];

            _bootstrap.GetEntityManager().SetComponentData(newAsteroid, new Translation()
            {
                Value = spawnPosition
            });
            
            float3 randomMoveDirection =
                math.normalize(new float3(Random.Range(-.8f, .8f), Random.Range(-.8f, .8f), 0));
            float3 randomRotation = GetRotationToSpawn();

            _bootstrap.GetEntityManager().SetComponentData(newAsteroid, new MovementCommandsComponentData()
            {
                CurrentDirectionOfMove = randomMoveDirection,
                CurrentLinearCommand = 1,
                CurrentAngularCommand = randomRotation,
                IsMovingDown = false,
                IsMovingLeft = false,
                IsMovingRight = false,
                IsMovingUp = false
            });
        }

        protected abstract float3 GetRotationToSpawn();
    }   
}
