using UnityEngine;
using System.Collections;

public class ScalingObjects : MonoBehaviour 
{
    public GameObject objectToScale;

    public GameObject scaleUpObject;
    public GameObject scaleDownObject;

    public float scaleFactor;
    //public float minScale;
    //public float maxScale;

    public void ScaleUp()
    {
        if(objectToScale == null)
        {
            return;
        }

        objectToScale.transform.localScale += Vector3.one * scaleFactor;

        float x, y, z;

        x = Mathf.Clamp(objectToScale.transform.localScale.x, 5, Mathf.Infinity);
        y = Mathf.Clamp(objectToScale.transform.localScale.y, 5, Mathf.Infinity);
        z = Mathf.Clamp(objectToScale.transform.localScale.z, 5, Mathf.Infinity);

        objectToScale.transform.localScale = new Vector3(x, y, z);
    }

    public void ScaleDown()
    {
        if (objectToScale == null)
        {
            return;
        }

        objectToScale.transform.localScale -= Vector3.one * scaleFactor;

        float x, y, z;

        x = Mathf.Clamp(objectToScale.transform.localScale.x, 5, Mathf.Infinity);
        y = Mathf.Clamp(objectToScale.transform.localScale.y, 5, Mathf.Infinity);
        z = Mathf.Clamp(objectToScale.transform.localScale.z, 5, Mathf.Infinity);

        objectToScale.transform.localScale = new Vector3(x, y, z);
    }

    	
}
