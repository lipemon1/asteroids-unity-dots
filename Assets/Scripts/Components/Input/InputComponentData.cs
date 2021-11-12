using Unity.Entities;

namespace Asteroids.Components
{
    public struct InputComponentData : IComponentData
    {
        public bool InputLeft;
        public bool InputRight;
        public bool InputForward;
        public bool InputShoot;
    }
}
