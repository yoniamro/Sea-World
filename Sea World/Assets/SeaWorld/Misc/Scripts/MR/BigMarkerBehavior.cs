using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Vuforia;

public class BigMarkerBehavior : MonoBehaviour, ITrackableEventHandler
{

    public ParticleSystem particleEffect;
    [Space(10)]
    public GameObject gearBox;
    [Space(10)]
    public GameObject[] children;

    private bool playedAnimalParticleOnce = false;
    private bool foundDuplicatedObject = false;
    private float animalShowUpTimer = 0f;


    private List<TrackableBehaviour> trackableObjects = new List<TrackableBehaviour>();
    private TrackableBehaviour[] lastTrackedObjects;
    private TrackableBehaviour currentTrackedBehavior;
    private TrackableBehaviour thisTrackableBehavior;
    private TrackableBehaviour mTrackableBehaviour;

    [HideInInspector]
    public bool showedObject = false;
    [HideInInspector]
    public bool tracked = false;
    [HideInInspector]
    public GameObject lastObject;
    [HideInInspector]
    public bool updatedFoodType = false;


    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }

        if (gearBox != null)
        {
            gearBox.SetActive(false);
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
            animalShowUpTimer = 0;
            foundDuplicatedObject = false;

            //OnTrackingLost();

            foreach (GameObject obj in children)
            {
                obj.SetActive(false);
            }

            if(gearBox != null)
            {
                gearBox.SetActive(false);
            }
        }
    }


    void Update()
    {
        lastTrackedObjects = new TrackableBehaviour[trackableObjects.Count];
        trackableObjects.CopyTo(lastTrackedObjects);
        trackableObjects.Clear();


        GetActiveTrackables();

        foreach (GameObject c in children)
        {
            if (currentTrackedBehavior != null)
            {
                if (c.name == currentTrackedBehavior.TrackableName)
                {
                    foundDuplicatedObject = true;
                }
            }
        }

        foreach (TrackableBehaviour trackable in lastTrackedObjects)
        {
            if (!trackableObjects.Contains(trackable))
            {
                showedObject = false;
                updatedFoodType = false;

                foreach (GameObject o in children)
                {
                    if (o.name == trackable.TrackableName && foundDuplicatedObject)
                    {
                        o.SetActive(false);
                        animalShowUpTimer = 0;

                        if (thisTrackableBehavior != null)
                        {
                            PlayParticles();
                        }


                    }

                }
            }
        }

        if (tracked)
        {


            animalShowUpTimer += Time.deltaTime;



            if (particleEffect != null)
            {
                if (animalShowUpTimer > 2 && !showedObject)
                {
                    //OnTrackingFound();


                    if (currentTrackedBehavior != null)
                    {
                        foreach (GameObject obj in children)
                        {
                            if (currentTrackedBehavior.TrackableName == obj.name)
                            {
                                lastObject = obj;
                            }
                        }
                    }


                    if (lastObject != null)
                    {
                        lastObject.SetActive(true);

                    }

                    if (gearBox != null)
                    {
                        gearBox.SetActive(true);
                    }
                    showedObject = true;

                }
            }
            else
            {
                if (!showedObject)
                {
                    //OnTrackingFound();
                    if (currentTrackedBehavior != null)
                    {
                        foreach (GameObject obj in children)
                        {
                            if (currentTrackedBehavior.TrackableName == obj.name)
                            {
                                lastObject = obj;
                            }
                        }
                    }


                    if (lastObject != null)
                    {
                        lastObject.SetActive(true);
                    }

                    if (gearBox != null)
                    {
                        gearBox.SetActive(true);
                    }

                    showedObject = true;
                }
            }


            //if (lastObject != null)
            //{
            //    if (lastObject.GetComponent<Animator>() != null)
            //    {
            //        if (lastObject.GetComponent<Animator>().parameterCount > 1)
            //        {
            //            if (lastObject.GetComponent<Animator>().GetParameter(1).name == "Move")
            //            {
            //                lastObject.GetComponent<Animator>().SetFloat("Move", agent.velocity.magnitude);
            //            }
            //        }
            //    }
            //}

        }
    }

    //IEnumerator StartMovingToPos()
    //{
    //    while(tracked && agent != null)
    //    {

    //        yield return new WaitForSeconds(0.25f);
    //        Debug.Log("inside coroutine");
    //        agent.SetDestination(targetPos);
    //    }
    //}

    private void PlayParticles()
    {
        GameObject particle = Instantiate(particleEffect.gameObject, transform.GetChild(0).position, particleEffect.transform.rotation) as GameObject;
        particle.transform.parent = gameObject.transform;
        particle.transform.localPosition = new Vector3(0, 0.1f, 0);
        particle.transform.localRotation = Quaternion.Euler(90, 0, 0);
        Destroy(particle, 2);
    }


    private void GetActiveTrackables()
    {
        // Get the StateManager
        StateManager sm = TrackerManager.Instance.GetStateManager();

        IEnumerable<TrackableBehaviour> activeTrackables = sm.GetActiveTrackableBehaviours();

        thisTrackableBehavior = null;
        foundDuplicatedObject = false;

        foreach (TrackableBehaviour tb in activeTrackables)
        {
            trackableObjects.Add(tb);

            if (tb.TrackableName != "BigFishMarker" && tb.TrackableName != "groundcard" && tb.TrackableName != "action-card")
            {
                currentTrackedBehavior = tb;
            }

            if (tb.TrackableName == "BigFishMarker")
            {
                thisTrackableBehavior = tb;
            }

        }
    }
}

