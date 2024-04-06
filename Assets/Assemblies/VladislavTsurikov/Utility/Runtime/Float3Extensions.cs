using Unity.Mathematics;

namespace VladislavTsurikov.Utility.Runtime
{
    public static class Float3Extensions
    {
        public static float3 Normalize(this float3 vector)
        {
            float length = math.length(vector);
            if (length > 0)
            {
                return vector / length;
            }
            else
            {
                return float3.zero;
            }
        }
    }
}