using UnityEngine;
using System.Collections;

public class FoodTrigger : MonoBehaviour
{

    EatingEntity.FoodType type;

    void OnTriggerEnter(Collider c)
    {
        Debug.Log("Entered Trigger! Eating Now!");
        if(c.gameObject.GetComponent<EatingEntity>())
        {
            type = c.gameObject.GetComponent<EatingEntity>().GetFoodType();
        }

        if(type.ToString() == gameObject.name)
        {
            c.gameObject.GetComponent<Animator>().SetTrigger("Eat");

            gameObject.SetActive(false);
        }
    }
}
