using UnityEngine;
using System.Collections;

public class TouchControls : MonoBehaviour 
{
	public enum ObjectType { AR_Object, ReferenceObject };

	public ObjectType type = ObjectType.AR_Object;

	[Space(20)]
    public float rotationSpeed = 40f;
    public float scaleFactor = 0.5f;
    public float minScale = 0.02f;
    public float maxScale = 0.2f;
    public Animator anim;
    public LayerMask touchMask;

    [HideInInspector]
    public bool objectVisible = false;

    private Vector3 startScale;
    private Vector3 startPos;
    private Quaternion startRot;
	private Plane plane;
	private GameObject selectedObject;
	private bool scaling = false;
    private bool draging = false;

	private SnappedObject snappedObject;

	private Color _originalColor;


    //private NavMeshAgent agent;

    void Start()
    {
        objectVisible = false;

		if(GetComponent<SnappedObject>())
			snappedObject = GetComponent<SnappedObject> ();
       
		if (type == ObjectType.ReferenceObject)
			plane = new Plane (-Camera.main.transform.forward, GetComponent<Collider> ().bounds.center);
		//else if (type == ObjectType.AR_Object)
            //plane = new Plane(-Camera.main.transform.forward, Vector3.zero);

        startScale = transform.localScale;
        //startRot = transform.rotation;
        startPos = transform.localPosition;

        //if (gameObject.name != "monkey")
        //{
        //    _originalColor = GetComponentInChildren<Renderer> ().sharedMaterial.color;
        //}

        //CHANGES: ADDED FOR NAVMESH
        //if(GetComponent<NavMeshAgent>() != null)
        //{
        //    agent = GetComponent<NavMeshAgent>();
        //    agent.enabled = false;
        //}
    
    }
    
  
    void Update () 
    {
		if(objectVisible || type == ObjectType.ReferenceObject && snappedObject.refObject.activeSelf)
        {
            if(Input.touchCount > 0)
            {
				foreach(Touch t in Input.touches)
				{
					Ray ray = Camera.main.ScreenPointToRay(t.position);
					RaycastHit hit;  

					if(t.phase == TouchPhase.Began)
					{
						if (Physics.Raycast (ray, out hit, touchMask))
						{
							selectedObject = hit.transform.gameObject;

                            //Walk Animation
                            //if (t.tapCount == 2)
                            //{
                            //    //CHANGES: ADDED FOR NAVMESH
                            //    //agent.enabled = true;


                            //    //float dist;
                            //    //if (plane.Raycast(ray, out dist))
                            //    //{
                            //    //    agent.SetDestination(ray.GetPoint(dist));
                            //    //}
                            //    ///////////////////////////////

                            //    if (selectedObject.GetComponent<Animator>() != null)
                            //        selectedObject.GetComponent<Animator>().SetTrigger("Walk");
                            //}
                            
                            //anim.SetTrigger("Walk");
                            draging = true;
						}
                        //else
                        //    selectedObject = null;
                        else
                            draging = false;
						
					}
					else if(t.phase == TouchPhase.Moved)
					{
						//Drag
						float dist = 0;

                        if (type == ObjectType.ReferenceObject)
                        {    
                            plane = new Plane(-Camera.main.transform.forward, GetComponent<Collider>().bounds.center);
		
					        if (plane.Raycast (ray, out dist))
					        {
						        if (selectedObject != null && !scaling && draging)
						        {
								    selectedObject.transform.position = ray.GetPoint (dist);
								    selectedObject.transform.localPosition = new Vector3(selectedObject.transform.localPosition.x, 
																						 selectedObject.GetComponent<SnappedObject>().startPos.y,
																			  			 selectedObject.transform.localPosition.z);
						       }
						    }
                            
						}

						//Rotation
						if(selectedObject != null && Input.touchCount == 1 && !draging)
						{
							float distX = - t.deltaPosition.x * rotationSpeed;
                            float distY = - t.deltaPosition.y * rotationSpeed;


							if (type == ObjectType.ReferenceObject)
                            {
                                selectedObject.transform.Rotate(Vector3.up * Time.deltaTime * distX);
                                selectedObject.transform.Rotate(Vector3.right * Time.deltaTime * distY);
                            }
							
							else if(type == ObjectType.AR_Object)
                                selectedObject.transform.Rotate(transform.up * Time.deltaTime * distX);
						}	

						//Scale
                        if (Input.touchCount == 2 && selectedObject != null)
						{
                            if (selectedObject.name == this.gameObject.name)
                            {
                                scaling = true;

                                Touch touchZero = Input.GetTouch(0);
                                Touch touchOne = Input.GetTouch(1);

                                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                                float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;

                                transform.localScale += Vector3.one * deltaMagnitudeDiff * scaleFactor * Time.deltaTime;
                                
                                float x, y, z;

                                x = Mathf.Clamp(transform.localScale.x, minScale, maxScale);
                                y = Mathf.Clamp(transform.localScale.y, minScale, maxScale);
                                z = Mathf.Clamp(transform.localScale.z, minScale, maxScale);

                                transform.localScale = new Vector3(x, y, z);
                            }
						} 
						else
							scaling = false;

					}
                    //else if(t.phase == TouchPhase.Ended)
                    //    selectedObject = null;

				}

            }
            
        }
        else
        {
            //CHANGES: ADDED FOR NAVMESH
            //agent.enabled = false;
            ////////////////////

			transform.localScale = startScale;

            if (gameObject.name == "Squid")
            {
			    transform.localRotation = Quaternion.Euler(0, 0f, 0);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 180f, 0);
            }

            transform.localPosition = startPos;

            //if (gameObject.name != "monkey")
            //{
            //    GetComponentInChildren<Renderer>().sharedMaterial.color = _originalColor;
            //}
        }
	}
}
