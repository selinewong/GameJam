using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private PlayerInputActions _input;
    private CharacterController cc;
    private Vector2 movementVector;
    public float moveSpeed = 5f;

    void Awake()
    {
        _input = new PlayerInputActions();
        cc = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        _input.PlayerActionMap.Enable();
        _input.PlayerActionMap.Movement.performed += ctx => movementVector = ctx.ReadValue<Vector2>();
        _input.PlayerActionMap.Movement.canceled  += ctx => movementVector = Vector2.zero;
    }

    void OnDisable()
    {
        _input.PlayerActionMap.Movement.performed -= ctx => movementVector = ctx.ReadValue<Vector2>();
        _input.PlayerActionMap.Movement.canceled  -= ctx => movementVector = Vector2.zero;
        _input.PlayerActionMap.Disable();
    }

    void Update()
    {
        Vector3 move = (transform.forward * movementVector.y + transform.right * movementVector.x)
                       * moveSpeed * Time.deltaTime;
        cc.Move(move);
    }
}
