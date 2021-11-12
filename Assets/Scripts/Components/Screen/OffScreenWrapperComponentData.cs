using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Asteroids.Components
{
    public struct OffScreenWrapperComponentData : IComponentData
    {
        public float2 RendererSize;
        public bool IsOffScreenLeft;
        public bool IsOffScreenRight;
        public bool IsOffScreenDown;
        public bool IsOffScreenUp;
    }   
}
