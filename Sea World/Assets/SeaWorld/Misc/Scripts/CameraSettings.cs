using UnityEngine;
using System.Collections;

namespace Vuforia
{
    public class CameraSettings : MonoBehaviour 
    {
        private float ReFocusTime;
        private bool FirstRefocus;
	    
        void Start ()
        {
		    //bool focusModeSet = 
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
	    }
	
	    void Update () 
        {
            ReFocusTime += Time.deltaTime;

            if (ReFocusTime > 20)
            {
                //bool focusModeSet = 
                CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
                ReFocusTime = 0;
            }

            if (ReFocusTime > 1 && !FirstRefocus)
            {
                FirstRefocus = true;
                //bool focusModeSet = 
                CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
                ReFocusTime = 0;
            }
	    }
    }
}
