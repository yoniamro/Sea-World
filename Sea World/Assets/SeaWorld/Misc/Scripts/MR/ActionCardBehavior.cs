using UnityEngine;
using System.Collections;
using Vuforia;

public class ActionCardBehavior : MonoBehaviour, ITrackableEventHandler
{
    public ParticleSystem particleEffect;
    [Space(10)]
    public GameObject gearBox;
    [Space(10)]
    public BigMarkerBehavior bigMarkerBehavior;



    private bool playedAnimalParticleOnce = false;
    private bool tracked = false;
    private bool showedObject = false;
    private float objectShowUpTimer = 0f;

    private TrackableBehaviour mTrackableBehaviour;

    //Current tracked object components.
    private GameObject currentActiveBigMarkerChild;
    private Animator currentActiveBigMarkerChildAnimator;
    private UnityEngine.AI.NavMeshAgent currentActiveBigMarkerChildAgent;

    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
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
                    OnTrackingFound();
                    showedObject = true;
                }
            }
            else
            {
                if (!showedObject)
                {
                    OnTrackingFound();
                    showedObject = true;
                }
            }


            if(bigMarkerBehavior != null)
            {
                if(bigMarkerBehavior.tracked && bigMarkerBehavior.showedObject && showedObject)
                {
                    if (bigMarkerBehavior.lastObject != null)
                    {
                        currentActiveBigMarkerChild = bigMarkerBehavior.lastObject;

                        if (currentActiveBigMarkerChild.GetComponent<Animator>() != null)
                        {
                            currentActiveBigMarkerChildAnimator = currentActiveBigMarkerChild.GetComponent<Animator>();

                            if (currentActiveBigMarkerChild.name != "duck" && currentActiveBigMarkerChild.name != "chimpanzee"
                                && currentActiveBigMarkerChild.name != "chicken" && currentActiveBigMarkerChild.name != "peakock"
                                && currentActiveBigMarkerChild.name != "owl" && currentActiveBigMarkerChild.name != "snake")
                            {
                                currentActiveBigMarkerChildAnimator.SetTrigger("Eat");
                            }
                            else
                            {
                                currentActiveBigMarkerChildAnimator.SetTrigger("Sound");
                            }

                        }


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

            if (!playedAnimalParticleOnce)
            {
                if (particleEffect != null)
                {
                    if (transform.childCount > 0)
                    {
                        PlayParticles();
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

            gearBox.SetActive(false);
        }
    }

    private void PlayParticles()
    {
        GameObject particle = Instantiate(particleEffect.gameObject, transform.GetChild(0).localPosition + Vector3.up * 0.2f, particleEffect.transform.rotation) as GameObject;
        particle.transform.parent = gameObject.transform;
        particle.transform.localPosition = Vector3.zero;
        particle.transform.localRotation = Quaternion.Euler(90, 0, 0);
        Destroy(particle, particleEffect.startLifetime);
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
