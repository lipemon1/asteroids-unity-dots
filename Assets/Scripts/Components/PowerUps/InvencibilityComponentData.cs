using Unity.Entities;

namespace Asteroids.Components
{
    [GenerateAuthoringComponent]
    public struct InvencibilityComponentData : IComponentData
    {
        public float CurTime;
        public float MaxTime;
        public bool Active;
    }   
}
