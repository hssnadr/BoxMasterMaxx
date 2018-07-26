using UnityEngine;

namespace Bezier
{
    public static class Bezier
    {
        /// <summary>
        /// Quadraticly interpolates the position of a point at the value t. B(t) = (1 - t)2 P0 + 2 (1 - t) t P1 + t2 P2.
        /// </summary>
        /// <param name="p0">First point</param>
        /// <param name="p1">Second point</param>
        /// <param name="p2">Third point</param>
        /// <param name="p3">Fourth point</param>
        /// <param name="t">the value</param>
        /// <returns></returns>
        public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            // B(t) = (1 - t)3 P0 + 3(1 - t)2 t P1 +3(1 - t) t2 P2 +t3 P3
            t = Mathf.Clamp01(t);
            float oneMinusT = 1.0f - t;
            return oneMinusT * oneMinusT * oneMinusT * p0 +
                3.0f * oneMinusT * oneMinusT * t * p1 +
                3.0f * oneMinusT * t * t * p2 +
                t * t * t * p3;
        }

        /// <summary>
        /// Get the derivative. 
        /// </summary>
        /// <param name="p0">First point</param>
        /// <param name="p1">Second point</param>
        /// <param name="p2">Third point</param>
        /// <param name="p3">Fourth point</param>
        /// <param name="t">value</param>
        /// <returns></returns>
        public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            // B'(t) = 3 (1 - t)2 (P1 - P0) + 6 (1 - t) t (P2 - P1) + 3 t2 (P3 - P2)
            t = Mathf.Clamp01(t);
            float oneMinusT = 1.0f - t;
            return 3.0f * oneMinusT * oneMinusT * (p1 - p0) +
                6.0f * oneMinusT * t * (p2 - p1) +
                3.0f * t * t * (p3 - p2);
        }
    }
}