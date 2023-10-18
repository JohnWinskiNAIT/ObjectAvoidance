using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float movementSpeed = 3.0f, forwardDist = 1.0f, sideDist = 3.0f;
    
    bool isLeft, isRight;

    RaycastHit hit;

    [SerializeField] GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, 1, 1));

            if (Vector3.Distance(target.transform.position, transform.position) < 2.0f)
            {
                target.SetActive(false);
                target = null;
            }
        }

        AvoidWalls();
        
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponentInParent<Rigidbody>();

        if (rb != null)
        {
            if (rb.tag == "Collectable")
            {
                target = other.gameObject;
            }
        }
    }

    void AvoidWalls()
    {
        if (Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), transform.forward, out hit, Quaternion.identity, forwardDist))
        {
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, -hit.normal, 1, 1));

            // Rotate based on what is to the sides
            isLeft = Physics.Raycast(transform.position, -transform.right, sideDist);
            isRight = Physics.Raycast(transform.position, transform.right, sideDist);

            if (isLeft && isRight)
            {
                transform.Rotate(Vector3.up, 180);
            }
            else if (isLeft && !isRight)
            {
                transform.Rotate(Vector3.up, 90);
            }
            else if (!isLeft && isRight)
            {
                transform.Rotate(Vector3.up, -90);
            }
            else
            {
                if (Random.Range(1, 3) == 1)
                {
                    transform.Rotate(Vector3.up, 90);
                }
                else
                {
                    transform.Rotate(Vector3.up, -90);
                }
            }
        }
    }
}
