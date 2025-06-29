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
/* Worked a bit   
    public PlayerInputActions _inputActions;
    public MoveData moveData;
    CharacterController cc;
    Vector3 movementVector;
    
    private PlayerInputActions _input;

    public Transform planet;         // Assign this in the Inspector to the planet GameObject
    public float gravitySpeed = 9.81f; // Gravity strength
    private Vector3 gravity;         // Stores the gravity direction and force
    //public float moveSpeed = 50f;

    private System.Action<InputAction.CallbackContext> onMovePerformed;
    private System.Action<InputAction.CallbackContext> onMoveCanceled;

    private Rigidbody rb;
    private MeshCollider planetCollider;
    private void OnEnable()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        planetCollider = planet.GetComponent<MeshCollider>();

        if (!rb)
    {
        Debug.LogError("Missing Rigidbody on Player!");
        enabled = false;
    }
        if (!planetCollider)
        {
            Debug.LogError("Planet must have a MeshCollider.");
            enabled = false; // disable script to prevent further errors
        }
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
        RotateToSurface();        // Find nearest point on the planet surface
         // Cast a ray from player toward the planet's center
        Vector3 directionToPlanet = (planet.position - transform.position).normalized;

        Ray ray = new Ray(transform.position, directionToPlanet);
        RaycastHit hit;

        // Check if ray hits the planet
        if (planetCollider.Raycast(ray, out hit, 1000f))
        {
            Vector3 gravityDirection = (hit.point - transform.position).normalized;

        // Apply gravity force
        rb.AddForce(Vector3.Scale(gravityDirection, gravity));

            // Optional: align object 'up' to gravity direction
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
            Vector3 nearestPoint = planetCollider.ClosestPoint(transform.position);

        // Compute direction from object to the surface point
        //Vector3 gravityDirection = (nearestPoint - transform.position).normalized;

        

        // Optional: rotate object to align 'up' with gravity direction
        //Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        
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
*/
    public PlayerInputActions _inputActions;
    public MoveData moveData;

    private CharacterController cc;
    private Rigidbody rb;
    private MeshCollider planetCollider;

    private Vector3 movementVector;
    private Vector3 gravity;

    public Transform planet; // Assign this in the Inspector
    public float gravitySpeed = 9.81f;

    void Awake()
    {
        cc = GetComponent<CharacterController>();

        if (planet == null)
        {
            GameObject foundPlanet = GameObject.Find("Planet");
            if (foundPlanet != null)
                planet = foundPlanet.transform;
            else
                Debug.LogWarning("Planet GameObject not found in scene!");
        }
    }

    void OnEnable()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb)
        {
            Debug.LogError("Missing Rigidbody on Player!");
            enabled = false;
            return;
        }

        if (!planet)
        {
            Debug.LogError("Planet reference is missing!");
            enabled = false;
            return;
        }

        planetCollider = planet.GetComponent<MeshCollider>();
        if (!planetCollider)
        {
            Debug.LogError("Planet must have a MeshCollider!");
            enabled = false;
        }
    }

    void Update()
    {
        Move(_inputActions.PlayerActionMap.Movement.ReadValue<Vector2>());
    }

    void FixedUpdate()
    {
        ApplyGravityWithRaycast();
        RotateToSurface();

        float speed = moveData.moveSpeed * 5f;
        Vector3 totalMovement = (movementVector * speed + gravity) * Time.deltaTime;
        cc.Move(totalMovement);
    }

    private void Move(Vector2 input)
    {
        Vector3 localMove = transform.forward * input.y + transform.right * input.x;
        movementVector = localMove.magnitude > 1f ? localMove.normalized : localMove;
        Debug.Log("Move Vector: " + movementVector);
    }

    private void Fall()
    {
        gravity = (planet.position - transform.position).normalized * gravitySpeed;
    }

    private void ApplyGravityWithRaycast()
    {
        Vector3 directionToPlanet = (planet.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, directionToPlanet);
        RaycastHit hit;

        if (planetCollider.Raycast(ray, out hit, 1000f))
        {
            Vector3 gravityDirection = (hit.point - transform.position).normalized;
            gravity = gravityDirection * gravitySpeed;

            rb.AddForce(gravity, ForceMode.Acceleration);

            // Optional: align object's up with surface
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    private void RotateToSurface()
    {
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravity) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f * Time.deltaTime);
    }
}
