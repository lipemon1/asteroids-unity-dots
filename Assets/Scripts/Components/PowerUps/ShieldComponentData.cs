using Unity.Entities;

namespace Asteroids.Components
{
    [GenerateAuthoringComponent]
    public struct ShieldComponentData : IComponentData
    {
        public float ShieldTime;
    }   
}
