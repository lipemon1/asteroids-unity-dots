using Unity.Entities;

namespace Asteroids.Components
{
    [GenerateAuthoringComponent]
    public struct LifetimeComponentData : IComponentData
    {
        public float MaxLifetime;
        public float CurrentLifetime;
    }   
}
