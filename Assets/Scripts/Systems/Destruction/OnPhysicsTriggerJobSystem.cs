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
            public ComponentDataFromEntity<ShieldComponentData> ShieldComponents;
            public ComponentDataFromEntity<InvencibilityComponentData> InvencibilityComponents;
            
            public void Execute(TriggerEvent triggerEvent)
            {
                Entity entityA = triggerEvent.EntityA;
                Entity entityB = triggerEvent.EntityB;
                
                bool entityAHasShield = ShieldComponents.HasComponent(entityA);
                bool entityBHasShield = ShieldComponents.HasComponent(entityB);
                
                if (entityAHasShield || entityBHasShield)
                {
                    //someone is a shield
                    if (entityBHasShield)
                    {
                        EnableInvencibility(entityA, entityB);
                        MarkAsDestroyable(entityB);
                    }
                    else if(entityAHasShield)
                    {
                        EnableInvencibility(entityB, entityA);
                        MarkAsDestroyable(entityA);
                    }
                }
                else
                {
                    //normal destruction
                    TryMarkToDestroy(ref triggerEvent);
                }
            }

            void EnableInvencibility(Entity invencibilityEntity, Entity shieldEntity)
            {
                float shieldValue = ShieldComponents[shieldEntity].ShieldTime;
                InvencibilityComponentData invencibility = InvencibilityComponents[invencibilityEntity];
                
                invencibility.CurTime = 0;
                invencibility.MaxTime = shieldValue;
                invencibility.Active = true;

                InvencibilityComponents[invencibilityEntity] = invencibility;
            }

            void TryMarkToDestroy(ref TriggerEvent triggerEvent)
            {
                if (Destroyables.HasComponent(triggerEvent.EntityA))
                    MarkAsDestroyable(triggerEvent.EntityA);

                if (Destroyables.HasComponent(triggerEvent.EntityB))
                    MarkAsDestroyable(triggerEvent.EntityB);
            }

            void MarkAsDestroyable(Entity targetEntity)
            {
                DestroyableComponentData destroyable = Destroyables[targetEntity];
                destroyable.MustBeDestroyed = true;
                Destroyables[targetEntity] = destroyable;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            ComponentDataFromEntity<DestroyableComponentData> destroyables = GetComponentDataFromEntity<DestroyableComponentData>();
            ComponentDataFromEntity<ShieldComponentData> shields = GetComponentDataFromEntity<ShieldComponentData>();
            ComponentDataFromEntity<InvencibilityComponentData> invencibilityComponents = GetComponentDataFromEntity<InvencibilityComponentData>();

            TriggerJob job = new TriggerJob
            {
                Destroyables = destroyables,
                ShieldComponents = shields,
                InvencibilityComponents = invencibilityComponents
            };

            JobHandle jobHandle = job.Schedule(_stepPhysicsWorld.Simulation, ref _buildPhysicsWorld.PhysicsWorld, inputDeps);
            jobHandle.Complete();
            return jobHandle;
        }
    }
}
