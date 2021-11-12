using Unity.Mathematics;
using Random = UnityEngine.Random;

namespace Asteroids.Spawners
{
    public class AsteroidSpawner : Spawner
    {
        protected override float3 GetRotationToSpawn()
        {
            return math.normalize(new float3(0, 0, Random.value));
        }
    }   
}
