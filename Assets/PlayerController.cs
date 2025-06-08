using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private PlayerInputActions _input;
    private CharacterController cc;
    private Vector2 movementVector;
    public float moveSpeed = 50f;

    private System.Action<InputAction.CallbackContext> onMovePerformed;
    private System.Action<InputAction.CallbackContext> onMoveCanceled;


    void Awake()
    {
        _input = new PlayerInputActions();
        cc = GetComponent<CharacterController>();

        onMovePerformed = ctx => movementVector = ctx.ReadValue<Vector2>();
    onMoveCanceled = ctx => movementVector = Vector2.zero;

    }

    
    void OnEnable()
{
    _input.PlayerActionMap.Enable();
    _input.PlayerActionMap.Movement.performed += onMovePerformed;
    _input.PlayerActionMap.Movement.canceled += onMoveCanceled;
}


    void OnDisable()
{
    _input.PlayerActionMap.Movement.performed -= onMovePerformed;
    _input.PlayerActionMap.Movement.canceled -= onMoveCanceled;
    _input.PlayerActionMap.Disable();
}


    void Update()
    {
        Vector3 move = (transform.forward * movementVector.y + transform.right * movementVector.x)
                       * moveSpeed * Time.deltaTime;
        cc.Move(move);
    }
}
