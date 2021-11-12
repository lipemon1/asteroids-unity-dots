using Asteroids.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Asteroids.Systems
{
    public class OnPhysicsTriggerJobSystem : JobComponentSystem
    {
        BuildPhysicsWorld _buildPhysicsWorld;
        StepPhysicsWorld _stepPhysicsWorld;

        protected override void OnCreate()
        {
            base.OnCreate();
            _buildPhysicsWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BuildPhysicsWorld>();
            _stepPhysicsWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<StepPhysicsWorld>();
        }

        [BurstCompile]
        struct TriggerJob : ITriggerEventsJob
        {
            public ComponentDataFromEntity<DestroyableComponentData> Destroyables;
            
            public void Execute(TriggerEvent triggerEvent)
            {
                if (Destroyables.HasComponent(triggerEvent.EntityA))
                {
                    DestroyableComponentData destroyable = Destroyables[triggerEvent.EntityA];
                    destroyable.MustBeDestroyed = true;
                    Destroyables[triggerEvent.EntityA] = destroyable;
                }

                if (Destroyables.HasComponent(triggerEvent.EntityB))
                {
                    DestroyableComponentData destroyable = Destroyables[triggerEvent.EntityB];
                    destroyable.MustBeDestroyed = true;
                    Destroyables[triggerEvent.EntityB] = destroyable;
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            ComponentDataFromEntity<DestroyableComponentData> destroyables = GetComponentDataFromEntity<DestroyableComponentData>();

            TriggerJob job = new TriggerJob
            {
                Destroyables = destroyables
            };

            JobHandle jobHandle = job.Schedule(_stepPhysicsWorld.Simulation, ref _buildPhysicsWorld.PhysicsWorld, inputDeps);
            jobHandle.Complete();
            return jobHandle;
        }
    }   
}
