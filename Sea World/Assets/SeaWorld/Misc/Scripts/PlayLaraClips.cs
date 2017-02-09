using UnityEngine;
using System.Collections;

public class PlayLaraClips : MonoBehaviour 
{
    public static bool isActionCard = false;
    public static bool particleFinished = false;

    public Animator bigMarkerLaraAnimator;
    public Animator animator;
    public AudioSource source;
    [Space(10)]
    public AudioClip[] introClips;


    private bool playedIntroOnce = false;
    private string currentChoosenLanguage = "English";

    void Awake()
    {
        isActionCard = false;
        particleFinished = false;
    }

    void Start()
    {
        currentChoosenLanguage = PlayerPrefs.GetString("Language", "English");
    }

    void Update()
    {
        if (isActionCard && !playedIntroOnce && particleFinished)
        {
            switch (currentChoosenLanguage)
            {
                case "English":
                    source.clip = introClips[0];
                    break;
                case "Chinese":
                    source.clip = introClips[1];
                    break;
                case "Spanish":
                    source.clip = introClips[2];
                    break;
                case "Russian":
                    source.clip = introClips[3];
                    break;
                default:
                    source.clip = introClips[0];
                    break;
            }

            animator.SetTrigger("Talk");
            source.Play();

            playedIntroOnce = true;
        }
    }

    public void PlayIntroClip()
    {
        switch (currentChoosenLanguage)
        {
            case "English":
                source.clip = introClips[0];
                break;
            case "Chinese":
                source.clip = introClips[1];
                break;
            case "Spanish":
                source.clip = introClips[2];
                break;
            case "Russian":
                source.clip = introClips[3];
                break;
            default:
                source.clip = introClips[0];
                break;
        }

        animator.SetTrigger("Talk");
        source.Play();
    }

    public void Wave()
    {
        animator.SetTrigger("Wave");
        bigMarkerLaraAnimator.SetTrigger("Wave");
    }


}
