namespace VladislavTsurikov.DOTSEntityLayer.Runtime
{
    public static class LayerMaskUtility
    {
        public static int ConvertIndexLayerToLayerMask(int layerIndex)
        {
            return 1 << layerIndex;
        }
    }
}