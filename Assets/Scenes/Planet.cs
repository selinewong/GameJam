using UnityEngine;

//BEFORE
/****************************************************************************************************
public class Planet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
*/

//NEW
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField]
    private float gravitationalPull;

    public float GravitationalPull {get => gravitationalPull;}
}