using UnityEngine;
using System.Collections;

public class VRSceneGameManager : MonoBehaviour 
{
	public LayerMask vrMask;

	private GameObject currentSelectedPlayAudioObject;
	private PlayAudioInVR playAudioInVR;


	void Update()
	{
		#if UNITY_EDITOR

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if(Input.GetMouseButtonDown(0))
		{
			if(Physics.Raycast(ray, out hit, 100f, vrMask))
			{
				if(hit.collider.gameObject.layer == 9)
				{
                    currentSelectedPlayAudioObject = hit.collider.gameObject;
                    if (currentSelectedPlayAudioObject.GetComponent<PlayAudioInVR>() != null)
                    {
                        playAudioInVR = currentSelectedPlayAudioObject.GetComponent<PlayAudioInVR>();
                    }
                }
			}
		}


#elif !UNITY_EDITOR

		if(Input.touchCount > 0)
		{
			RaycastHit hit;

			Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);


			if(Physics.Raycast(ray, out hit, 100f, vrMask))
			{
				if(hit.collider.gameObject.layer == 9)
				{
					currentSelectedPlayAudioObject = hit.collider.gameObject;
                    if (currentSelectedPlayAudioObject.GetComponent<PlayAudioInVR>() != null)
                    {
                        playAudioInVR = currentSelectedPlayAudioObject.GetComponent<PlayAudioInVR>();
                    }
				}			
			}

		}

#endif
    }


    public void PlayInfoClips()
	{
		if(currentSelectedPlayAudioObject == null)
		{
			Debug.Log ("No selected object");
			return;
		}

		if(currentSelectedPlayAudioObject.GetComponent<PlayAudioInVR>() == null)
		{
			Debug.Log ("No PlayAudioInVR script on this object");
			return;
		}

		playAudioInVR.PlayInfoClipOnClick();



	}

	public void PlayNameClip()
	{
		
		if(currentSelectedPlayAudioObject == null)
		{
			Debug.Log ("No selected object");
			return;
		}

		if(currentSelectedPlayAudioObject.GetComponent<PlayAudioInVR>() == null)
		{
			Debug.Log ("No PlayAudioInVR script on this object");
			return;
		}

		playAudioInVR.PlayAnimalNameClipOnClick();

	}

	public void PlaySoundClip()
	{

		if(currentSelectedPlayAudioObject == null)
		{
			Debug.Log ("No selected object");
			return;
		}


		if(currentSelectedPlayAudioObject.GetComponent<PlayAudioInVR>() == null)
		{
			Debug.Log ("No PlayAudioInVR script on this object");
			return;
		}

		playAudioInVR.PlayAnimalSoundClipOnClick();

	}
}
