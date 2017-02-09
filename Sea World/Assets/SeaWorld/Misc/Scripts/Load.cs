using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Load : MonoBehaviour 
{

	//public	Text mytext;
    //private string nextScene = "MainMenu";
	private bool obbisok = false;
	private bool loading = false;
	private bool replacefiles = false; //true if you wish to over copy each time

    private bool finishedSettingTimer = false;

	private string[] paths = 
	{	
        "QCAR/AquariumColoring.dat",
        "QCAR/AquariumColoring.xml",
        "QCAR/SeaWorld.dat",
		"QCAR/SeaWorld.xml",
	};

    void Start()
    {

        PlayerPrefs.SetInt("COUNTERVALRUNTIME", PlayerPrefs.GetInt("COUNTERVAL", 30) * 60);
        PlayerPrefs.Save();
        //Debug.Log("Split Scene Timer: " + PlayerPrefs.GetInt("COUNTERVAL", 30) * 60);
        finishedSettingTimer = true;
    }

    void Update()
	{
		if (Application.platform == RuntimePlatform.Android)	
		{
			if (Application.dataPath.Contains(".obb") && !obbisok)
			{
				StartCoroutine(CheckSetUp());
				obbisok = true;
			}
            else
            {
                StartApp();
            }
		} 
		else 
		{
			if (!loading) 
			{
                if (finishedSettingTimer)
                {
                    StartApp();
                }
			}
		}
	}

	public void StartApp() 
	{
		loading = true;
		SceneManager.LoadScene (1);
	}

	public IEnumerator CheckSetUp() 
	{
		//Check and install!
		for(int i = 0; i < paths.Length; ++i) 
		{
			yield return StartCoroutine(PullStreamingAssetFromObb(paths[i]));
		}

		yield return new WaitForSeconds(1f); 

        if(finishedSettingTimer)
        {
		    StartApp();
        }
	}

	//Alternatively with movie files these could be extracted on demand and destroyed or written over
	//saving device storage space, but creating a small wait time.

	public IEnumerator PullStreamingAssetFromObb(string sapath) 
	{ 

		if (!File.Exists(Application.persistentDataPath+sapath)||replacefiles) 
		{
			WWW unpackerWWW = new WWW(Application.streamingAssetsPath + "/" + sapath);
			while (!unpackerWWW.isDone) 
			{
				yield return null;
			}

			if (!string.IsNullOrEmpty(unpackerWWW.error)) 
			{
				Debug.Log("Error unpacking:" + unpackerWWW.error + " path: "+unpackerWWW.url);
				
				yield break; //skip it
			}
			else
			{
				Debug.Log ("Extracting " + sapath + " to Persistant Data");
				
				if(!Directory.Exists(Path.GetDirectoryName(Application.persistentDataPath+"/"+sapath))) {
					Directory.CreateDirectory(Path.GetDirectoryName(Application.persistentDataPath+"/"+sapath));
				}
				File.WriteAllBytes(Application.persistentDataPath+"/"+sapath,unpackerWWW.bytes);
				//could add to some kind of uninstall list?
			}
		}

		yield return 0;
	}

	/*public void tetload ()
	{
		//string uriOfDatasetInObb = Application.streamingAssetsPath + "/QCAR/second.xml"; // do the same same for .dat file too
		string uriOfDatasetInObb =  "/storage/sdcard0/Android/obb/com.m.m/split.main.obb/QCAR/second.xml"; // do the same same for .dat file too
		
		Debug.Log(uriOfDatasetInObb);
		var www = new WWW(uriOfDatasetInObb); 
		//mytext.text = "it's  " + uriOfDatasetInObb;
		//string uriOfDatasetInObb1 = Application.streamingAssetsPath + "/QCAR/second.dat"; // do the same same for .dat file too
		string uriOfDatasetInObb1 = "/storage/sdcard0/Android/obb/com.m.m/split.main.obb/QCAR/second.dat"; // do the same same for .dat file too
		mytext.text = "Load is Done1  " + uriOfDatasetInObb;
	
		var www1 = new WWW(uriOfDatasetInObb1); 
		File.WriteAllBytes(Application.persistentDataPath + "/QCAR/second.xml", www .bytes ); 
		mytext.text = "Load is Done2  " + Application.persistentDataPath + "/QCAR/second.xml";
		File.WriteAllBytes(Application.persistentDataPath + "/QCAR/second.dat", www1 .bytes ); 
		// E:\amr\unity5.1 projects\newspleter\Assets\StreamingAssets\QCAR
		//	File.WriteAllBytes("E:/amr/unity5.1 projects/newspleter/Assets/StreamingAssets/QCAR", www .bytes ); 
		mytext.text = "Load is Done  " + uriOfDatasetInObb;
		//Application.LoadLevel(nextScene);
	}*/
}
