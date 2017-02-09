using UnityEngine;
using System.Collections;

public class SnappedObject : MonoBehaviour
{
    public GameObject refObject;


    [HideInInspector]
    public Vector3 startPos;
    [HideInInspector]
    public Vector3 startScale;

    private Quaternion startRot;

    void Awake()
    {
        refObject.SetActive(true);
        startRot = refObject.transform.localRotation;
        startPos = refObject.transform.localPosition;
        startScale = refObject.transform.localScale;
    }


    public void InitTransform()
    {
        refObject.transform.localPosition = startPos;
        refObject.transform.localScale = startScale;

        float x = startRot.eulerAngles.x;
        float y = startRot.eulerAngles.y;
        float z = startRot.eulerAngles.z;



        refObject.transform.localRotation = Quaternion.Euler(x, y, z);
    }
}
