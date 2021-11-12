using Asteroids.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;

namespace Asteroids.Systems
{
    public class RotateSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((
                ref PhysicsVelocity velocity,
                ref MovementCommandsComponentData movementCommandsComponentData,
                ref Rotation rotation,
                in MovementParametersComponentData movementParametersComponentData,
                in PhysicsMass physicsMass
                ) =>
            {
                PhysicsComponentExtensions.ApplyAngularImpulse(
                    ref velocity,
                    physicsMass,
                    movementCommandsComponentData.CurrentAngularCommand * movementParametersComponentData.AngularVelocity
                    );

                float3 currentAngularSpeed =
                    PhysicsComponentExtensions.GetAngularVelocityWorldSpace(in velocity, in physicsMass, in rotation);

                if (math.length(currentAngularSpeed) > movementParametersComponentData.MaxAngularVelocity)
                {
                    PhysicsComponentExtensions.SetAngularVelocityWorldSpace(ref velocity, physicsMass, rotation, math.normalize(currentAngularSpeed) * movementParametersComponentData.MaxAngularVelocity);
                }

            }).Schedule();
        }
    }   
}
