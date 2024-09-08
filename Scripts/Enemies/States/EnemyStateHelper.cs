using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateHelper : MonoBehaviour
{
    public Collider PhysicsCollider;
    public Enemy self;
    [SerializeField] Rigidbody rb;
    PlayerController player;
    [HideInInspector] public EnemySFXManager SFXManager;
    public void SetStats()
    {
        SFXManager = GameManager.instance.EnemySoundFXManager;
        player = PlayerController.instance;
    }

    void Update()
    {
        rb.transform.rotation = transform.rotation;
        rb.position = transform.position;
    }

    public void LookAtPlayerOnYAxis()
    {
        Vector3 lookPosition = new Vector3(
            player.transform.position.x,
            self.transform.position.y,
            player.transform.position.z);
        self.transform.LookAt(lookPosition);
    }
    public void LookAtPlayerOnYAxis(Vector3 offset)
    {
        Vector3 lookDir = GetOffsetDirToPlayer(offset);
        self.transform.rotation = Quaternion.LookRotation(lookDir);
    }
    public void MoveForwardWithMod(float mod = 1f, ForceMode fMode = ForceMode.VelocityChange)
    {
        rb.AddForce(self.transform.forward.normalized * mod, fMode);
    }

    public void DisableCollisions()
    {
        StartCoroutine(WaitForFrameToDisableCollisions());
    }
    //Waits for end of frame as to not interefere
    // with the particle instantiation origin
    IEnumerator WaitForFrameToDisableCollisions()
    {
        yield return new WaitForEndOfFrame();
        rb.useGravity = false;
        PhysicsCollider.enabled = false;
    }

    public Vector3 GetRBPosition()
    {
        return rb.position;
    }

    public Vector3 GetDirToPlayer(Transform t, bool normalized = true)
    {
        Vector3 dir = player.transform.position - t.position;
        if (normalized)
        {
            return dir.normalized;
        }
        return dir;
    }
    public Vector3 GetOffsetDirToPlayer(Vector3 offset)
    {
        Vector3 lookDir = GetDirToPlayer(self.transform, false);
        if (player.transform.position.x > self.transform.position.x)
        {
            offset.z *= -1;
        }
        if (player.transform.position.z < self.transform.position.z)
        {
            offset.x *= -1;
        }
        lookDir.Normalize();
        return new Vector3(lookDir.x + offset.x, 0, lookDir.z + offset.z);
    }

    public static Vector3 GetDiryFromXtoY(Transform x, Transform y, bool normalized = true)
    {
        Vector3 dir = y.position - x.position;
        if (normalized)
        {
            return dir.normalized;
        }
        return dir;
    }
}
