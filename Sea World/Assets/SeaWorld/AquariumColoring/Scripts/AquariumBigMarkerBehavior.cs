using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vuforia;

public class AquariumBigMarkerBehavior : MonoBehaviour, ITrackableEventHandler
{

    public GameObject fishBowl;
    public GameObject fishHolder;
    public GameObject cleanButton;
    public GameObject foodButton;
    public GameObject forbiddenArea;

    [Space(20)]
    public GameObject fishFoodPrefab;
    public GameObject[] foodPoints;

    public bool tracked = false;
    public bool eatingStateEnabled = false;
    
    
    private List<TrackableBehaviour> trackableObjects = new List<TrackableBehaviour>();
    private TrackableBehaviour[] lastTrackedObjects;
    private TrackableBehaviour mTrackableBehaviour;

    void Start()
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
            tracked = true;

            if (fishBowl != null)
            {
                fishBowl.SetActive(true);
            }

            if(fishHolder != null)
            {
                fishHolder.SetActive(true);
            }

            if(cleanButton != null)
            {
                cleanButton.SetActive(true);
            }

            if (foodButton != null)
            {
                foodButton.SetActive(true);
            }
        }       
        else
        {
            tracked = false;

            if (fishBowl != null)
            {
                fishBowl.SetActive(false);
            }

            if (fishHolder != null)
            {
                fishHolder.SetActive(false);
            }

            if (cleanButton != null)
            {
                cleanButton.SetActive(false);
            }

            if (foodButton != null)
            {
                foodButton.SetActive(false);
            }
        }
    }

    public void CleanUpTank()
    {
        if(fishHolder.transform.childCount > 0)
        {
            foreach(Transform t in fishHolder.transform)
            {
                Joysticks joysticks = FindObjectOfType<Joysticks>();

                if(joysticks != null)
                {
                    if(joysticks.fishPlayer.name == t.gameObject.name)
                    {
                        joysticks.enableMovement = false;
                        joysticks.gameObject.SetActive(false);
                    }

                }

                Destroy(t.gameObject);

            }
        }
    }


    public void FeedFish()
    {
        for (int i = 0; i < foodPoints.Length; i++)
        {
            GameObject fishFood = (GameObject)Instantiate(fishFoodPrefab, fishFoodPrefab.transform.position, fishFoodPrefab.transform.rotation);

            fishFood.transform.parent = fishHolder.transform;
            fishFood.transform.localPosition = foodPoints[i].transform.localPosition;

            Destroy(fishFood, 10);
        }

        eatingStateEnabled = true;
        Invoke("DisbaleEatingState", 10);
    }


    void DisbaleEatingState()
    {
        Debug.Log("Invoked Disable Eating State!");
        eatingStateEnabled = false;
    }
    //void Update()
    //{
    //    lastTrackedObjects = new TrackableBehaviour[trackableObjects.Count];
    //    trackableObjects.CopyTo(lastTrackedObjects);
    //    trackableObjects.Clear();


    //    GetActiveTrackables();


    //}

    //private void GetActiveTrackables()
    //{
    //    // Get the StateManager
    //    StateManager sm = TrackerManager.Instance.GetStateManager();

    //    IEnumerable<TrackableBehaviour> activeTrackables = sm.GetActiveTrackableBehaviours();


    //    foreach (TrackableBehaviour tb in activeTrackables)
    //    {
    //        trackableObjects.Add(tb);

    //        if (tb.TrackableName != "BigFishMarker" && tb.TrackableName != "groundcard" && tb.TrackableName != "action-card")
    //        {
             
    //        }

           

    //    }
    //}

}
