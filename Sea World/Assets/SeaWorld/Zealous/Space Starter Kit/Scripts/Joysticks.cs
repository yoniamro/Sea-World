using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Joysticks : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Image joystickBG;
    public Image stick;

    [Header("Fish Player...")]
    public GameObject fishPlayer;
    public float speed = 5f;
    public float movementSmoothTime = 0.2f;
    public bool enableMovement = false;

    

    private Vector3 input;
    private Vector3 velocity;
    private Vector3 currentVelocity;


    void Update()
    {
        if(enableMovement)
        {
            if(fishPlayer != null)
            {
                velocity = input * speed;
                velocity.z = 0;


                float moveDist = speed * Time.deltaTime;

                RaycastHit hit;

                Ray ray = new Ray(fishPlayer.transform.position, fishPlayer.transform.forward);

                if (!Physics.Raycast(ray, out hit, moveDist + 3f, 1 << 9))
                {

                    fishPlayer.transform.position = Vector3.SmoothDamp(fishPlayer.transform.position, fishPlayer.transform.position + velocity, ref currentVelocity, movementSmoothTime);
                }

                //Debug.DrawRay(ray.origin, ray.direction * (moveDist + 3f), Color.red);

                fishPlayer.transform.LookAt(fishPlayer.transform.position + velocity);
                
            }
        }
    }

  

    public virtual void OnPointerDown(PointerEventData pointer)
    {
        OnDrag(pointer);
    }

    public virtual void OnPointerUp(PointerEventData pointer)
    {
        input = Vector3.zero;
        stick.rectTransform.anchoredPosition = Vector3.zero;
    }

    public virtual void OnDrag(PointerEventData pointer)
    {
        Vector2 pos = Vector2.zero;

        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBG.rectTransform, pointer.position, pointer.pressEventCamera, out pos))
        {
            pos.x = (pos.x / joystickBG.rectTransform.sizeDelta.x);
            pos.y = (pos.y / joystickBG.rectTransform.sizeDelta.y);

            float x = (joystickBG.rectTransform.pivot.x == 1) ? pos.x * 2 + 1 : pos.x * 2 - 1;
            float y = (joystickBG.rectTransform.pivot.y == 1) ? pos.y * 2 + 1 : pos.y * 2 - 1;

            input = new Vector3(x, y, 0);
            input = (input.magnitude > 1.0f) ? input.normalized : input;

            //Debug.Log(input);

            stick.rectTransform.anchoredPosition = new Vector3(input.x * (joystickBG.rectTransform.sizeDelta.x / 3), input.y * (joystickBG.rectTransform.sizeDelta.y / 3));
        }

    }
}


