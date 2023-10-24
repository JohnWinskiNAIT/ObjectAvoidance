using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float movementSpeed = 3.0f, forwardDist = 1.0f, sideDist = 3.0f;

    bool isLeft, isRight;

    RaycastHit hit;

    [SerializeField] List<GameObject> collectables;
    [SerializeField] GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckTargets();

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
                collectables.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponentInParent<Rigidbody>();

        if (rb != null)
        {
            if (rb.tag == "Collectable")
            {
                for(int i = 0; i < collectables.Count; i++)
                {
                    if (other.gameObject == collectables[i])
                    {
                        collectables.RemoveAt(i);
                    }
                }
            }
        }
    }

    void AvoidWalls()
    {
        if (Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), transform.forward, out hit, Quaternion.identity, forwardDist))
        {
            if (hit.transform.gameObject.tag == "Wall")
            {

                // Rotate based on what is to the sides
                isLeft = Physics.Raycast(transform.position, -transform.right, sideDist);
                isRight = Physics.Raycast(transform.position, transform.right, sideDist);

                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, -hit.normal, 1, 1));

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
    void CheckTargets()
    {
        float distance = 10000.0f;
        
        for (int i = 0; i < collectables.Count; i++)
        {
            if (Vector3.Distance(transform.position, collectables[i].transform.position) < distance)
            {
                distance = Vector3.Distance(transform.position, collectables[i].transform.position);
                target = collectables[i];
            }
        }
        
        if (target != null)
        {
            if (target.gameObject.activeSelf)
            {
                if (Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), target.transform.position - transform.position, out hit, Quaternion.identity))
                {
                    if (hit.transform.tag != "Wall")
                    {
                        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, 1, 1));
                    }

                    if (Vector3.Distance(target.transform.position, transform.position) < 2.0f)
                    {
                        target.SetActive(false);
                        target = null;
                        for (int i = 0; i < collectables.Count; i++)
                        {
                            if (target == collectables[i])
                            {
                                collectables.RemoveAt(i);
                            }
                        }
                    }
                }
            }
            else
            {
                target = null;
            }
        }
    }
}
