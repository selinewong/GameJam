using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    /// before
    /*
            private PlayerInputActions _input;
            private CharacterController cc;
            private Vector2 movementVector;
            public float moveSpeed = 50f;

            public Transform planet;         // Assign this in the Inspector to the planet GameObject
            public float gravitySpeed = 10f; // Gravity strength
            private Vector3 gravity;         // Stores the gravity direction and force

            private System.Action<InputAction.CallbackContext> onMovePerformed;
            private System.Action<InputAction.CallbackContext> onMoveCanceled;

            private void Fall()
            {
                if (planet == null)
                {
                    Debug.LogWarning("Planet reference is missing!");
                    return;
                }

                gravity = (planet.position - transform.position).normalized * gravitySpeed;
            }

            void RotateToSurface()
            {
                Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravity) * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f * Time.deltaTime);
            }

            void Awake()
            {
                _input = new PlayerInputActions();
                cc = GetComponent<CharacterController>();

                if (planet == null)
                {
                    GameObject foundPlanet = GameObject.Find("Planet");
                    if (foundPlanet != null)
                    {
                        planet = foundPlanet.transform;
                    }
                    else
                    {
                        Debug.LogWarning("Planet GameObject not found in scene!");
                    }
                }

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
                Fall();
                RotateToSurface();

                Vector3 gravityDirection = (transform.position - planet.position).normalized;
                Vector3 tangentForward = Vector3.Cross(transform.right, gravityDirection).normalized;
                Vector3 tangentRight = Vector3.Cross(gravityDirection, tangentForward).normalized;

                Vector3 move = Vector3.zero;

                if (movementVector.sqrMagnitude > 0.01f)
                {
                    move = (tangentForward * movementVector.y + tangentRight * movementVector.x)
                        * moveSpeed * Time.deltaTime;
                    cc.Move(move);
                }

                Vector3 desiredForward = move.normalized;
                if (desiredForward.sqrMagnitude > 0.01f)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(desiredForward, gravityDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);
                }

                cc.Move(gravity * Time.deltaTime);

                Debug.Log("Movement Vector: " + movementVector);
                Debug.DrawRay(transform.position, gravity, Color.red);
                Debug.Log("Gravity vector: " + gravity);
            }
        */

    /// after
    
    public PlayerInputActions _inputActions;
    public MoveData moveData;
    CharacterController cc;
    Vector3 movementVector;
    
    private PlayerInputActions _input;

    public Transform planet;         // Assign this in the Inspector to the planet GameObject
    public float gravitySpeed = 10f; // Gravity strength
    private Vector3 gravity;         // Stores the gravity direction and force
    //public float moveSpeed = 50f;

    private System.Action<InputAction.CallbackContext> onMovePerformed;
    private System.Action<InputAction.CallbackContext> onMoveCanceled;

    private void OnEnable()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
    }

    void Awake()
{
    cc = GetComponent<CharacterController>();

    if (planet == null)
    {
        GameObject foundPlanet = GameObject.Find("Planet");
        if (foundPlanet != null)
        {
            planet = foundPlanet.transform;
        }
        else
        {
            Debug.LogWarning("Planet GameObject not found in scene!");
        }
    }
}
    

    private void Update()
    {
        Move(_inputActions.PlayerActionMap.Movement.ReadValue<Vector2>());
    }

    private void FixedUpdate()
    {
        Fall();
        RotateToSurface();

        float speed = moveData.moveSpeed * 5f;
        Vector3 totalMovement = (movementVector * speed + gravity) * Time.deltaTime;

        cc.Move(totalMovement);
        //cc.Move(movementVector * moveData.moveSpeed * 5f * Time.deltaTime);
    }
    
    private void Fall()
    {
        gravity = (planet.position - transform.position).normalized * gravitySpeed;
    }
     void RotateToSurface()
            {
                Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravity) * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f * Time.deltaTime);
            }
    void Move(Vector2 _input)
    {
        Vector3 localMove = transform.forward * _input.y + transform.right * _input.x;
        movementVector = localMove.magnitude > 1f ? localMove.normalized : localMove;
        Debug.Log("Move Vector: " + movementVector);
    }
}
