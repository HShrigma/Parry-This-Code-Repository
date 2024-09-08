using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderColorTint : MonoBehaviour
{
    [SerializeField] Image handleImg;
    [SerializeField] Image fillImg;
    [SerializeField] TextMeshProUGUI TMPro;

    [SerializeField] Color baseTMPColor;
    [SerializeField] Color selectedTMPColor;

    [SerializeField][ColorUsage(true, true)] Color baseHandleEmmision;
    [SerializeField][ColorUsage(true, true)] Color selectedHandleEmmision;
    [SerializeField][ColorUsage(true, true)] Color baseFillEmmision;
    [SerializeField][ColorUsage(true, true)] Color selectedFillEmmision;


    Material originalHandleMaterial;
    Material instanceHandleMaterial;

    Material originalFillMaterial;
    Material instanceFillMaterial;

    void Awake()
    {
        CreateMaterialInstances();
    }

    void CreateMaterialInstances()
    {
        // create a unique instance of the material
        if (handleImg != null && handleImg.material != null)
        {
            originalHandleMaterial = handleImg.material;
            instanceHandleMaterial = new Material(originalHandleMaterial);
            handleImg.material = instanceHandleMaterial;
        }
        if (fillImg != null && fillImg.material != null)
        {
            originalFillMaterial = fillImg.material;
            instanceFillMaterial = new Material(originalFillMaterial);
            fillImg.material = instanceFillMaterial;
        }
    }
    void SetHandleTint(Color emmision)
    {
        if (handleImg != null)
        {
            handleImg.material.SetColor("_EmissionColor", emmision);
        }
    }
    void SetFillTint( Color emmision)
    {
        if (fillImg != null)
        {
            fillImg.material.SetColor("_EmissionColor", emmision);
        }
    }
    void SetTMPColor(Color color)
    {
        TMPro.color = color;
    }
    public void SetSelectTint()
    {
        SetHandleTint(selectedHandleEmmision);
        SetFillTint(selectedFillEmmision);
        SetTMPColor(selectedTMPColor);
    }
    public void SetDeselectTint()
    {
        SetHandleTint(baseHandleEmmision);
        SetFillTint(baseFillEmmision);
        SetTMPColor(baseTMPColor);
    }
    void OnDestroy()
    {
        // clean up the instance materials
        if (instanceFillMaterial != null)
        {
            Destroy(instanceFillMaterial);
        }
        if(instanceHandleMaterial != null)
        {
            Destroy(instanceHandleMaterial);
        }
    }
}
