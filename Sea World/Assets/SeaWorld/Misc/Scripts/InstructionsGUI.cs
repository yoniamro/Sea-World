using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InstructionsGUI : MonoBehaviour 
{
	[Space(10)]
	public Sprite[] infoScreens;
	[Space(10)]
	public GameObject infoImage;
	public GameObject backButton;
	public GameObject nextButton;
	public GameObject startButton;

	private int infoScreenNo = 0;

	void Start()
	{
		infoScreenNo = 0;
		infoImage.SetActive (true);
		backButton.SetActive (true);
		nextButton.SetActive (true);
		startButton.SetActive (true);

        if (infoScreenNo >= infoScreens.Length - 1)
        {
            nextButton.SetActive(false);
        }

        if(infoScreens.Length < 2 || infoScreenNo <= 0)
        {
            backButton.SetActive(false);
        }

	}

	public void AdvanceToNextScreen()
	{
        infoScreenNo++;

        infoScreenNo = Mathf.Clamp(infoScreenNo, 0, infoScreens.Length - 1);

        if (infoScreenNo >= infoScreens.Length - 1)
        {
            nextButton.SetActive(false);
        }

        if (infoScreenNo > 0)
        {
            backButton.SetActive(true);
        }

        if (infoScreenNo < infoScreens.Length)
        {
            infoImage.GetComponent<Image>().sprite = infoScreens[infoScreenNo];
        }
    }

	public void BackToPreviousScreen()
	{
		infoScreenNo--;

        infoScreenNo = Mathf.Clamp(infoScreenNo, 0, infoScreens.Length - 1);

        //if (infoScreenNo < 0)
        //{
        //    SceneManager.LoadScene(1);
        //}

        if (infoScreenNo <= 0)
        {
            backButton.SetActive(false);
        }

		if(infoScreenNo < infoScreens.Length - 1 && infoScreenNo >= 0)
		{
			nextButton.SetActive (true);
			infoImage.GetComponent<Image> ().sprite = infoScreens [infoScreenNo];
		}

	}


	public void AdvanceToSceneDirectly()
	{
		infoImage.SetActive (false);
		backButton.SetActive (false);
		nextButton.SetActive (false);
		startButton.SetActive (false);
	}
}
