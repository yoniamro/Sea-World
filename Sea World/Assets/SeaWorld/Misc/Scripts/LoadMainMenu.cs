using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadMainMenu : MonoBehaviour {

	void Start()
    {
        SceneManager.LoadScene(1);
    }
}
