using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    private CharacterController cc;
    private Vector3 movementVector;
    public float moveSpeed = 5f;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        cc = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        _inputActions.PlayerActionMap.Enable();           // Use your actual Action Map name
    }

    private void OnDisable()
    {
        _inputActions.PlayerActionMap.Disable();
    }

    private void Update()
    {
        Vector2 input = _inputActions.PlayerActionMap.Movement.ReadValue<Vector2>();
        Move(input);
    }

    private void FixedUpdate()
    {
        cc.Move(movementVector * moveSpeed * Time.fixedDeltaTime);
    }

    void Move(Vector2 input)
    {
        movementVector = transform.forward * input.y + transform.right * input.x;
    }
}
