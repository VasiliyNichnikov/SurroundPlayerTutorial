#nullable enable
using UnityEngine;

namespace RadiusModule
{
    public struct RadiusSettings
    {
        public float LengthFromCenter { get; }
        public float AngleIncrease { get; }

        public static RadiusSettings Create(float lengthFromCenter, float angleIncrease)
        {
#if UNITY_EDITOR || DEBUG
            if (lengthFromCenter <= 0)
            {
                Debug.LogError("RadiusSettings.Create: the length from the center cannot be less than zero.");
                lengthFromCenter = 1;
            }

            if (angleIncrease <= 0)
            {
                Debug.LogError("RadiusSettings.Create: the angle of inclination must be greater than zero.");
                angleIncrease = 1;
            }

            if (angleIncrease > 360)
            {
                Debug.LogError("RadiusSettings.Create: the angle of inclination should be less than 360.");
                angleIncrease = 1;
            }
#endif
            return new RadiusSettings(lengthFromCenter, angleIncrease);
        }

        private RadiusSettings(float lengthFromCenter, float angleIncrease)
        {
            LengthFromCenter = lengthFromCenter;
            AngleIncrease = angleIncrease;
        }
    }
}