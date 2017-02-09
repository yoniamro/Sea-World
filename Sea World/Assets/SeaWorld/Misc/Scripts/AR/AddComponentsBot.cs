using UnityEngine;
using System.Collections;

public class AddComponentsBot : MonoBehaviour
{
    public GameObject[] trackables;
    public GameObject button;

	public void AddDesiredComponents ()
    {
	    foreach(GameObject obj in trackables)
        {
            if (button.transform.IsChildOf(obj.transform))
            {
                Destroy(button);
            }

            else if(!button.transform.IsChildOf(obj.transform))
            {
                GameObject btn = Instantiate(button) as GameObject;
                btn.transform.parent = obj.transform;
                btn.name = "FlocksOnOff";
                btn.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
                btn.transform.localPosition = new Vector3(0.719f, 0.005f, -0.44f);
            }
        }
	}
	
}
