using UnityEngine;
using System.Collections;

public class UserSettings : MonoBehaviour
{
    [Header("Background AudioSource..")]
    public AudioSource bgMusicSource;


    void Start()
    {        
        if (bgMusicSource != null)
        {
            bgMusicSource.volume = PlayerPrefs.GetFloat("BGVOLUME", 0.2f);
        }

    }

    //void Update()
    //{
    //    if (source != null && !source.isPlaying)
    //    {
    //        IEnumerator inst = null;
    //        inst = FadeVolumeInOut(bgMusicSource.volume, PlayerPrefs.GetFloat("BGVOLUME", 0.2f));
    //        StopCoroutine(inst);
    //        StartCoroutine(inst);
    //    }
    //}

    public void FadeBGMusicVolume(float fadeDuration)
    {

        StartCoroutine(FadeVolumeInOut(bgMusicSource.volume, 0));

        Invoke("FadeBGMusicVolumeBack", fadeDuration);

    }

    void FadeBGMusicVolumeBack()
    {
        bgMusicSource.enabled = true;
        StartCoroutine(FadeVolumeInOut(bgMusicSource.volume, PlayerPrefs.GetFloat("BGVOLUME", 0.2f)));
    }

    IEnumerator FadeVolumeInOut(float startVal, float endVal)
    {
        float percent = 0;
        
        while(percent <= 1)
        {
            percent += Time.deltaTime * 1.5f;

            bgMusicSource.volume = Mathf.Lerp(startVal, endVal, percent);

            yield return null;    
        }
    }

}
