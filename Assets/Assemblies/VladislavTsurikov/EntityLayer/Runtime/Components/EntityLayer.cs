using Unity.Entities;
using UnityEngine;
using VladislavTsurikov.DOTSEntityLayer.Runtime;

namespace VladislavTsurikov.EntityLayer.Runtime.Components
{
    /// With an Entity that has a Sprite Renderer Component, you cannot get a LayerMask. Therefore this component is a necessity <inheritdoc />
    public struct EntityLayer : IComponentData
    {
        public int LayerMask;
        
        public EntityLayer(LayerMask value)
        {
            LayerMask = value;
        }
        
        public EntityLayer(int indexLayer)
        {
            LayerMask = LayerMaskUtility.ConvertIndexLayerToLayerMask(indexLayer);
        }
    }
}