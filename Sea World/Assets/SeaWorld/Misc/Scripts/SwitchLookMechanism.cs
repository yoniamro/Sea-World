using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class SwitchLookMechanism : MonoBehaviour
{
	public Joystick lookJoystick;
	public GameObject lookCircle;
	public MinimalSensorCamera sensor;

	private bool enabledSensor = false;

	public void SwitchLook()
	{
		if(!enabledSensor)
		{
			sensor.enabled = true;
			lookJoystick.CleanUp ();
			lookJoystick.enabled = false;
			lookJoystick.gameObject.SetActive (false);
			lookCircle.SetActive (false);
			enabledSensor = true;
		}
		else if(enabledSensor)
		{
			sensor.enabled = false;
			lookJoystick.gameObject.SetActive (true);
			lookJoystick.CleanUp ();
			lookJoystick.enabled = true;
			lookCircle.SetActive (true);
			enabledSensor = false;
		}
	}
}
