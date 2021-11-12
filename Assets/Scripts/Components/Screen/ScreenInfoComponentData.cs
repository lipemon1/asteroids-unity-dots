using Unity.Entities;

namespace Asteroids.Components
{
    public struct ScreenInfoComponentData : IComponentData
    {
        public float Height;
        public float Width;
    }   
}