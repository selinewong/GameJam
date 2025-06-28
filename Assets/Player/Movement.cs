using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    [SerializeField]
    protected MoveData moveData;

    [SerializeField]
    protected PlayerInputActions _inputActions;

    public Transform planet;
    protected Vector3 movementVector;
    protected Vector3 gravity;

    [SerializeField]
    LayerMask GroundLayerMask;
    [SerializeField]
    Transform groundCollider;
    bool isGrounded;
    public float gravitySpeed = 1f;
    public float surfaceRotationSpeed = 2f;

    private void OnEnable()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
    }

    void Update()
    {
        IsGrounded();
        RotateToSurface();
        Fall();
        Move(_inputActions.PlayerActionMap.Movement.ReadValue<Vector2>());
    }

    void RotateToSurface()
    {
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravity) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, surfaceRotationSpeed * Time.deltaTime);
    }

    private void Fall()
    {
        gravity = (planet.position - transform.position).normalized * gravitySpeed;
    }

    void Move(Vector2 _input)
    {
        movementVector = transform.forward * _input.y + transform.right * _input.x;
        movementVector *= moveData.moveSpeed;
    }

    void IsGrounded()
    {
        if (Physics.CheckSphere(groundCollider.position, 1f, GroundLayerMask))
        {
            isGrounded = true;
            return;
        }

        isGrounded = false;
    }
}
