namespace Data
{
    // Место, от какой части экрана расчитывается позиция
    // необходимо для local и global позиции, а также так как
    // экраны телефонов бывают разными, а поэтому необходим инструмент
    // для большей кастомизации позиции объектов
    public enum AnchorPresets
    {
        Left_Top = 0, Center_Top = 1, Right_Top = 2,
        Left_Middle = 3, Center_Middle = 4, Right_Middle = 5,
        Left_Bottom = 6, Center_Bottom = 7, Right_Bottom = 8
    }
}