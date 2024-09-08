using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class DestructibleObjectTouch : DestructibleObject
{

    [Header("Collision Layers & Tags")]
    [SerializeField] LayerMask collisionMask;
    [SerializeField] protected LayerMask groundMask;
    [SerializeField] List<string> allowedTags;
    private void OnCollisionEnter(Collision collision)
    {
        HandleCollisionEnter(collision);
    }

    public override void TriggerDestruction()
    {

    }

    protected virtual void HandleCollisionEnter(Collision collision)
    {
        if ((collisionMask & (1 << collision.gameObject.layer)) != 0 &&
            allowedTags.Any(aTag => collision.gameObject.CompareTag(aTag)))
        {
            TriggerDestruction();
        }
    }

}
