using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Vuforia;

public class ColoringTrackingBehavior : MonoBehaviour, ITrackableEventHandler
{
    public ParticleSystem particleEffect;
    [Space(10)]
    public GameObject child;
    [Space(10)]
    public GameObject regionCapture;

    private TrackableBehaviour mTrackableBehaviour;
    private bool playedAnimalParticleOnce = false;
    private bool tracked = false;
    private bool showedObject = false;
    private float animalShowUpTimer = 0f;



    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }

        if (child != null)
        {
            child.SetActive(false);
        }

        if (regionCapture != null)
        {
            regionCapture.SetActive(false);
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
            animalShowUpTimer = 0;
            //OnTrackingLost();
            if (child != null)
            { 
                child.SetActive(false); 
            }

            if(regionCapture != null)
            {
                regionCapture.SetActive(false);
            }
        }
    }

    void Update()
    {

        if (tracked)
        {
            animalShowUpTimer += Time.deltaTime;

            if (particleEffect != null)
            {
                if (animalShowUpTimer > particleEffect.startLifetime && !showedObject)
                {
                    //OnTrackingFound();
                    if (regionCapture != null)
                    {
                        regionCapture.SetActive(true);
                    }

                    if (child != null)
                    {
                        child.SetActive(true);
                    }

                    showedObject = true;
                }
            }
            else
            {
                if (!showedObject)
                {
                    //OnTrackingFound();
                    if (regionCapture != null)
                    {
                        regionCapture.SetActive(true);
                    }
                    if(child != null)
                    {
                        child.SetActive(true);
                    }
                    showedObject = true;
                }
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
