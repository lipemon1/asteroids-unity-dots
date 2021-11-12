using Unity.Entities;

namespace Asteroids.Components
{
    [GenerateAuthoringComponent]
    public struct MovementParametersComponentData : IComponentData
    {
        public float LinearVelocity;
        public float MaxLinearVelocity;
        public float AngularVelocity;
        public float MaxAngularVelocity;
    }   
}
