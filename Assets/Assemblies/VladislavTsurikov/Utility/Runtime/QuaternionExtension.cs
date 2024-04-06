using Unity.Mathematics;

namespace VladislavTsurikov.Utility.Runtime
{
    public static class QuaternionExtension
    {
        //At the time of writing the code, there was no way to get EulerAngles in the Mathematics package, this appeared only in Unity 2023, and the project is in Unity 2022
        public static float3 GetEulerAngles(this quaternion q2)
        {
            float4 q1 = q2.value;
 
            float sqw = q1.w * q1.w;
            float sqx = q1.x * q1.x;
            float sqy = q1.y * q1.y;
            float sqz = q1.z * q1.z;
            float unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            float test = q1.x * q1.w - q1.y * q1.z;
            float3 v;
 
            if (test > 0.4995f * unit)
            { // singularity at north pole
                v.y = 2f * math.atan2(q1.y, q1.x);
                v.x = math.PI / 2;
                v.z = 0;
                return NormalizeAngles(math.degrees(v));
            }
            if (test < -0.4995f * unit)
            { // singularity at south pole
                v.y = -2f * math.atan2(q1.y, q1.x);
                v.x = -math.PI / 2;
                v.z = 0;
                return NormalizeAngles(math.degrees(v));
            }
 
            quaternion q3 = new quaternion(q1.w, q1.z, q1.x, q1.y);
            float4 q = q3.value;
 
            v.y = math.atan2(2f * q.x * q.w + 2f * q.y * q.z, 1 - 2f * (q.z * q.z + q.w * q.w));   // Yaw
            v.x = math.asin(2f * (q.x * q.z - q.w * q.y));                                         // Pitch
            v.z = math.atan2(2f * q.x * q.y + 2f * q.z * q.w, 1 - 2f * (q.y * q.y + q.z * q.z));   // Roll
 
            return NormalizeAngles(math.degrees(v));
        }
 
        static float3 NormalizeAngles(float3 angles)
        {
            angles.x = NormalizeAngle(angles.x);
            angles.y = NormalizeAngle(angles.y);
            angles.z = NormalizeAngle(angles.z);
            return angles;
        }
 
        static float NormalizeAngle(float angle)
        {
            while (angle > 360)
                angle -= 360;
            while (angle < 0)
                angle += 360;
            return angle;
        }
    }
}