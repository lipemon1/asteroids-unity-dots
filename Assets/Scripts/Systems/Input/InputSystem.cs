using Asteroids.Components;
using Unity.Entities;
using UnityEngine;

namespace Asteroids.Systems
{
    public class InputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref InputComponentData input) =>
            {
                input.InputLeft = Input.GetKey(KeyCode.A);
                input.InputRight = Input.GetKey(KeyCode.D);
                input.InputForward = Input.GetKey(KeyCode.W);
                input.InputShoot = Input.GetKey(KeyCode.Space);
                
            }).Run();
        }
    }   
}
