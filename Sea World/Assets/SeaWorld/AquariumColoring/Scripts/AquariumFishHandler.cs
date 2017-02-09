using UnityEngine;
using System.Collections;

public class AquariumFishHandler : MonoBehaviour
{

    public GameObject fishPrefab;
    public AquariumBigMarkerBehavior aquariumBehavior;
    public GameObject[] passPoints;


    public void InstantiatePlayerFishInAquarium()
    {

        GameObject fish = (GameObject)Instantiate(fishPrefab, fishPrefab.transform.position, fishPrefab.transform.rotation);

        fish.transform.parent = aquariumBehavior.fishHolder.transform;
        fish.transform.localPosition = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.08f, 0.3f), Random.Range(-0.18f, 0.15f));
        
        while (Vector3.Distance(fish.transform.localPosition, aquariumBehavior.forbiddenArea.transform.localPosition) < 0.2f)
        {
            fish.transform.localPosition = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.08f, 0.3f), Random.Range(-0.18f, 0.15f));
        }

        fish.transform.localScale = new Vector3(2, 2, 2);

        Joysticks joysticks = GameObject.Find("Joysticks").transform.GetChild(0).GetComponent<Joysticks>();
        joysticks.gameObject.SetActive(true);
        joysticks.fishPlayer = fish;
        joysticks.enableMovement = true;

    }

    public void InstantiateFishInAquarium()
    {

        GameObject fish = (GameObject)Instantiate(fishPrefab, fishPrefab.transform.position, fishPrefab.transform.rotation);
           
        fish.transform.parent = aquariumBehavior.fishHolder.transform;
        fish.transform.localPosition = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.08f, 0.3f), Random.Range(-0.18f, 0.15f));

        while (Vector3.Distance(fish.transform.localPosition, aquariumBehavior.forbiddenArea.transform.localPosition) < 0.2f)
        {
            fish.transform.localPosition = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.08f, 0.3f), Random.Range(-0.18f, 0.15f));
        }

        fish.transform.localScale = new Vector3(1f, 1f, 1f);
        fish.transform.Rotate(fish.transform.up * Random.Range(20f, 200f));

           
            
        FishMovement fishMovement = fish.AddComponent<FishMovement>();
        fishMovement.aquariumBehavior = aquariumBehavior;
        fishMovement.SetPassPoints (passPoints);
        //fishMovement.movementType = (FishMovement.MovementType)Random.Range(0, 2);
        
    }

    public void InstantiateRCFishInAquarium()
    {

        RenderTextureCamera renderTextureCamera = fishPrefab.transform.parent.GetComponent<ColoringTrackingHandler>().RC.GetComponent<RenderTextureCamera>();

        GameObject fish = (GameObject)Instantiate(fishPrefab, fishPrefab.transform.position, fishPrefab.transform.rotation);

        fish.transform.parent = aquariumBehavior.fishHolder.transform;
        fish.transform.localPosition = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.08f, 0.3f), Random.Range(-0.18f, 0.15f));

        while (Vector3.Distance(fish.transform.localPosition, aquariumBehavior.forbiddenArea.transform.localPosition) < 0.2f)
        {
            fish.transform.localPosition = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.08f, 0.3f), Random.Range(-0.18f, 0.15f));
        }


        fish.transform.localScale = new Vector3(1f, 1f, 1f);
        fish.transform.Rotate(fish.transform.up * Random.Range(20f, 200f));

        //Texture2D tex2D = new Texture2D(renderTextureCamera.GetRenderTexture().width, renderTextureCamera.GetRenderTexture().height);
        //tex2D.ReadPixels(RenderTextureCamera.ARCamera_Camera.rect, 0, 0);
        //tex2D.Apply();

        fish.GetComponentInChildren<GetTexture>().enabled = false;

        Texture2D FrameTexture = new Texture2D(renderTextureCamera.GetRenderTexture().width, renderTextureCamera.GetRenderTexture().height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTextureCamera.GetRenderTexture();
        FrameTexture.ReadPixels(new Rect(0, 0, renderTextureCamera.GetRenderTexture().width, renderTextureCamera.GetRenderTexture().height), 0, 0);
        RenderTexture.active = null;

        FrameTexture.Apply();

        Material newMat = Resources.Load("FishMat", typeof(Material)) as Material;
        newMat.EnableKeyword("_MainTex");
        newMat.SetTexture("_MainTex", FrameTexture);

         

        fish.GetComponentInChildren<SkinnedMeshRenderer>().material = newMat;

        //GetComponentInChildren<SkinnedMeshRenderer>().material.SetTexture("_MainTex", FrameTexture);

        FishMovement fishMovement = fish.AddComponent<FishMovement>();
        fishMovement.aquariumBehavior = aquariumBehavior;
        fishMovement.SetPassPoints(passPoints);
        //fishMovement.movementType = (FishMovement.MovementType)Random.Range(0, 2);

    }
}
