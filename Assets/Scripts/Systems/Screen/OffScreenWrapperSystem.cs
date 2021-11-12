using System;
using Asteroids.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Asteroids.Systems
{
    public class OffScreenWrapperSystem : SystemBase
    {
        enum Direction
        {
            Right,
            Down,
            Left,
            Up
        }
        
        protected override void OnUpdate()
        {
            ScreenInfoComponentData screenDataComponent = GetSingleton<ScreenInfoComponentData>();

            Entities.WithAll<OffScreenWrapperComponentData>().ForEach(
                (
                    Entity entity,
                    ref OffScreenWrapperComponentData offScreenWrapperComponentData,
                    ref Translation translation
                ) =>
                {
                    if (offScreenWrapperComponentData.IsOffScreenLeft)
                        translation.Value = SpawnOnOppositeDirection(translation.Value, offScreenWrapperComponentData.RendererSize.x, screenDataComponent, Direction.Right);
                    else if (offScreenWrapperComponentData.IsOffScreenRight)
                        translation.Value = SpawnOnOppositeDirection(translation.Value, offScreenWrapperComponentData.RendererSize.x,
                            screenDataComponent, Direction.Left);
                    else if (offScreenWrapperComponentData.IsOffScreenUp)
                        translation.Value = SpawnOnOppositeDirection(translation.Value, offScreenWrapperComponentData.RendererSize.y,
                            screenDataComponent, Direction.Down);
                    else if (offScreenWrapperComponentData.IsOffScreenDown)
                        translation.Value = SpawnOnOppositeDirection(translation.Value, offScreenWrapperComponentData.RendererSize.y,
                            screenDataComponent, Direction.Up);

                    offScreenWrapperComponentData.IsOffScreenDown = false;
                    offScreenWrapperComponentData.IsOffScreenRight = false;
                    offScreenWrapperComponentData.IsOffScreenUp = false;
                    offScreenWrapperComponentData.IsOffScreenLeft = false;

                }).ScheduleParallel();
        }

        static float3 SpawnOnOppositeDirection(float3 position, float bounds,
            ScreenInfoComponentData screenInfoComponentData, Direction wantedDirection)
        {
            switch (wantedDirection)
            {
                case Direction.Right:
                    return new float3((bounds + screenInfoComponentData.Width) * .5f, position.y, 0);
                case Direction.Down:
                    return new float3(position.x, -(bounds + screenInfoComponentData.Height) * .5f, 0);
                case Direction.Left:
                    return new float3(-(bounds + screenInfoComponentData.Width) * .5f, position.y, 0);
                case Direction.Up:
                    return new float3(position.x, (bounds + screenInfoComponentData.Height) * .5f, 0);
                default:
                    throw new ArgumentOutOfRangeException(nameof(wantedDirection), wantedDirection, null);
            }
        }
    }   
}
