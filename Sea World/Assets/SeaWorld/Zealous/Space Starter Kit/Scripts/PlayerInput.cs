using UnityEngine;
using System.Collections;
using System;

public class PlayerInput : MonoBehaviour
{

    private Vector3 input;


    private bool movementButtonPressed = false;
    private bool fireButtonPressed = false;
  
    void Start()
    {
       
    }


    void Update()
    {

        if(!movementButtonPressed)
        {
            //Gets input from keyboard.
            input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            //player.SetVelocity(input);
        }

   


        
    }

   

    public void OnButtonDown(string buttonName)
    {

        switch (buttonName)
        {
            //case "RightArrow":
            //    input.x = 1;
            //    movementButtonPressed = true;
            //    break;

            //case "LeftArrow":
            //    input.x = -1;
            //    movementButtonPressed = true;
            //    break;

            case "FireButton":
                fireButtonPressed = true;
                break;

            default:
                fireButtonPressed = false;
                break;
        }

    }


    public void OnButtonUp(string buttonName)
    {
        
       
        switch (buttonName)
        {
            //case "RightArrow":
            //    movementButtonPressed = false;
            //    input.x = 0;
            //    break;

            //case "LeftArrow":
            //    movementButtonPressed = false;
            //    input.x = 0;
            //    break;

            case "FireButton":
                fireButtonPressed = false;
                break;


            default:
                fireButtonPressed = false;
                break;
        }

    }

  
}
