using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObjectTriggerReplace : DestructibleObjectTrigger
{
    [Header("Multiple Colliders")]
    [SerializeField] List<Collider> additionalDestructibleColliders;
    [SerializeField] List<DestructibleObjectTrigger> heldTriggers;
    [Header("ReplacedObject")]
    [SerializeField] GameObject replaced;
    protected override void DoOnDestructionBase()
    {
        base.DoOnDestructionBase();
        additionalDestructibleColliders.ForEach(n => n.enabled = false);
        heldTriggers.ForEach(n => n.TriggerDestruction());
        replaced.SetActive(true);
    }
}
