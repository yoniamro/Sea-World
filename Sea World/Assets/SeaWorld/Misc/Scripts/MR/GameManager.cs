using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vuforia;
using System.IO;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Space(10)]
	public AudioSource source;

	public float audioClipsInterval = 3f;

    private PlayAudio playAudio;
    private List<TrackableBehaviour> trackableObjects = new List<TrackableBehaviour>();

    private TrackableBehaviour[] lastTrackedObjects;
    private TrackableBehaviour currentTrackedTarget;
    [HideInInspector]
    public TrackableBehaviour bigMarkerTrackable;
    [HideInInspector]
    public TrackableBehaviour currentTrackedPlayAudioTarget;



	//private int noOfActiveTrackables = 0;
    
    private float passedTime = 1.5f;
	private float animalNameTimer = 0.0f;
	private bool playedAnimalNameOnce = false;
    private bool playedAnimalVoiceOnce = false;

    [Space(10)]
    public UserSettings userSettings;


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

                //DELETE IF YOU DONT WANT TO RESET ANIMAL CLIPS WHEN THE MARKER TRACKING IS LOST.
                if(playAudio != null)
                {
                    playAudio.indexNumber = 0;
                }
                //

                passedTime = 1.75f;
				animalNameTimer = 0.0f;
				playedAnimalNameOnce = false;
            }

        }

    }

    void GetCurrentTrackableObjects()
    {
        // Get the StateManager
        StateManager sm = TrackerManager.Instance.GetStateManager();

        IEnumerable<TrackableBehaviour> activeTrackables = sm.GetActiveTrackableBehaviours();

        //noOfActiveTrackables = activeTrackables.Count();
        
		// Iterate through the list of active trackables
        foreach (TrackableBehaviour tb in activeTrackables)
        {

            if (tb.TrackableName != "action-card" && tb.TrackableName != "groundcard" && tb.TrackableName != "BigFishMarker")
            {
                currentTrackedPlayAudioTarget = tb;
				animalNameTimer += Time.deltaTime;

                if (currentTrackedPlayAudioTarget.GetComponentInChildren<PlayAudio>() != null)
                {
                    playAudio = currentTrackedPlayAudioTarget.GetComponentInChildren<PlayAudio>();
                }

                if(!playedAnimalNameOnce && animalNameTimer >= passedTime)
				{
                    playedAnimalNameOnce = true;
                    playedAnimalVoiceOnce = false;

                    if (playAudio != null)
                    {
                        playAudio.PlayAnimalNameClipOnClick();
						passedTime = passedTime + playAudio.currentClipLength + audioClipsInterval;
                    }

				}

                if (playedAnimalNameOnce && animalNameTimer >= passedTime)
                {

                    if (playAudio != null)
                    {
                        if(playAudio.indexNumber == 0 && !playedAnimalVoiceOnce)
                        {
                            playedAnimalVoiceOnce = true;                  
                            playAudio.PlayAnimalSoundClipOnClick();
							passedTime = passedTime + playAudio.currentClipLength + audioClipsInterval;
                        }
                        else
                        {
                            if(playAudio.indexNumber > playAudio.maxArrayLength - 1)
                            {
                                playedAnimalNameOnce = false;
                                
                                playAudio.indexNumber = 0;
                            }
                            else
                            {
                                playAudio.PlayInfoClipOnClick();
								passedTime = passedTime + playAudio.currentClipLength + audioClipsInterval;
                            }
                        }
                        
                    }
                }

            }

            if(tb.TrackableName != "groundcard" && tb.TrackableName != "BigFishMarker")
            {
                bigMarkerTrackable = tb;
            }

            currentTrackedTarget = tb;


            trackableObjects.Add(currentTrackedTarget);
        }
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
     
        if(userSettings != null)
        {
            userSettings.FadeBGMusicVolume(playAudio.GetAudioSource().clip.length + 0.5f);
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


        if (userSettings != null)
        {
            userSettings.FadeBGMusicVolume(playAudio.GetAudioSource().clip.length + 0.5f);
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


        if (userSettings != null)
        {
            userSettings.FadeBGMusicVolume(playAudio.GetAudioSource().clip.length + 0.5f);
        }


    }



    public void TakeScreenShot()
    {
		ScreenshotManager.SaveScreenshot("SeaWorld_Screenshot", "Sea World", "jpeg");
    }

	
  
}
