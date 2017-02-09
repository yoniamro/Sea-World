using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vuforia;
using System.IO;
using UnityEngine.UI;

public class GameManager_AR : MonoBehaviour
{
    [Space(10)]
    public AudioSource source;
    [Space(10)]
    public GameObject[] iconsUI;
    [Space(10)]
    public GameObject[] m_UIOjects;


    [Header("Questions Clips...")]
    [Space(10)]
    public AudioClip[] englishQuestionClips;
    [Space(10)]
    public AudioClip[] chineseQuestionClips;

    [Space(10)]
    public AudioClip[] answers;
    [Space(10)]
    public GameObject[] objects;
    [Space(10)]
    public string[] names;

    [HideInInspector]
    public static Plane raycastPlane;


    private PlayAudio playAudio;
    private List<TrackableBehaviour> trackableObjects = new List<TrackableBehaviour>();

    private TrackableBehaviour[] lastTrackedObjects;
    private TrackableBehaviour currentTrackedTarget;
    [HideInInspector]
    public TrackableBehaviour currentTrackedPlayAudioTarget;

    private string currentTrackedTargetName;
 


    private int randomIndex = 0;
    private float msTimeBeforePlayingCorrectOrWrong = 0;
    private bool scannedCurrentTrackable = true;
    private bool answeredCorrectly = true;
    private bool breakOutOfQuestions = false;

    private int noOfActiveTrackables = 0;

    private float cardTrackingTimer = 0.0f;
    private bool playedAnimalNameOnce = false;
    private bool takingScreenshot = false;
    private float timeBeforeQuestionsPlayRightOrWRong = 3f;


    private string currentChoosenLanguage = "English";

    [Space(10)]
    public UserSettings userSettings;


    void Start()
    {
        currentChoosenLanguage = PlayerPrefs.GetString("Language", "English");

        raycastPlane = new Plane(-Camera.main.transform.forward, Vector3.zero);
    }

    void Update()
    {
        lastTrackedObjects = new TrackableBehaviour[trackableObjects.Count];
        trackableObjects.CopyTo(lastTrackedObjects);
        trackableObjects.Clear();

        GetCurrentTrackableObjects();

        foreach (TrackableBehaviour trackable in lastTrackedObjects)
        {
            if (!trackableObjects.Contains(trackable))
            {
                if (trackable.GetComponentInChildren<TouchControls>())
                {
                    trackable.GetComponentInChildren<TouchControls>().objectVisible = false;
                }


                foreach (GameObject obj in objects)
                {
                    if (obj.name == currentTrackedTarget.TrackableName)
                    {
                        foreach (GameObject objectUI in iconsUI)
                            objectUI.SetActive(false);


                        obj.GetComponent<SnappedObject>().refObject.SetActive(true);

                        obj.GetComponent<SnappedObject>().InitTransform();
                        
                    }

                }
                cardTrackingTimer = 0.0f;
                playedAnimalNameOnce = false;
          
                scannedCurrentTrackable = false;
                msTimeBeforePlayingCorrectOrWrong = Time.time + 1.5f;

            }
            
        }

    }

    void GetCurrentTrackableObjects()
    {
        // Get the StateManager
        StateManager sm = TrackerManager.Instance.GetStateManager();

        IEnumerable<TrackableBehaviour> activeTrackables = sm.GetActiveTrackableBehaviours();

        noOfActiveTrackables = activeTrackables.Count();

        // Iterate through the list of active trackables
        foreach (TrackableBehaviour tb in activeTrackables)
        {

            //Debug.Log("Trackable: " + tb.TrackableName);

            if (tb.TrackableName != "action-card" && tb.TrackableName != "groundcard" && tb.TrackableName != "BigFishMarker")
            {
                currentTrackedPlayAudioTarget = tb;
                cardTrackingTimer += Time.deltaTime;



                if (!playedAnimalNameOnce && cardTrackingTimer >= 1.75f)
                {
                    if (currentTrackedPlayAudioTarget.GetComponentInChildren<PlayAudio>() != null)
                    {
                        playAudio = currentTrackedPlayAudioTarget.GetComponentInChildren<PlayAudio>();
						playAudio.PlayAnimalNameClipOnClick();
                    }
                    playedAnimalNameOnce = true;
                }
            }

            currentTrackedTarget = tb;


            if (currentTrackedTarget.GetComponentInChildren<TouchControls>())
            {
                currentTrackedTarget.GetComponentInChildren<TouchControls>().objectVisible = true;
            }

           
            if (currentTrackedTarget.TrackableName == "action-card" && noOfActiveTrackables == 1)
            {
                foreach (GameObject objectUI in iconsUI)
                    objectUI.SetActive(false);
            }
            else
            {
                if (!takingScreenshot)
                {
                    foreach (GameObject objectUI in iconsUI)
                        objectUI.SetActive(true);
                }
            }

            foreach (GameObject obj in objects)
            {
                GameObject ro = obj.GetComponent<SnappedObject>().refObject;

                if (ro != null)
                {
                    if(ro.activeSelf)
                    {
                        ro.SetActive (false);
                    }
				}
            }

            trackableObjects.Add(currentTrackedTarget);
            currentTrackedTargetName = currentTrackedTarget.TrackableName;

            if (!source.isPlaying && !breakOutOfQuestions && currentTrackedTargetName != "action-card" && currentTrackedTargetName != "groundcard")
            {
                if (!scannedCurrentTrackable && cardTrackingTimer >= timeBeforeQuestionsPlayRightOrWRong)
                {

                    if (!answeredCorrectly)
                    {
                        if (Time.time > msTimeBeforePlayingCorrectOrWrong)
                        {
                            if (currentTrackedTargetName == names[randomIndex])
                            {
                                switch (currentChoosenLanguage)
                                {
                                    case "English":
                                        source.clip = answers[0];
                                        break;
                                    case "Chinese":
                                        source.clip = answers[2];
                                        break;
                                    default:
                                        source.clip = answers[0];
                                        break;
                                }

                                if(userSettings != null)
                                {
                                    userSettings.FadeBGMusicVolume(source.clip.length + 0.5f);
                                }

                                source.Play();
                                answeredCorrectly = true;
                            }
                            else
                            {
                                switch (currentChoosenLanguage)
                                {
                                    case "English":
                                        source.clip = answers[1];
                                        break;
                                    case "Chinese":
                                        source.clip = answers[3];
                                        break;                  
                                    default:
                                        source.clip = answers[1];
                                        break;
                                }

                                if (userSettings != null)
                                {
                                    userSettings.FadeBGMusicVolume(source.clip.length + 0.5f);
                                }
                                source.Play();
                            }

                            scannedCurrentTrackable = true;
                        }
                    }
                }
            }

        }
    }

    public void PlayQuestionClips()
    {
        foreach (GameObject gameObj in objects)
        {
            if(gameObj.activeSelf)
            {
                gameObj.SetActive(false);
            }

        }

        if (!source.isPlaying)
        {
            randomIndex = Random.Range(0, englishQuestionClips.Length);

            switch (currentChoosenLanguage)
            {
                case "English":
                    source.clip = englishQuestionClips[randomIndex];
                    break;
                case "Chinese":
                    source.clip = chineseQuestionClips[randomIndex];
                    break;
                default:
                    source.clip = englishQuestionClips[randomIndex];
                    break;
            }

            if(userSettings != null)
            {
                userSettings.FadeBGMusicVolume(source.clip.length + 0.5f);
            }

            source.Play();


            msTimeBeforePlayingCorrectOrWrong = Time.time + source.clip.length + 2f;
        }

        breakOutOfQuestions = false;
        scannedCurrentTrackable = false;
        answeredCorrectly = false;
    }

    public void PlayInfoClips()
    {
        if (trackableObjects.Count < 1)
        {
            Debug.Log("No Trackables Found!");
            return;
        }

        if (currentTrackedPlayAudioTarget == null || currentTrackedPlayAudioTarget.GetComponentInChildren<PlayAudio>() == null)
        {
            Debug.Log("No PlayAudio script attached to this object, or there is no active trackable target atm!");
            return;
        }


        breakOutOfQuestions = true;
		if (playAudio != null)
		{
			playAudio.PlayInfoClipOnClick ();

            if (userSettings != null)
            {
			    userSettings.FadeBGMusicVolume(playAudio.GetAudioSource().clip.length + 0.5f);
            }
		}


    }

    public void PlayNameClip()
    {
        if (trackableObjects.Count < 1)
        {
            Debug.Log("No Trackables Found!");
            return;
        }

        if (currentTrackedPlayAudioTarget == null || currentTrackedPlayAudioTarget.GetComponentInChildren<PlayAudio>() == null)
        {
            Debug.Log("No PlayAudio script attached to this object, or there is no active trackable target atm!");
            return;
        }

        breakOutOfQuestions = true;

		if (playAudio != null) 
		{
			playAudio.PlayAnimalNameClipOnClick ();

            if (userSettings != null)
            {
                userSettings.FadeBGMusicVolume(playAudio.GetAudioSource().clip.length + 0.5f);
            }
        }

    }

    public void PlaySoundClip()
    {
        if (trackableObjects.Count < 1)
        {
            Debug.Log("No Trackables Found!");
            return;
        }

        if (currentTrackedPlayAudioTarget == null || currentTrackedPlayAudioTarget.GetComponentInChildren<PlayAudio>() == null)
        {
            Debug.Log("No PlayAudio script attached to this object, or there is no active trackable target atm!");
            return;
        }

        breakOutOfQuestions = true;

		if (playAudio != null) 
		{
			playAudio.PlayAnimalSoundClipOnClick();


            if (userSettings != null)
            {
                userSettings.FadeBGMusicVolume(playAudio.GetAudioSource().clip.length + 0.5f);
            }
        }

    }



    public void TakeScreenShot()
    {
        takingScreenshot = true;

        foreach (GameObject obj in m_UIOjects)
        {
            obj.SetActive(false);
        }
        foreach (GameObject objUI in iconsUI)
        {
            objUI.SetActive(false);
        }

        ScreenshotManager.SaveScreenshot("SeaWorld_Screenshot", "Sea World", "jpeg");

        Invoke("TurnOnUI", 0.5f);
    }

    private void TurnOnUI()
    {
        foreach (GameObject obj in m_UIOjects)
        {
            obj.SetActive(true);
        }

        if (noOfActiveTrackables == 0)
        {
            foreach (GameObject buttonObj in iconsUI)
                buttonObj.SetActive(false);
        }
        else
        {
            foreach (GameObject buttonObj in iconsUI)
                buttonObj.SetActive(true);
        }

        takingScreenshot = false;

    }


}
