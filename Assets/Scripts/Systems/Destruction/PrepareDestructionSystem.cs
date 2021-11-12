using Asteroids.Components;
using Unity.Entities;

namespace Asteroids.Systems
{
    public class PrepareDestructionSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;
            Entities.ForEach(
                (
                    Entity entity,
                    ref LifetimeComponentData lifetimeComponentData,
                    ref DestroyableComponentData destroyableComponentData
                ) =>
                {
                    lifetimeComponentData.CurrentLifetime += deltaTime;
                    if (lifetimeComponentData.CurrentLifetime > lifetimeComponentData.MaxLifetime &&
                        !destroyableComponentData.MustBeDestroyed)
                    {
                        destroyableComponentData.MustBeDestroyed = true;
                    }

                }).ScheduleParallel();
        }
    }   
}