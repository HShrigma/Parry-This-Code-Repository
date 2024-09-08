using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorTint : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] TextMeshProUGUI TMPro;
    [SerializeField] Color baseColor;
    [SerializeField] Color selectedColor;
    [SerializeField] Color submitColor;
    [SerializeField][ColorUsage(true, true)] Color baseEmmision;
    [SerializeField][ColorUsage(true, true)] Color selectedEmmision;
    [SerializeField][ColorUsage(true, true)] Color submitEmmision;

    Material originalMaterial;
    Material instanceMaterial;
    void Awake()
    {
        CreateMaterialInstance();
    }

    void CreateMaterialInstance()
    {
        // Create a unique instance of the material
        if (img != null && img.material != null)
        {
            originalMaterial = img.material;
            instanceMaterial = new Material(originalMaterial);
            img.material = instanceMaterial;
        }
    }
    void SetTint(Color color, Color emmision)
    {
        if (img != null)
        {
            img.material.SetColor("_BaseColor", color); 
            img.material.SetColor("_EmissionColor", emmision);
            TMPro.color = color;
        }
    }

    public void SetSelectTint()
    {
        SetTint(selectedColor, selectedEmmision);
    }
    public void SetDeselectTint()
    {
        SetTint(baseColor, baseEmmision);
    }
    public void SetSubmitTint()
    {
        SetTint(submitColor, submitEmmision);
    }
    void OnDestroy()
    {
        // clean up the instance material
        if (instanceMaterial != null)
        {
            Destroy(instanceMaterial);
        }
    }
}