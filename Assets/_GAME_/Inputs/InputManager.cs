using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static Vector2 Movement;
    public static bool OpenMenuPressed { get; private set; }
    public static bool AttackPressed { get; private set; }
    public static bool InteractPressed { get; private set; }

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _openMenu;
    private InputAction _attack;
    private InputAction _interact;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _openMenu = _playerInput.actions["OpenUI"];
        _attack = _playerInput.actions["Attack"];
        _interact = _playerInput.actions["Interact"];
    }

    private void Update()
    {
        Movement = _moveAction.ReadValue<Vector2>();
        OpenMenuPressed = _openMenu.triggered;
        AttackPressed = _attack.triggered;
        InteractPressed = _interact.triggered;
    }
}
