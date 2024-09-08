using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DestructibleObject : MonoBehaviour
{
    [Header("Destructible Components")]
    [SerializeField] MeshRenderer destructibleMesh;
    [SerializeField] Collider destructibleCollider;
    [Header("Audio")]
    [SerializeField] AudioSource audioSrc;
    [SerializeField] Vector2 audioPitchLimits;


    protected virtual void DisableDestructibleComponents()
    {
        destructibleCollider.enabled = false;
        destructibleMesh.enabled = false;
    }

    public abstract void TriggerDestruction();
    protected void SetPitchAndPlayAudio()
    {
        audioSrc.pitch = Random.Range(audioPitchLimits.x, audioPitchLimits.y);
        audioSrc.Play();
    }
}
