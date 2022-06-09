namespace Data
{
    // Типы функций плавности
    public enum EasingType
    {
        Linear = 0, Constant = 1,// без изменений
        InSine = 2, OutSine = 3, InOutSine = 4,// синусовые функции
        InQuad = 5, OutQuad = 6, InOutQuad = 7,// 2-степенные функции (квадрат)
        InCubic = 8, OutCubic = 9, InOutCubic = 10,// 3-степенные функции (куб)
        InQuart = 11, OutQuart = 12, InOutQuart = 13,// 4-степенные функции (тессеракт)
        InQuint = 14, OutQuint = 15, InOutQuint = 16,// 5-степенные функции
        InExpo = 17, OutExpo = 18, InOutExpo = 19,// экспанинциальные функции
        InCirc = 20, OutCirc = 21, InOutCirc = 22,// круговые функции
        InBack = 23, OutBack = 24, InOutBack = 25,// инерциальные функции
        InElastic = 26, OutElastic = 27, InOutElastic = 28// эластичные функции
    }
}