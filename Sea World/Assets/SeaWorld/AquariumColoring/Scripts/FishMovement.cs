using UnityEngine;
using System.Collections;

public class FishMovement : MonoBehaviour
{
    public enum MovementType { Random, Loop };

    public float moveSpeed = 4f;
    public MovementType movementType = MovementType.Random;
    [HideInInspector]
    public AquariumBigMarkerBehavior aquariumBehavior;


    private bool rotated = false;
    private GameObject[] passPoints;
    private Vector3 moveDir;
    private Vector3 targetPos;
    private int index;


    private float minX = -47f;
    private float maxX = -16f;

    private float minY = 4f;
    private float maxY = 15f;

    private float minZ = -9f;
    private float maxZ =  8.3f;

    private float msBeforeChangingTargetPos;

    void Start()
    {

        if (movementType == MovementType.Loop)
        {
            targetPos = passPoints[index].transform.position;
            index++;
            index = index % passPoints.Length;
        }
        else if (movementType == MovementType.Random)
        {
            //targetPos = passPoints[Random.Range(0, passPoints.Length)].transform.position;
            targetPos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
        }

        moveDir = targetPos - transform.position;
    }

    void Update()
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        float moveDist = moveSpeed * Time.deltaTime;

        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        //if (Physics.Raycast(ray, out hit, moveDist + 6f, 1 << 9))
        //{
        //    if (!rotated)
        //    {
        //        RotateFish();
        //        Invoke("EnableRotationAgain", 1.5f);
        //    }
        //}

        //transform.Translate(Vector3.forward * moveDist, Space.Self);


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //float moveDist = moveSpeed * Time.deltaTime;

        if(!aquariumBehavior.eatingStateEnabled)
        {
            if (transform.position == targetPos || Physics.Raycast(ray, out hit, moveDist + 6f, 1 << 9))
            {

                if(movementType == MovementType.Loop)
                {
                    targetPos = passPoints[index].transform.position;
                    index++;
                    index = index % passPoints.Length;
                }
                else if(movementType == MovementType.Random)
                {
                    msBeforeChangingTargetPos = Time.time + 0.25f;

                    //targetPos = passPoints[Random.Range(0, passPoints.Length)].transform.position;
                    targetPos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
                }
            

                moveDir = targetPos - transform.position;
            }
            //else if(Physics.Raycast(ray, out hit, moveDist + 6f, 1 << 10))
            //{
            //    if(Time.time > msBeforeChangingTargetPos)
            //    {
            //        msBeforeChangingTargetPos = Time.time + 0.25f;

            //        targetPos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
            
            //        moveDir = targetPos - transform.position;
            //    }

            //}
            else if (Physics.Raycast(ray, out hit, moveDist + 6f, 1 << 11))
            {
                targetPos = Random.insideUnitSphere;

                moveDir = targetPos - transform.position;
            }
        }
        else if(aquariumBehavior.eatingStateEnabled)
        {
            Vector3 foodPosToGo = aquariumBehavior.foodPoints[0].transform.position;
            float temp0 = Vector3.Distance(transform.position, aquariumBehavior.foodPoints[0].transform.position);


            for (int i = 0; i < aquariumBehavior.foodPoints.Length; i++)
            {
                float temp1 = Vector3.Distance(transform.position, aquariumBehavior.foodPoints[i].transform.position);

                if(temp1 < temp0)
                {
                    temp0 = temp1;
                    foodPosToGo = aquariumBehavior.foodPoints[i].transform.position;
                }

            }

            targetPos = new Vector3(foodPosToGo.x, foodPosToGo.y - 3.5f, foodPosToGo.z);

            //if(Physics.Raycast(ray, out hit, moveDist + 6f, 1 << 10))
            //{
            //    targetPos = targetPos + Random.insideUnitSphere * 2;
            //}

            //if(Vector3.Distance(transform.position, targetPos) < 1f)
            //{
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().position = targetPos; // + Random.insideUnitSphere * 2;
            //}

            moveDir = foodPosToGo - transform.position;

        }


            //RaycastHit hit;
            //Ray ray = new Ray(transform.position, transform.forward);

            //if (Physics.Raycast(ray, out hit, moveDist + 6f, 1 << 9))
            //{
            //    if (!rotated)
            //    {
            //        RotateFish();
            //        Invoke("EnableRotationAgain", 1.5f);
            //    }
            //}



        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), 4 * Time.deltaTime);
        //transform.LookAt(targetPos);

   
        //transform.Translate(moveDir * moveDist, Space.Self);


    }

    public void SetPassPoints(GameObject[] _passPoints)
    {
        passPoints = new GameObject[_passPoints.Length];

        for(int i = 0; i < _passPoints.Length; i++)
        {
            passPoints[i] = _passPoints[i];
        }
    }

    void RotateFish()
    {
        rotated = true;
        transform.Rotate(transform.up * Random.Range(180f, 200f));
    }

    void EnableRotationAgain()
    {
        rotated = false;
    }
}
