using UnityEngine;

namespace Core.Utils
{
    public static class MyUtils
    {
        public static Vector3 GetVectorFromAngle(float angle)
        {
            var angleRad = angle * (Mathf.PI / 180f);
            return new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad));
        }
    }
}