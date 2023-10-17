using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float movementSpeed = 3.0f, distance = 3.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, distance))
        {
            transform.Rotate(Vector3.up, 90);
        }
        
        
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }
}
