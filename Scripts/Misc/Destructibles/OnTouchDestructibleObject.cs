using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class OnTouchDestructibleObject : DestructibleObjectTouch
{
    [Header("RigidBody")]
    [SerializeField] Rigidbody rb;
    [Header("Destroyed Components")]
    [SerializeField] MeshRenderer destroyedMesh;
    [SerializeField] Collider destroyedCollider;

    [Header("Physics Modifiers")]
    [SerializeField] float explosionSpeed;
    [SerializeField] float explosionRadius;
    [SerializeField] float torqueCoefficient;
    [SerializeField] float disablePhysicsTime;
    [SerializeField] float groundLength;

    enum states
    {
        enabled,
        hit,
        shouldDisable,
        disabled
    }
    states state = states.enabled;
    private void Awake()
    {
        SetDestroyedComponentsEnabled(false);
    }

    private void FixedUpdate()
    {
        if (state != states.disabled)
        {
            if (state == states.shouldDisable)
            {
                Vector3[] groundDirs = new Vector3[4]
                { transform.right.normalized,
                  (-1f*transform.right).normalized,
                  transform.up.normalized,
                (-1f*-transform.up).normalized
                };

                foreach (Vector3 groundDir in groundDirs)
                {
                    if (Physics.Raycast(transform.position, groundDir, groundLength, groundMask))
                    {
                        DisablePhysics();
                        break;
                    };
                    Debug.DrawRay(transform.position, groundDir * groundLength, Color.red);
                }

            }
        }
    }
    void SetDestroyedComponentsEnabled(bool value)
    {
        destroyedMesh.enabled = value;
        destroyedCollider.enabled = value;
    }
    public override void TriggerDestruction()
    {
        if (state == states.enabled)
        {
            state = states.hit;
            DisableDestructibleComponents();
            SetDestroyedComponentsEnabled(true);
            SetPitchAndPlayAudio();
            rb.AddExplosionForce(explosionSpeed, transform.position, explosionRadius);
            Vector3 randomDir = new Vector3(Random.value, Random.value, Random.value);
            rb.AddTorque(randomDir * (explosionSpeed * torqueCoefficient), ForceMode.VelocityChange);
            StartCoroutine(disablePhysicsAfterTime());
        }

    }

    IEnumerator disablePhysicsAfterTime()
    {
        yield return new WaitForSeconds(disablePhysicsTime);
        state = states.shouldDisable;
    }

    void DisablePhysics()
    {
        rb.isKinematic = true;
        destroyedCollider.enabled = false;
        state = states.disabled;
    }
}
