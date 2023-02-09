using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
    }
    private void OnDestroy()
    {
        playerInput.Player.Disable();
    }
}
