using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Vuforia;

public class GroundCardScenario : MonoBehaviour, ITrackableEventHandler
{
    public LayerMask coloringMask;
    //[Space(10)]
    //public GameObject[] animals;

    private bool startColoring = false;
    
    private Color _color;
    private Color _originalColor;

    private Renderer animalRenderer;

    private GameManager gameManager;
    private GameManager_AR gameManagerAR;
    private TrackableBehaviour currentTrackedAnimal;
    private TrackableBehaviour mTrackableBehaviour;
 
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();

        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }

        if(SceneManager.GetActiveScene().name == "AR_Cardboard")
        {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }
        else if (SceneManager.GetActiveScene().name == "AR_Scene")
        {
            gameManagerAR = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager_AR>();
        }


    }

    void Update()
    {
        if (startColoring)
        {
#if UNITY_EDITOR


            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, coloringMask))
                {
                    if (hit.collider.GetComponent<ColorProperties>() != null)
                    {
                        _color = hit.collider.GetComponent<ColorProperties>().GetColor();
                        hit.collider.GetComponent<ColorProperties>().PlayColorName();

                        if (SceneManager.GetActiveScene().name == "AR_Cardboard")
                        {
                            if (gameManager != null)
                            {
                                if (gameManager.currentTrackedPlayAudioTarget != null)
                                {
                                    currentTrackedAnimal = gameManager.currentTrackedPlayAudioTarget;

                                    if (currentTrackedAnimal.GetComponentInChildren<Renderer>() != null)
                                    {
                                        currentTrackedAnimal.GetComponentInChildren<Renderer>().sharedMaterial.color = _color;
                                    }
                                }
                            }
                        }
                        else if (SceneManager.GetActiveScene().name == "AR_Scene")
                        {
                            if (gameManagerAR != null)
                            {
                                if (gameManagerAR.currentTrackedPlayAudioTarget != null)
                                {
                                    currentTrackedAnimal = gameManagerAR.currentTrackedPlayAudioTarget;

                                    if (currentTrackedAnimal.GetComponentInChildren<Renderer>() != null)
                                    {
                                        currentTrackedAnimal.GetComponentInChildren<Renderer>().sharedMaterial.color = _color;
                                    }
                                }
                            }
                        }
                    }
                }
            }
#elif !UNITY_EDITOR

			foreach(Touch t in Input.touches)
			{
				if(t.phase == TouchPhase.Began)
				{
					RaycastHit hit;
					Ray ray = Camera.main.ScreenPointToRay (t.position);

					if(Physics.Raycast(ray, out hit, Mathf.Infinity, coloringMask))
					{
						if(hit.collider.GetComponent<ColorProperties>() != null)
						{
							_color = hit.collider.GetComponent<ColorProperties> ().GetColor ();
						    hit.collider.GetComponent<ColorProperties>().PlayColorName();
						
						    if(SceneManager.GetActiveScene().name == "AR_Cardboard")
                            {
                                if(gameManager != null)  
                                {    
                                    if (gameManager.currentTrackedPlayAudioTarget != null)
                                    {
                                        currentTrackedAnimal = gameManager.currentTrackedPlayAudioTarget;

                                        if (currentTrackedAnimal.GetComponentInChildren<Renderer>() != null)
                                        {
                                            currentTrackedAnimal.GetComponentInChildren<Renderer>().sharedMaterial.color = _color;
                                        }
                                    }
                                }
                            }
                            else if(SceneManager.GetActiveScene().name == "AR_Scene")
                            {
                                if(gameManagerAR != null)  
                                { 
                                    if (gameManagerAR.currentTrackedPlayAudioTarget != null)
                                    {
                                        currentTrackedAnimal = gameManagerAR.currentTrackedPlayAudioTarget;

                                        if (currentTrackedAnimal.GetComponentInChildren<Renderer>() != null)
                                        {
                                            currentTrackedAnimal.GetComponentInChildren<Renderer>().sharedMaterial.color = _color;
                                        }
                                    }
                                }
                            }
						
						}
					}
				}
			}
#endif

        }
    }

    public void OnTrackableStateChanged(
                                      TrackableBehaviour.Status previousStatus,
                                      TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            startColoring = true;
        }
        else
        {
            startColoring = false;
        }
    }

    public void SetColorForCardboard(Color color)
    {
        _color = color;

        if(gameManager != null)
        {
            if (gameManager.currentTrackedPlayAudioTarget != null)
            {
                currentTrackedAnimal = gameManager.currentTrackedPlayAudioTarget;

                if (currentTrackedAnimal.GetComponentInChildren<Renderer>() != null)
                {
                    currentTrackedAnimal.GetComponentInChildren<Renderer>().sharedMaterial.color = _color;
                }
            }
        }
    }
}
