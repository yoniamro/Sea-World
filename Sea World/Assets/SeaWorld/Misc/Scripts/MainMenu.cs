using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	
    [Header("Loading..")]
    public GameObject loadingScreen;

    [Header("Settings..")]
    public GameObject settingsMenu;

    [Space(10)]
    [Header("Volume Control..")]
    public Slider volumeSlider;

    [Header("Counter..")]
    public InputField counterField;
    [Header("Parental Control...")]
    public ParentalControl parentalControl;

    [Header("Language Toggles..")]
    public Toggle[] languageToggles;
    [Space(10)]
    public string[] languageNames;

    void Start()
    {
        if (counterField != null)
            counterField.text = PlayerPrefs.GetInt("COUNTERVAL", 30).ToString();

        if (volumeSlider != null)
            volumeSlider.normalizedValue = PlayerPrefs.GetFloat("BGVOLUME", 0.2f);
    }

    public void VolumeControl()
    {
        PlayerPrefs.SetFloat("BGVOLUME", volumeSlider.normalizedValue);
        PlayerPrefs.Save();
    }

    public void SetCounterValue()
    {
        if (counterField != null)
        {
            if (counterField.text == "")
            {
                PlayerPrefs.SetInt("COUNTERVAL", int.MaxValue);
                PlayerPrefs.SetInt("COUNTERVALRUNTIME", int.MaxValue);
                if (parentalControl != null)
                {
                    parentalControl.timerValue = int.MaxValue;
                }
                PlayerPrefs.Save();
            }
            else
            {
                PlayerPrefs.SetInt("COUNTERVAL", int.Parse(counterField.text));
                PlayerPrefs.SetInt("COUNTERVALRUNTIME", int.Parse(counterField.text) * 60);
                if (parentalControl != null)
                {
                    parentalControl.timerValue = int.Parse(counterField.text) * 60;
                }
                PlayerPrefs.Save();
            }
        }
    }

    public void ChooseLanguage(int index)
    {
        if (languageToggles[index].isOn)
        {
            PlayerPrefs.SetString("Language", languageNames[index]);
            PlayerPrefs.Save();
        }
    }

    public void OnFinishEdit()
    {
        if(counterField != null)
        {
            counterField.text = PlayerPrefs.GetInt("COUNTERVAL", 30).ToString();
        }
    }

    public void LoadSettingsMenu()
    {
        settingsMenu.SetActive(true);
    }

    public void BackFromSettingsMenu()
    {
        settingsMenu.SetActive(false);
    }


    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(1);
    }


    public void ExitGame()
    {
        Application.Quit();
    }

	public void LevelSelection(string levelName)
	{
		if (loadingScreen != null) 
		{
			loadingScreen.SetActive (true);
		}
		SceneManager.LoadScene (levelName);
	}

}
