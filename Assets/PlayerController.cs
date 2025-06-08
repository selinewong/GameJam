using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private PlayerInputActions _input;
    private CharacterController cc;
    private Vector2 movementVector;
    public float moveSpeed = 5f;
    
public Transform planet;         // Assign this in the Inspector to the planet GameObject
public float gravitySpeed = 9.81f; // Gravity strength
private Vector3 gravity;         // Stores the gravity direction and force


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

        Vector3 move = (transform.forward * movementVector.y + transform.right * movementVector.x)
                       *10 * moveSpeed * Time.deltaTime;
        cc.Move(move);
    }
}
