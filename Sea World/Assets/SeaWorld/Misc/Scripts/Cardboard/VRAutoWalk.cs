using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class VRAutoWalk : MonoBehaviour
{
    public float speed = 5.0f;
    public float lookAngle = 35.0f;
    public CharacterController characterController;

    private bool autoWalk = false;
    private Transform cardboardHead;

	void Start ()
    {
        cardboardHead = Camera.main.transform;
    }


    void Update ()
    {
	    if(cardboardHead.eulerAngles.x >= lookAngle && cardboardHead.eulerAngles.x < 90f)
            autoWalk = true;
        else
            autoWalk = false;

        if(autoWalk)
        {
            Vector3 dir = cardboardHead.TransformDirection(Vector3.forward);

            characterController.SimpleMove(dir * speed);
        }
	}
}
