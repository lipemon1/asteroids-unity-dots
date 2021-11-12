using Asteroids.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;

namespace Asteroids.Systems
{
    public class MoveSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((
                ref MovementCommandsComponentData movementCommandsComponentData,
                ref PhysicsVelocity velocity,
                in MovementParametersComponentData movementParametersComponentData,
                in PhysicsMass physicsMass,
                in Translation translation
            ) =>
            {
                PhysicsComponentExtensions.ApplyLinearImpulse(
                    ref velocity,
                    physicsMass,
                    movementCommandsComponentData.CurrentDirectionOfMove * movementCommandsComponentData.CurrentLinearCommand * movementParametersComponentData.LinearVelocity
                    );

                if (math.length(velocity.Linear) > movementParametersComponentData.MaxLinearVelocity)
                    velocity.Linear = math.normalize(velocity.Linear) *
                                      movementParametersComponentData.MaxLinearVelocity;

            }).ScheduleParallel();
        }
    }   
}
