using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MoveButtons : MonoBehaviour
{
    public static UI_MoveButtons instance { private set; get; }
    public float movementInput { private set; get; }

    private void Awake()
    {
        instance = this;
    }

    public void PressedButtonRight()
    {
        movementInput = 1;
    }

    public void PressedButtonLeft()
    {
        movementInput = -1;
    }

    public void EndPressButton()
    {
        movementInput = 0;
    }
}
