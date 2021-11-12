using Asteroids.Components;
using Asteroids.Components.Tags;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Systems
{
    public class PlayerControlSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            EntityQuery query = GetEntityQuery(typeof(InputComponentData));
            NativeArray<InputComponentData> array = query.ToComponentDataArray<InputComponentData>(Allocator.TempJob);

            InputComponentData inputData = array[0];

            Entities.WithAll<PlayerTagComponentData>().ForEach(
                (
                    ref MovementCommandsComponentData movementCommandsComponentData,
                    ref WeaponComponentData weaponComponentData
                    ) =>
                {
                    int turningLeft = inputData.InputLeft ? 1 : 0;
                    int turningRight = inputData.InputRight ? 1 : 0;

                    int rotationDirection = turningLeft - turningRight;

                    weaponComponentData.IsFiring = inputData.InputShoot;

                    movementCommandsComponentData.CurrentAngularCommand = new float3(0, 0, rotationDirection);
                    movementCommandsComponentData.CurrentLinearCommand = inputData.InputForward ? 1 : 0;
                    
                }).ScheduleParallel();
        }
    }   
}
