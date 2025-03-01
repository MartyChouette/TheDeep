using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static UserInput instance;

    public Vector2 MoveInput { get; private set; }

    public bool JumpJustPressed { get; private set; }


    public bool AttackInput { get; private set; }

    public bool DashInput { get; private set; }
    public bool MenuOpenCloseInput { get; private set; }


    private PlayerInput _playerInput;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _attackAction;
    private InputAction _dashAction;
    private InputAction _menuOpenCloseAction;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();

        SetupInputActions();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
    }

    private void SetupInputActions()
    {
        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
        _attackAction = _playerInput.actions["Attack"];
        _dashAction = _playerInput.actions["Dash"];
        _menuOpenCloseAction = _playerInput.actions["MenuOpenClose"];

    }

    private void UpdateInput()
    {
        MoveInput = _moveAction.ReadValue<Vector2>();
        JumpJustPressed = _jumpAction.WasPressedThisFrame();
        AttackInput = _attackAction.WasPressedThisFrame();
        DashInput = _dashAction.WasPressedThisFrame();
        MenuOpenCloseInput = _menuOpenCloseAction.WasPressedThisFrame();
    }
}
