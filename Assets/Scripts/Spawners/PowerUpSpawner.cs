using Unity.Mathematics;

namespace Asteroids.Spawners
{
    public class PowerUpSpawner : Spawner
    {
        protected override float3 GetRotationToSpawn()
        {
            return 0;
        }
    }   
}
