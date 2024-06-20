namespace VladislavTsurikov.EntityLayer.Runtime.Utility
{
    public static class LayerMaskUtility
    {
        public static int ConvertIndexLayerToLayerMask(int layerIndex)
        {
            return 1 << layerIndex;
        }
    }
}