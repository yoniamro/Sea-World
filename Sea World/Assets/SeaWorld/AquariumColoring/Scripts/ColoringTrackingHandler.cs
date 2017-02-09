using UnityEngine;
using System.Collections;
using Vuforia;

public class ColoringTrackingHandler : MonoBehaviour, ITrackableEventHandler
{
    public ParticleSystem particleEffect;
    public GameObject fish;
    public GameObject RC;
    public GameObject aquariumButton;

    private TrackableBehaviour mTrackableBehaviour;

    private bool playedAnimalParticleOnce = false;
    private bool tracked = false;
    private bool showedObject = false;
    private float objectShowUpTimer = 0f;

    void Start ()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();

        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
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

            OnTrackingLost();
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

        }
    }

    private void OnTrackingFound()
    {
        if (RC != null)
        {
            RC.SetActive(true);
        }

        if (fish != null)
        {
            fish.SetActive(true);
        }

        if (aquariumButton != null)
        {
            aquariumButton.SetActive(true);
        }
    }


    private void OnTrackingLost()
    {
        if (aquariumButton != null)
        {
            aquariumButton.SetActive(false);
        }

        if (fish != null)
        {
            fish.SetActive(false);
        }

        if (RC != null)
        {
            RC.SetActive(false);
        }
    }
}
