using Unity.Mathematics;

namespace Asteroids.Spawners
{
    public class UFOSpawner : Spawner
    {
        protected override float3 GetRotationToSpawn()
        {
            return 0;
        }
    }   
}
