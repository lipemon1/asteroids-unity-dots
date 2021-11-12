using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Asteroids.Components
{
    [GenerateAuthoringComponent]
    public struct WeaponComponentData : IComponentData
    {
        public Entity ProjectilePrefab;
        public bool IsFiring;
        public float FireRate;
        public float Timer;
    }   
}