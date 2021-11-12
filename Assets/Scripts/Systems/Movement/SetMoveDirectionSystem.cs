using Asteroids.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Asteroids.Systems
{
    public class SetMoveDirectionSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.WithAll<MovingUpDirectionComponentData>().ForEach((
                ref MovementCommandsComponentData movementCommandsComponentData,
                in Rotation rotation
                ) =>
            {
                float3 direction = math.mul(rotation.Value, math.up());
                movementCommandsComponentData.CurrentDirectionOfMove = direction;
                
            }).Schedule();
        }
    }   
}
