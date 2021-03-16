using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Калькулятор, работающий с выражением из строки
[Serializable]
public class Calculator
{
    List<int> sortIDs = new List<int>();// массив с сортировкой ид по приоритету выполнения
    List<ExpressionSign> signs = new List<ExpressionSign>();// Список знаков
    List<float> nums = new List<float>();// Список чисел (всегда его длина = количество знаков + 1)
    static Dictionary<Sign, int> signPriority = new Dictionary<Sign, int>()// Приоритет операций
        { { Sign.Plus, 1 }, { Sign.Minus, 1 }, { Sign.Multiply, 2 }, { Sign.Division, 2 }, { Sign.Power, 3 } };
    public Calculator(string expression)// Создание выражения и его индексация
    {
        if (expression.Length == 0)
        {
            nums.Add(0);
            return;
        }

        int force = 0;// сила операций
        int order = 1;// порядок операции
        string num = string.Empty;// переменная для индексация чисел
        foreach (char c in expression)
        {
            if (c == '(')
            {
                if (num.Length != 0)
                {
                    nums.Add(float.Parse(num));
                    num = string.Empty;
                }
                force++;
            }
            else if (c == ')')
            {
                if (num.Length != 0)
                {
                    nums.Add(float.Parse(num));
                    num = string.Empty;
                }
                force--;
            }
            else if (c == '+' || c == '-' || c == '*' || c == '/' || c == '^')
            {
                if (num.Length != 0)
                {
                    nums.Add(float.Parse(num));
                    num = string.Empty;
                }
                signs.Add(new ExpressionSign((Sign)c, force, order));
                order++;
            }
            else if (c == ',')
                num += '.';
            else if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' ||
                c == '5' || c == '6' || c == '7' || c == '8' || c == '9' || c == '.')
                num += c;
        }
        if (num.Length != 0)
        {
            nums.Add(float.Parse(num));
        }

        for (int x = 0; x < signs.Count; x++)// сам алгоритм сортировки
        {
            int biggestID = 0;
            int signPower = 0;
            int signForce = 0;
            for (int y = 0; y < signs.Count; y++)
            {
                if (sortIDs.Contains(y))
                    continue;
                if (signs[y].force > signForce)
                {
                    biggestID = y;
                    signPower = signPriority[signs[y].sign];
                    signForce = signs[y].force;
                }
                else if (signs[y].force == signForce)
                {
                    if (signPriority[signs[y].sign] > signPower)
                    {
                        biggestID = y;
                        signPower = signPriority[signs[y].sign];
                        signForce = signs[y].force;
                    }
                }
            }
            sortIDs.Add(biggestID);
        }
        /*
        string end = string.Empty;
        string sort = string.Empty;
        for (int i = 0; i < signs.Count; i++)
        {
            end += signs[i].sign.ToString() + ' ';
        }
        for (int i = 0; i < nums.Count; i++)
        {
            sort += nums[i].ToString() + ' ';
        }
        Debug.Log(end);
        Debug.Log(sort);
        */
    }

    public float GetResult()// Выдача результата
    {
        if (nums.Count == 1)
        {
            return nums[0];
        }

        List<int> sortIDs = this.sortIDs;
        List<ExpressionSort> expressions = new List<ExpressionSort>();// выражения
        for (int i = 0; i < signs.Count; i++)// 
            expressions.Add(new ExpressionSort(signs[i], nums[i], nums[i + 1]));//  
        for (int i = 0; i < signs.Count - 1; i++)// 
        {
            //Print();
            float result = SolveExpression(expressions[sortIDs[0]]);// решение выражения
            if (sortIDs[0] > 0)// если сзади есть ещё операции
                expressions[sortIDs[0] - 1].SetResult(result, 2);// замена 2 числа на результат
            if (sortIDs[0] < sortIDs.Count - 1)// если спереди есть ещё операции
                expressions[sortIDs[0] + 1].SetResult(result, 1);// замена 1 числа на результат
            expressions.RemoveAt(sortIDs[0]);// удаление элемента сортировки

            for (int x = 0; x < sortIDs.Count; x++)
            {
                if (sortIDs[x] > sortIDs[0])
                    sortIDs[x]--;
            }
            sortIDs.RemoveAt(0);
        }
        /*
        void Print()
        {
            string end = string.Empty;
            string sort = string.Empty;
            for (int i = 0; i < expressions.Count; i++)
            {
                end += expressions[i].num1.ToString() + expressions[i].sign.sign + expressions[i].num2.ToString() + ' ';
            }
            for (int i = 0; i < sortIDs.Count; i++)
            {
                sort += sortIDs[i].ToString() + ' ';
            }
            Debug.Log(end);
            Debug.Log(sort);
        }*/
        //Print();

        return SolveExpression(expressions[0]);// возвращает ответ
    }

    class ExpressionSort
    {
        public ExpressionSign sign;
        public float num1;
        public float num2;

        public ExpressionSort(ExpressionSign sign, float num1, float num2)
        {
            this.sign = sign;
            this.num1 = num1;
            this.num2 = num2;
        }
        public void SetResult(float result, int numID)
        {
            switch (numID)
            {
                case 1:
                    num1 = result;
                    break;
                case 2:
                    num2 = result;
                    break;
            }
        }
    }

    float SolveExpression(ExpressionSort sort)
    {
        switch (sort.sign.sign)
        {
            case Sign.Plus:
                return sort.num1 + sort.num2;
            case Sign.Minus:
                return sort.num1 - sort.num2;
            case Sign.Multiply:
                return sort.num1 * sort.num2;
            case Sign.Division:
                return sort.num1 / sort.num2;
            case Sign.Power:
                return Mathf.Pow(sort.num1, sort.num2);
        }
        return 0;
    }

    [Serializable]
    class ExpressionSign
    {
        public Sign sign;
        public int force;
        public int order;
        public ExpressionSign(Sign sign, int force, int order)
        {
            this.sign = sign;
            this.force = force;
            this.order = order;
        }
    }
    enum Sign
    {
        Plus = '+', Minus = '-',
        Multiply = '*', Division = '/',
        Power = '^'
    }
}