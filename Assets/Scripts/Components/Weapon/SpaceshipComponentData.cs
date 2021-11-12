using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Components
{
    public struct SpaceshipComponentData : ISharedComponentData
    {
        public float3 CurrentPlayerPosition;
    }    
}
