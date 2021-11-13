using Asteroids.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Asteroids.Systems
{
    public class OffScreenDetectionSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            ScreenInfoComponentData screenData = GetSingleton<ScreenInfoComponentData>();

            Entities.ForEach(
                (
                Entity entity,
                ref OffScreenWrapperComponentData offScreenWrapperComponentData,
                ref MovementCommandsComponentData movementCommandsComponentData,
                in Translation translation,
                in PhysicsVelocity physicsVelocity
                ) =>
                {
                    bool isMovingLeft = physicsVelocity.Linear.x < 0;
                    bool isMovingRight = physicsVelocity.Linear.x > 0;
                    bool isMovingUp = physicsVelocity.Linear.y > 0;
                    bool isMovingDown = physicsVelocity.Linear.y < 0;

                    movementCommandsComponentData.IsMovingLeft = isMovingLeft;
                    movementCommandsComponentData.IsMovingRight = isMovingRight;
                    movementCommandsComponentData.IsMovingUp = isMovingUp;
                    movementCommandsComponentData.IsMovingDown = isMovingDown;

                    //left
                    offScreenWrapperComponentData.IsOffScreenLeft =
                        translation.Value.x < -(screenData.Width + offScreenWrapperComponentData.RendererSize.x) * .5f &&
                        isMovingLeft;

                    //right
                    offScreenWrapperComponentData.IsOffScreenRight =
                        translation.Value.x > (screenData.Width + offScreenWrapperComponentData.RendererSize.x) * .5f &&
                        isMovingRight;
                    
                    //up
                    offScreenWrapperComponentData.IsOffScreenUp =
                        translation.Value.y > (screenData.Height + offScreenWrapperComponentData.RendererSize.y) * .5f &&
                        isMovingUp;
                    
                    //down
                    offScreenWrapperComponentData.IsOffScreenDown =
                        translation.Value.y < -(screenData.Height + offScreenWrapperComponentData.RendererSize.y) * .5f &&
                        isMovingDown;

                }).ScheduleParallel();
        }
    }   
}
