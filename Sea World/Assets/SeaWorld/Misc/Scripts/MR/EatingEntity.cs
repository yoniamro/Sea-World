using UnityEngine;
using System.Collections;

public class EatingEntity : MonoBehaviour
{
    public enum FoodType { Meat, Nuts, Wheat };

    public FoodType foodType = FoodType.Meat;

    public FoodType GetFoodType()
    {
        return foodType;
    }

    void OnTriggerEnter(Collider c)
    {
        Debug.Log("Eating Now!");

        if(c.CompareTag(foodType.ToString()))
        {
            GetComponent<Animator>().SetTrigger("Eat");
            StartCoroutine(HideFood(c.gameObject));
        }

    }

    IEnumerator HideFood(GameObject obj)
    {
        yield return new WaitForSeconds(1f);
        obj.SetActive(false);
    }
}
