using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#if UNITY_EDITOR_WIN
using System;
using UnityEditor;
#endif

namespace Asteroids.Components.Authoring
{
    public class OffScreenWrapperComponentData_Authoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public SpriteRenderer SpriteRenderer;
        public float2 RendererSize;
    
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new OffScreenWrapperComponentData()
            {
                RendererSize = RendererSize
            });
        }

#if UNITY_EDITOR_WIN
        [ContextMenu("Calculate Renderer Size")]
        public void SetRendererSize()
        {
            if (SpriteRenderer == null)
                SpriteRenderer = GetComponent<SpriteRenderer>();
            
            float width = SpriteRenderer.sprite.texture.width;
            float widthFloated = (float)Math.Round(width) / 100f;
            
            float height = SpriteRenderer.sprite.texture.height;
            float heightFloated = (float)Math.Round(height) / 100f;

            RendererSize.x = widthFloated;
            RendererSize.y = heightFloated;
            
            AssetDatabase.Refresh();
        }  
#endif
    }
}