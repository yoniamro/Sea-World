using UnityEngine;
using System.Collections;
using Vuforia;

public class ARTrackingHandler : MonoBehaviour, ITrackableEventHandler
{
    public ParticleSystem particleEffect;
    [Space(10)]
    public GameObject childObject;
    [Space(10)]
    public ObjectMovement objectMovement;

    private bool playedAnimalParticleOnce = false;
    private bool tracked = false;
    private bool showedObject = false;
    private float objectShowUpTimer = 0f;

    private TrackableBehaviour mTrackableBehaviour;

    private GameObject gearBox;

    private Vector3 modelStartPos;

    
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();

        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }

        if(objectMovement != null)
        {
            modelStartPos = objectMovement.agent.transform.localPosition;
        }
    }

    void Update()
    {

        if (tracked)
        {
            objectShowUpTimer += Time.deltaTime;

            if (particleEffect != null)
            {
                if (objectShowUpTimer > particleEffect.startLifetime && !showedObject)
                {
                    //OnTrackingFound();
                    if (childObject != null)
                    {
                        childObject.SetActive(true);
                    }

                    if (objectMovement != null)
                    {
                        objectMovement.enabled = true;
                        objectMovement.tracked = true;
                    }

                    showedObject = true;
                }
            }
            else
            {
                if (!showedObject)
                {
                    //OnTrackingFound();
                    if (childObject != null)
                    {
                        childObject.SetActive(true);
                    }

                    if (objectMovement != null)
                    {
                        objectMovement.enabled = true;
                        objectMovement.tracked = true;
                    }

                    showedObject = true;
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

            if (!playedAnimalParticleOnce)
            {
                if (particleEffect != null)
                {
                    if (transform.childCount > 0)
                    {
                        GameObject particle = Instantiate(particleEffect.gameObject, transform.GetChild(0).position, particleEffect.transform.rotation) as GameObject;
                        particle.transform.parent = gameObject.transform;
                        particle.transform.localPosition = Vector3.zero;
                        particle.transform.localRotation = Quaternion.Euler(90, 0, 0);
                        Destroy(particle, particleEffect.startLifetime);
                    }
                }
                playedAnimalParticleOnce = true;
            }

            tracked = true;

          
        }
        else
        {
            tracked = false;
            showedObject = false;
            playedAnimalParticleOnce = false;
            objectShowUpTimer = 0;
            //OnTrackingLost();

            if (objectMovement != null)
            {
                objectMovement.tracked = false;
                objectMovement.StopMovement();
                objectMovement.agent.transform.localPosition = modelStartPos;
                objectMovement.enabled = false;
            }

            if (childObject != null)
            {
                childObject.SetActive(false);
            }


        }
    }


    private void OnTrackingFound()
    {

        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

        // Enable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = true;
        }

        // Enable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = true;
        }

        //Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
    }


    private void OnTrackingLost()
    {

        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

        // Enable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = false;
        }

        // Enable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = false;
        }

        //Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
    }
}
