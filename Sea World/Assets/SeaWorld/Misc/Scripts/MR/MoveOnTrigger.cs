using UnityEngine;
using System.Collections;
using Vuforia;

public class MoveOnTrigger : MonoBehaviour, ITrackableEventHandler
{
    public GameObject plane;
    private Vector3 targetPos;

    private UnityEngine.AI.NavMeshAgent agent;
    private TrackableBehaviour mTrackableBehaviour;

    private bool tracked = false;
    private bool firstMovement = true;

    private Plane raycastPlane;
    private Ray ray;


    //private Vector3 startLocalPos;
    //private Quaternion startLocalRot;
    
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();

        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        raycastPlane = new Plane(-Camera.main.transform.forward, Vector3.zero);


        if(GetComponentInChildren<UnityEngine.AI.NavMeshAgent>() != null)
        {
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            agent.enabled = false;
        }

        targetPos = transform.GetChild(0).position;

        //startLocalPos = transform.GetChild(0).localPosition;
        //startLocalRot = transform.GetChild(0).localRotation;


        if (plane != null)
        {
            plane.SetActive(false);
        }
    }

    void Update()
    {
        if (tracked)
        {
            if (Cardboard.SDK.Triggered) 
            {
              
                ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward * 10000f);
                
                plane.SetActive(true);

                if (agent != null)
                {
                    agent.enabled = true;
                }
              
                //Debug.DrawLine(ray.origin, ray.direction * 10000f, Color.red);

                float dist;
                
                if (raycastPlane.Raycast(ray, out dist))
                {
                    targetPos = ray.GetPoint(dist);
                    //Debug.Log(targetPos);
                    //bug.Log("Plane Raycasted!");
                }

                firstMovement = false;
                 
            }

            if (agent != null && !firstMovement)
            {
                if (agent.enabled)
                {
                    agent.SetDestination(targetPos);
                }

            }

            if (mTrackableBehaviour.TrackableName != "groundcard" && mTrackableBehaviour.TrackableName != "actionCard" && mTrackableBehaviour.TrackableName != "BigMarker")
            {
                if (transform.GetChild(0).GetComponent<Animator>())
                {
                    if (transform.GetChild(0).GetComponent<Animator>().GetParameter(1).name == "Move")
                    {
                        transform.GetChild(0).GetComponent<Animator>().SetFloat("Move", agent.velocity.magnitude);
                    }
                }
            }
        }

    }

    public void OnTrackableStateChanged(
                                     TrackableBehaviour.Status previousStatus,
                                     TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            //plane.SetActive(true);
            //if (agent != null)
            //{
            //    agent.enabled = true;
            //}

            tracked = true;

        }
        else
        {
            firstMovement = true;
            tracked = false;
            
            if (agent != null)
            {
                agent.enabled = false;
            }

            if (plane != null)
            {
                plane.SetActive(false);
            }

            //transform.GetChild(0).localPosition = startLocalPos;
            //transform.GetChild(0).localRotation = Quaternion.Euler(-45f, -180f, 0f);
        }
    }
}
