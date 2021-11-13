using Asteroids.Components;
using Asteroids.Components.Tags;
using Asteroids.Statics;
using Unity.Entities;

namespace Asteroids.Systems
{
    public class AsteroidDestructionSystem : SystemBase
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
                .WithAll<AsteroidTagComponent>()
                .ForEach(
                    (
                        Entity entity,
                        in DestroyableComponentData destroyableComponentData
                    ) =>
            {
                if (destroyableComponentData.MustBeDestroyed)
                {
                    _entityManager.DestroyEntity(entity);
                    ScoreHandler.AddScore(destroyableComponentData.PointsForDestroying);
                }
            }).Run();
        }
    }   
}
