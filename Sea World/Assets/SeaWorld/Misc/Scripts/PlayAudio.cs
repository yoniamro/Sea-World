using UnityEngine;
using System.Collections;

public class PlayAudio : MonoBehaviour 
{
	public AudioClip animalSound;
	[Space(10)]
	public AudioClip[] animalName;
	[Space(10)]
    [Header("English...")]
	public AudioClip[] englishClips;
	[Space(10)]
	[Header("Chinese...")]
    public AudioClip[] chineseClips;
   
    [Space(10)]
	public AudioSource source;
	public Animator anim;
	
    [HideInInspector]
    public int indexNumber;
    [HideInInspector]
    public int maxArrayLength;
    [HideInInspector]
    public float currentClipLength;


    [Space(10)]
    public UserSettings userSettings;

    private string currentChoosenLanguage = "English";


    void Start()
    {
        currentChoosenLanguage = PlayerPrefs.GetString("Language", "English");

        switch(currentChoosenLanguage)
        {
            case "English":
                if(englishClips.Length > 0)
                {
                    maxArrayLength = englishClips.Length;
                }
                break;
            case "Chinese":
                if(chineseClips.Length > 0)
                {
                    maxArrayLength = chineseClips.Length;
                }
                break;
            default:
                if (englishClips.Length > 0)
                {
                    maxArrayLength = englishClips.Length;
                }
                break;
        }

    }

    public void PlayInfoClipOnClick()
	{
		//if (!source.isPlaying) 
		//{
		indexNumber++;

        switch (currentChoosenLanguage)
        {
            case "English":

                if (indexNumber > englishClips.Length)
                    indexNumber = 1;

                source.clip = englishClips[indexNumber - 1];
                currentClipLength = source.clip.length;
                break;

            case "Chinese":

                if (indexNumber > chineseClips.Length)
                    indexNumber = 1;

                source.clip = chineseClips[indexNumber - 1];
                currentClipLength = source.clip.length;
                break;

            default:

                if (indexNumber > englishClips.Length)
                    indexNumber = 1;

                source.clip = englishClips[indexNumber - 1];
                currentClipLength = source.clip.length;
                break;
        }

		if (userSettings != null)
        {		
			userSettings.FadeBGMusicVolume (currentClipLength + 0.5f);
		}

        if(source.clip != null)
        {
            source.Play();
        }
        //}

    }

	public void PlayAnimalNameClipOnClick()
	{
        //if (!source.isPlaying)
        //{
        switch (currentChoosenLanguage)
        {
            case "English":
                source.clip = animalName[0];
                currentClipLength = source.clip.length;
                break;

            case "Chinese":

                source.clip = animalName[1];
                currentClipLength = source.clip.length;
                break;
//
//                case "Spanish":
//
//                    source.clip = animalName[2];
//                    currentClipLength = source.clip.length;
//                    break;
//
//                case "Russian":
//
//                    source.clip = animalName[3];
//                    currentClipLength = source.clip.length;
//                    break;

            default:

                source.clip = animalName[0];
                currentClipLength = source.clip.length;
                break;
        }

		if (userSettings != null) {
			
			userSettings.FadeBGMusicVolume(currentClipLength + 0.5f);
		}
		
        if(source.clip != null)
        {
		    source.Play ();
        }
        //}
    }

	public void PlayAnimalSoundClipOnClick()
	{
        //if (!source.isPlaying)
        //{
        source.clip = animalSound;
        currentClipLength = source.clip.length;

		if (userSettings != null)
        {
			userSettings.FadeBGMusicVolume (currentClipLength + 0.5f);
		}

        if(source.clip != null)
        {
            source.Play();
            anim.SetTrigger("Sound");
        }
        //
    }

    public AudioSource GetAudioSource()
    {
        return source;
    }

	
}
