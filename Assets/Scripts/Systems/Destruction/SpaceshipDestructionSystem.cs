using Asteroids.Components;
using Asteroids.Components.Tags;
using Asteroids.Initialization;
using Unity.Entities;

namespace Asteroids.Systems
{
    public class SpaceshipDestructionSystem : SystemBase
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
                .WithStructuralChanges()
                .WithoutBurst()
                .WithAll<PlayerTagComponentData>()
                .ForEach(
                    (
                        Entity entity,
                        ref DestroyableComponentData destroyableComponentData
                    ) =>
                {
                    if (destroyableComponentData.MustBeDestroyed)
                    {
                        _entityManager.DestroyEntity(entity);
                        Bootstrap.Instance.LookForPlayerSpawnPosition();
                    }
                }).Run();
        }
    }   
}
