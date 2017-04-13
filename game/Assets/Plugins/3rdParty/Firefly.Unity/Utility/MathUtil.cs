using System;
using System.Collections.Generic;
using UnityEngine;

namespace Firefly.Unity.Utility
{
    public static class MathUtil
    {
        public static float PI = 3.14159274f;
        public static float TwoPI = 6.28318548f;

        public static readonly float DEG_TO_RAD = PI / 180f;
        public static readonly float RAD_TO_DEG = 180f / PI;

        private static System.Random random = new System.Random();

        public static float RadToDeg(float r)
        {
            return RAD_TO_DEG * r;
        }

        public static float DegToRad(float d)
        {
            return DEG_TO_RAD * d;
        }
        
        public static byte FloatToByte(float f)
        {
            byte one = 1 << 2;
            f = f * (float)one;
            return (byte)Math.Round(f);
        }

        public static float ByteToFloat(byte b)
        {
            sbyte one = 1 << 2;
            return (float)b / (float)one;
        }

        public static bool RandomBool(float f)
        {
            return random.NextDouble() < (double)f;
        }

        public static bool RandomBool()
        {
            return RandomBool(0.5f);
        }

        public static float Random(float f)
        {
            return (float)random.NextDouble() * f;
        }

        public static float Random(float min, float max)
        {
            return min + (float)random.NextDouble() * (max - min);
        }

        public static int Random(int min, int max)
        {
            return (int)((float)min + (float)random.NextDouble() * (float)(max - min));
        }

        public static int Random(int i)
        {
            return (int)(random.NextDouble() * (double)i);
        }

        public static bool Equals(float l, float r)
        {
            return Mathf.Abs(l - r) < 0.001f;
        }

        public static bool Equals(Plane l, Plane r)
        {
            return Equals(l.normal, r.normal) && Equals(l.distance, r.distance);
        }

        public static bool Equals(Vector3 l, Vector3 r)
        {
            return Equals(l.x, r.x) && Equals(l.y, r.y) && Equals(l.z, r.z);
        }

        public static bool Equals(Vector3 l, Vector3 r, float dis)
        {
            Vector3 delta = l - r;
            if (delta.sqrMagnitude < dis * dis)
            {
                return true;
            }
            return false;
        }

        public static bool Equals2D(Vector3 l, Vector3 r, float dis)
        {
            Vector3 delta = l - r;
            delta.y = 0;
            if (delta.sqrMagnitude < dis * dis)
            {
                return true;
            }
            return false;
        }

        public static float Distance(Vector3 l, Vector3 r)
        {
            return (l - r).magnitude;
        }

        public static float DistanceSqr(Vector3 l, Vector3 r)
        {
            return (l - r).sqrMagnitude;
        }

        public static float Distance2D(Vector3 l, Vector3 r)
        {
            Vector3 delta = l - r;
            delta.y = 0;
            return delta.magnitude;
        }

        public static float DistanceSqr2D(Vector3 l, Vector3 r)
        {
            Vector3 delta = l - r;
            delta.y = 0;
            return delta.sqrMagnitude;
        }

        public static void ParabolaToSegment(Vector3 s, Vector3 e, float angle, int segCount, ref List<Vector3> segments)
        {
            float dist = MathUtil.Distance(e, s);
            float disttan = dist * Mathf.Tan(Mathf.Deg2Rad * angle);

            float t = Mathf.Sqrt(2 * disttan / -Physics.gravity.y);
            float v0 = disttan / t;

            float step = t / segCount;

            segments.Clear();
            segments.Add(s);

            for (int i = 0; i < segCount; i++)
            {
                float tt = step * (i + 1);
                Vector3 vv = Vector3.Lerp(s, e, (float)(i + 1) / (float)segCount);
                float h = v0 * tt + 0.5f * Physics.gravity.y * tt * tt;
                vv.y += h;
                segments.Add(vv);
            }
        }

        public static float Constrain2Pi(float r)
        {
            if (r < 0f)
            {
                int v = (int)(-r / MathUtil.TwoPI) + 1;
                r += (float)v * MathUtil.TwoPI;
            }
            if (r >= MathUtil.TwoPI)
            {
                int v = (int)(r / MathUtil.TwoPI);
                r -= (float)v * MathUtil.TwoPI;
            }
            return r;
        }

        public static float ConstrainPI(float r)
        {
            if (r < -MathUtil.PI)
            {
                r += MathUtil.PI * 2f;
            }
            if (r >= MathUtil.PI)
            {
                r -= MathUtil.PI * 2f;
            }
            return r;
        }

        public static float AngleFromTo(float v0x, float v0y, float v1x, float v1y)
        {
            return RAD_TO_DEG * Constrain2Pi((float)(
                Math.Atan2((double)v1y, (double)v1x) -
                Math.Atan2((double)v0y, (double)v0x)));
        }

        public static float AngleFromTo(Vector2 v0, Vector2 v1)
        {
            return MathUtil.AngleFromTo(v0.x, v0.y, v1.x, v1.y);
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                int k = (UnityEngine.Random.Range(0, n) % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}