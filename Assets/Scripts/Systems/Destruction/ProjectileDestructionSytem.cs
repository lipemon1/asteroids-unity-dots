using Asteroids.Components;
using Asteroids.Components.Tags;
using Unity.Entities;

namespace Asteroids.Systems
{
    public class ProjectileDestructionSytem : SystemBase
    {
        EntityManager _entityManager;

        protected override void OnCreate()
        {
            base.OnCreate();
            _entityManager = World.EntityManager;
        }

        protected override void OnUpdate()
        {
            Entities.WithoutBurst().WithStructuralChanges().WithAll<ProjectileTagComponentData>().ForEach(
                (
                    Entity entity,
                    in DestroyableComponentData destroyableComponentData
                ) =>
            {
                if(destroyableComponentData.MustBeDestroyed)
                    _entityManager.DestroyEntity(entity);
                
            }).Run();
        }
    }   
}
