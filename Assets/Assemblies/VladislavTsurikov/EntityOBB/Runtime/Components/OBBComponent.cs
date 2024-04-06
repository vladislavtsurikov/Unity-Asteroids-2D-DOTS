using Unity.Entities;

namespace VladislavTsurikov.EntityOBB.Runtime.Components
{
    public struct OBBComponent : IComponentData
    {
        public Math.Runtime.OBB Obb;
    }
}