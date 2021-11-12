using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Asteroids.Components.Authoring
{
    public class ScreenInfoComponentData_Authoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public Camera Camera;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            Vector3 bottomLeftCornerPosition = Camera.ViewportToWorldPoint(new Vector3(0, 0));
            Vector3 topRightCornerPosition = Camera.ViewportToWorldPoint(new Vector3(1, 1));

            float screenHeight = topRightCornerPosition.y - bottomLeftCornerPosition.y;
            float screenWidth = topRightCornerPosition.x - bottomLeftCornerPosition.x;

            float2 size = new float2(screenWidth, screenHeight);

            dstManager.AddComponentData(entity, new ScreenInfoComponentData()
            {
                Height = size.y,
                Width = size.x
            });
        }
    }   
}