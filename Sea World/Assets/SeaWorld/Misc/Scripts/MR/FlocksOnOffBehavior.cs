using UnityEngine;
using System.Collections;

public class FlocksOnOffBehavior : MonoBehaviour 
{
    public GameObject flocks;
    

    public void TurnOnOff()
    {
        if(flocks.activeSelf)
        {
            flocks.SetActive(false);
        }
        else
        {
            flocks.SetActive(true);
        }
    }
	
}
