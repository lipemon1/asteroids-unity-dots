using Asteroids.Components;
using Unity.Entities;

namespace Asteroids.Systems
{
    public class InvencibleShieldSystem : SystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            
            Entities
                .WithAll<InvencibilityComponentData>()
                .ForEach(
                    (
                        Entity entity,
                        ref InvencibilityComponentData invencibilityComponentData
                    ) =>
                    {
                        invencibilityComponentData.Active = false;
                    }).Run();
        }

        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;

            Entities
                .WithAll<InvencibilityComponentData>()
                .ForEach(
                    (
                        Entity entity,
                        ref InvencibilityComponentData invencibilityComponentData,
                        ref DestroyableComponentData destroyableComponentData
                    ) =>
                    {
                        if (!invencibilityComponentData.Active) return;

                        float curTime = invencibilityComponentData.CurTime;
                        curTime += deltaTime;

                        if (curTime > invencibilityComponentData.MaxTime)
                        {
                            invencibilityComponentData.CurTime = 0;
                            invencibilityComponentData.Active = false;
                            destroyableComponentData.MustBeDestroyed = false;
                        }
                    }).Run();
        }
    }   
}
