using Unity.Entities;

namespace Asteroids.Components
{
    [GenerateAuthoringComponent]
    public struct DestroyableComponentData : IComponentData
    {
        public bool MustBeDestroyed;
    }
}
