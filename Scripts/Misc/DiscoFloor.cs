using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoFloor : MonoBehaviour
{
    int texIndex;
    [SerializeField] Texture2D[] textures;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] float animationTime;
    void Start()
    {
        texIndex = 0;
        StartCoroutine(SetTextureRecursive());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CycleTexIndex()
    {
        texIndex++;
        texIndex %= textures.Length;
    }

    IEnumerator SetTextureRecursive()
    {
        meshRenderer.material.SetTexture("_BaseMap", textures[texIndex]);
        meshRenderer.material.SetTexture("_EmissionMap", textures[texIndex]);
        yield return new WaitForSeconds(animationTime);
        CycleTexIndex();
        StartCoroutine(SetTextureRecursive());
    }
}
