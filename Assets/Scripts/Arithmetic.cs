using UnityEngine;
using Enums;

public class Arithmetic : MonoBehaviour
{
    int number;
    public int ArithmeticAction(string arithmetic, int number_1, int number_2)
    {
        switch (arithmetic)
        {
            case nameof(ARITHMETIC_ACTIONS.Mult):
                number = number_1 * number_2;
                break;
            case nameof(ARITHMETIC_ACTIONS.Div):
                number = number_1 / number_2;
                break;
            case nameof(ARITHMETIC_ACTIONS.Add):
                number = number_1 + number_2;
                break;
            case nameof(ARITHMETIC_ACTIONS.Sub):
                number = number_1 - number_2;
                break;
        }

        return number;
    }
}
