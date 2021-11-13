using Asteroids.Components;
using Unity.Entities;

namespace Asteroids.Systems
{
    public class ShieldPowerUpDestructionSystem : SystemBase
    {
        EntityManager _entityManager;

        protected override void OnCreate()
        {
            base.OnCreate();
            _entityManager = World.EntityManager;
        }
        
        protected override void OnUpdate()
        {
            Entities
                .WithoutBurst()
                .WithStructuralChanges()
                .WithAll<ShieldComponentData>()
                .ForEach(
                    (
                        Entity entity,
                        in DestroyableComponentData destroyableComponentData,
                        in ShieldComponentData shieldComponentData
                    ) =>
                    {
                        if (destroyableComponentData.MustBeDestroyed)
                        {
                            _entityManager.DestroyEntity(entity);
                        }
                    }).Run();
        }
    }   
}
