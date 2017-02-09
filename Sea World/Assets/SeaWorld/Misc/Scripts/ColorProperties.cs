using UnityEngine;
using System.Collections;

public class ColorProperties : MonoBehaviour 
{
	public Color color;
    public AudioClip colorName;
    public AudioSource source;
    [Space(10)]
    public GroundCardScenario scenario;


	public Color GetColor()
	{
		return color;
	}

    public void PlayColorName()
    {
        source.clip = colorName;
        source.Play();
    }

    public void SetColorForCardboard()
    {
        PlayColorName();
        scenario.SetColorForCardboard(color);
    }
}
