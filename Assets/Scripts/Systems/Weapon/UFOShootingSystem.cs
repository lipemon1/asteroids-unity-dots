using Asteroids.Components;
using Asteroids.Components.Tags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Asteroids.Systems
{
    public class UFOShootingSystem : SystemBase
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
            
            EntityCommandBuffer.ParallelWriter ecb = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer()
                .AsParallelWriter();
            
            float3 local = new float3(0,0,0);
            
            Entities
                .WithAll<PlayerTagComponentData>()
                .ForEach(
                (
                    in Translation translation    
                ) =>
                {
                    local = translation.Value;
                }).Run();

            Entities
                .WithAll<UFOTagComponentData>()
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
                    
                    if (!(weaponComponentData.Timer > weaponComponentData.FireRate)) return;
                    weaponComponentData.Timer = 0;
                    
                    Entity newProjectile =
                        ecb.Instantiate(entityInQueryIndex, weaponComponentData.ProjectilePrefab);

                    ecb.SetComponent(entityInQueryIndex, newProjectile, new Translation
                    {
                        Value = translation.Value
                    });

                    float3 dir = local - translation.Value;
                    
                    ecb.SetComponent(entityInQueryIndex, newProjectile, new Rotation
                    {
                        Value = Quaternion.LookRotation(Vector3.forward, dir)
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
