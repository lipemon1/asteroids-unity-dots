using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Components
{
    [GenerateAuthoringComponent]
    public struct MovementCommandsComponentData : IComponentData
    {
        public float3 CurrentDirectionOfMove;
        public float3 CurrentAngularCommand;
        public float CurrentLinearCommand;
        
        public bool IsMovingRight;
        public bool IsMovingLeft;
        public bool IsMovingDown;
        public bool IsMovingUp;
    }
}
