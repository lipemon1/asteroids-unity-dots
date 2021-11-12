using Asteroids.Components;
using Unity.Entities;
using Unity.Transforms;

namespace Asteroids.Systems
{
    public class WeaponSystem : SystemBase
    {
        EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;
        
        protected override void OnCreate()
        {
            base.OnCreate();
            _endSimulationEntityCommandBufferSystem = World.DefaultGameObjectInjectionWorld
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;
            EntityCommandBuffer.ParallelWriter ecb = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();

            Entities
                .ForEach(
                    (
                        Entity entity,
                        int entityInQueryIndex,
                        ref WeaponComponentData weaponComponentData,
                        in Translation translation,
                        in Rotation rotation
                    ) =>
            {
                weaponComponentData.Timer += deltaTime;

                if (!weaponComponentData.IsFiring) return;
                if (!(weaponComponentData.Timer > weaponComponentData.FireRate)) return;

                weaponComponentData.Timer = 0;
                Entity newProjectile = ecb.Instantiate(entityInQueryIndex, weaponComponentData.ProjectilePrefab);
                
                ecb.SetComponent(entityInQueryIndex, newProjectile, new Translation
                {
                    Value = translation.Value
                });
                
                ecb.SetComponent(entityInQueryIndex, newProjectile, new Rotation
                {
                    Value = rotation.Value
                });
                
                ecb.SetComponent(entityInQueryIndex, newProjectile, new MovementCommandsComponentData
                {
                    CurrentLinearCommand = 1
                });
                
            }).ScheduleParallel();
            _endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(this.Dependency);
        }
    }   
}
