using UnityEngine;
using Vuforia;
using System.Collections;

public class EatingBehavior : MonoBehaviour, ITrackableEventHandler
{
    public GameObject gearBox;
    [Space(10)]
    public GameObject meatObject;
    public GameObject wheatObject;
    public GameObject nutsObject;

    private TrackableBehaviour mTrackableBehaviour;
    private BigMarkerBehavior bigMarkerBehavior;

    private bool tracked = false;

    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }

        bigMarkerBehavior = FindObjectOfType<BigMarkerBehavior>();
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
        }
        else
        {
            tracked = false;
            meatObject.SetActive(false);
            wheatObject.SetActive(false);
            nutsObject.SetActive(false);
            gearBox.SetActive(false);
            if(bigMarkerBehavior != null)
            {
                bigMarkerBehavior.updatedFoodType = false;
            }
        }
    }

    void Update()
    {
        if(tracked)
        {
            if(bigMarkerBehavior != null)
            {
                if(bigMarkerBehavior.lastObject != null && !bigMarkerBehavior.updatedFoodType)
                {
                    if(bigMarkerBehavior.lastObject.GetComponent<EatingEntity>() != null)
                    {
                        if (bigMarkerBehavior.lastObject.GetComponent<EatingEntity>().GetFoodType() == EatingEntity.FoodType.Meat)
                        {
                            meatObject.SetActive(true);
                            wheatObject.SetActive(false);
                            nutsObject.SetActive(false);
                        }
                        else if (bigMarkerBehavior.lastObject.GetComponent<EatingEntity>().GetFoodType() == EatingEntity.FoodType.Wheat)
                        {
                            meatObject.SetActive(false);
                            wheatObject.SetActive(true);
                            nutsObject.SetActive(false);
                        }
                        else if (bigMarkerBehavior.lastObject.GetComponent<EatingEntity>().GetFoodType() == EatingEntity.FoodType.Nuts)
                        {
                            meatObject.SetActive(false);
                            wheatObject.SetActive(false);
                            nutsObject.SetActive(true);
                        }
                        gearBox.SetActive(true);
                        bigMarkerBehavior.updatedFoodType = true;
                    }
                }
                else if(bigMarkerBehavior.lastObject != null)
                {
                    if(bigMarkerBehavior.lastObject.GetComponent<EatingEntity>() == null)
                    {
                        if(bigMarkerBehavior.lastObject.GetComponent<Animator>() != null)
                        {
                            bigMarkerBehavior.lastObject.GetComponent<Animator>().SetTrigger("Sound");
                        }
                    }
                }
            }
        }
    }
}
