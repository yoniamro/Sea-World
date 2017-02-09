using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BackButton : MonoBehaviour 
{
	void Start ()
    {
        GetComponent<Cardboard>().OnBackButton += () => { StartCoroutine(LoadMainMenu()); };
	}

    IEnumerator LoadMainMenu()
    {
        if(GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().Play();
        }

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
    }
}
