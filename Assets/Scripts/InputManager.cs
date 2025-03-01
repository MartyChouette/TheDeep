using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{


    public static InputManager instance;


    public bool MenuOpenCloseInput { get; private set; }

    private PlayerInput _playerInput;

    private InputAction _menuOpenCloseAction;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();
        _menuOpenCloseAction = _playerInput.actions["MenuOpenClose"];
    }

    // Update is called once per frame
    private void Update()
    {
        MenuOpenCloseInput = _menuOpenCloseAction.WasPressedThisFrame();
    }
}
