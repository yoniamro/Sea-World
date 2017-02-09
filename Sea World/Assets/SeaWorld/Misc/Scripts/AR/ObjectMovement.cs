using UnityEngine;
using System.Collections;


public class ObjectMovement : MonoBehaviour
{
    public GameObject plane;
    public UnityEngine.AI.NavMeshAgent agent;


    [HideInInspector]
    public bool tracked = false;

    //private Plane raycastPlane;
    private Vector3 targetPos;
    private bool firstMovement = true;

    void Start()
    {

        if (agent != null)
        {
            targetPos = agent.transform.position;
        }

    }

    void Update()
    {
        if (tracked)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    if (touch.tapCount == 2)
                    {
                        if(plane != null)
                        {
                            plane.SetActive(true);
                        }
                        if(agent != null)
                        {
                            agent.enabled = true;
                        }

                        Ray ray = Camera.main.ScreenPointToRay(touch.position);

                        float dist;
                        if (GameManager_AR.raycastPlane.Raycast(ray, out dist))
                        {
                            targetPos = ray.GetPoint(dist);
                        }

                        firstMovement = false;
                    }

                }
            }

            if (agent != null && !firstMovement)
            {
                if (agent.enabled)
                {
                    agent.SetDestination(targetPos);
                    
                    if (agent.gameObject.GetComponent<Animator>() != null)
                    {
                        agent.gameObject.GetComponent<Animator>().SetFloat("Move", agent.velocity.magnitude);
                    }
                }

            }

            
        }

    }

    public void StopMovement()
    {
        firstMovement = true;

        if (agent != null)
        {
            agent.enabled = false;

            if (agent.gameObject.GetComponent<Animator>() != null)
            {
                agent.gameObject.GetComponent<Animator>().SetFloat("Move", 0);
            }
        }

        if (plane != null)
        {
            plane.SetActive(false);
        }
    }

}