namespace Data
{
    // Типы рандома для цвета
    public enum ColorRandomType//3
    {
        N = 0,// NONE - без рандома (4 числа)
        IMM = 1,// INSTANTMINMAX - рандом (8 чисел)
        MM = 2 // MINMAX - рандом с интервалом (8 чисел + 1 число (internal))
    }
}