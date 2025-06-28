using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyController : Movement
{
    Rigidbody rig;

    void OnEnable()
    {
        rig = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //rig.AddForce((movementVector + gravity) * gravityStrength + jumpVector) 
          //  * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}