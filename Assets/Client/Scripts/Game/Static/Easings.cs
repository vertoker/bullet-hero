using UnityEngine;
using Data;

namespace Game.Static
{
    public static class Easings
    {
        private const float c1 = 1.70158f;
        private const float c2 = c1 * 1.525f;
        private const float c3 = c1 + 1f;
        private const float c4 = 2 * Mathf.PI / 3;
        private const float c5 = 2 * Mathf.PI / 4.5f;

        public static float GetEasing(float x, EasingType easing)
        {
            switch (easing)
            {
                case EasingType.Linear:
                    return x;
                case EasingType.Constant:
                    return Mathf.Floor(x);

                case EasingType.InSine:
                    return 1 - Mathf.Cos((x * Mathf.PI) / 2);
                case EasingType.OutSine:
                    return Mathf.Sin((x * Mathf.PI) / 2);
                case EasingType.InOutSine:
                    return -(Mathf.Cos(Mathf.PI * x) - 1) / 2;

                case EasingType.InQuad:
                    return x * x;
                case EasingType.OutQuad:
                    return 1 - Mathf.Pow(1 - x, 2);
                case EasingType.InOutQuad:
                    return x < 0.5f ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;

                case EasingType.InCubic:
                    return x * x * x;
                case EasingType.OutCubic:
                    return 1 - Mathf.Pow(1 - x, 3);
                case EasingType.InOutCubic:
                    return x < 0.5f ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;

                case EasingType.InQuart:
                    return x * x * x * x;
                case EasingType.OutQuart:
                    return 1 - Mathf.Pow(1 - x, 4);
                case EasingType.InOutQuart:
                    return x < 0.5f ? 8 * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 4) / 2;

                case EasingType.InQuint:
                    return x * x * x * x * x;
                case EasingType.OutQuint:
                    return 1 - Mathf.Pow(1 - x, 5);
                case EasingType.InOutQuint:
                    return x < 0.5f ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;

                case EasingType.InExpo:
                    return x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10);
                case EasingType.OutExpo:
                    return x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
                case EasingType.InOutExpo:
                    return x == 0 ? 0 : x == 1 ? 1 : x < 0.5f
                        ? Mathf.Pow(2, 20 * x - 10) / 2 : (2 - Mathf.Pow(2, -20 * x + 10)) / 2;

                case EasingType.InCirc:
                    return 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2));
                case EasingType.OutCirc:
                    return Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
                case EasingType.InOutCirc:
                    return x < 0.5f ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2
                        : (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2;

                case EasingType.InBack:
                    return c3 * x * x * x - c1 * x * x;
                case EasingType.OutBack:
                    return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
                case EasingType.InOutBack:
                    return x < 0.5f ? (Mathf.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
                        : (Mathf.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;

                case EasingType.InElastic:
                    return x == 0 ? 0 : x == 1 ? 1 : -Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((x * 10 - 10.75f) * c4);
                case EasingType.OutElastic:
                    return x == 0 ? 0 : x == 1 ? 1 : Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * c4) + 1;
                case EasingType.InOutElastic:
                    return x == 0 ? 0 : x == 1 ? 1 : x < 0.5f
                        ? -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2
                        : (Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2 + 1;
            }
            return 0;
        }
    }
}