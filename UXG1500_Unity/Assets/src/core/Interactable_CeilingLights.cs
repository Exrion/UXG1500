using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering.Universal;

class Interactable_CeilingLights : IInteractable
{
    public Material lightmaterial;
    public Material nolightmaterial;
    
    public bool lightson = false;

    public GameObject darkmodeslides;
    public GameObject brightmodeslides;

    public GameObject[] Lights;
    public GameObject LightComponent;



    protected override void Start()
    {
        base.Start();
        darkmodeslides.SetActive(true);
        Lights = GameObject.FindGameObjectsWithTag("CeilingLight");

    }
    public override void OnInteracted()
    {
        Debug.Log("Interaction Performed");

        if (lightson == true)
        {
            TurnOffLights();
        }
        else { TurnOnLights(); }
    }

    void TurnOnLights()
    {
        //foreach (GameObject i in Lights)
        //{
        //    i.GetComponent<MeshRenderer>().materials[1] = lightmaterial;
        //}
        for (int i = 0; i < Lights.Length; i++)
        {
            Material[] mats = new Material[2];
            if (Lights[i].TryGetComponent(out MeshRenderer meshRen))
            {
                mats = meshRen.materials;
                mats[1] = lightmaterial;
                meshRen.materials = mats;   
            }
        }
        lightson = true;
        darkmodeslides.SetActive(false);
        brightmodeslides.SetActive(true);
        LightComponent.SetActive(true);
    }

    void TurnOffLights()
    {
        //foreach (GameObject i in Lights)
        //{
        //    i.GetComponent<MeshRenderer>().materials[1] = nolightmaterial;
        //}
        for (int i = 0; i < Lights.Length; i++)
        {
            Material[] mats = new Material[2];
            if (Lights[i].TryGetComponent(out MeshRenderer meshRen))
            {
                mats = meshRen.materials;
                mats[1] = nolightmaterial;
                meshRen.materials = mats;
            }
        }
        lightson = false;
        darkmodeslides.SetActive(true);
        brightmodeslides.SetActive(false);
        LightComponent.SetActive(false);
    }
}
