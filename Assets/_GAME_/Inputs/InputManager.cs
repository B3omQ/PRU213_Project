using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static Vector2 Movement;
    public static bool OpenMenuPressed { get; private set; }

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _openMenu;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _openMenu = _playerInput.actions["OpenUI"];
    }

    private void Update()
    {
        Movement = _moveAction.ReadValue<Vector2>();
        OpenMenuPressed = _openMenu.triggered;
    }
}
