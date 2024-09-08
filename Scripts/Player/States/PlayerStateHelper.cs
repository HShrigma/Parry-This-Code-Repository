using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerStateHelper : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    [Header("Hurtboxes")]    
    [SerializeField] GameObject hurtbox;
    [SerializeField] float hurtInvulnerabilitySeconds;
    public bool Invulnerable { get; private set; }
    [Header("Physics")]
    [SerializeField] float baseSpeed = 40f;
    [SerializeField] float idleDrag = 10f;
    [SerializeField] float moveDrag = 5f;
    [SerializeField] ForceMode movementMode;
    [SerializeField] Rigidbody rb;
    public CapsuleCollider CapsuleCollider;
    [Header("Layer Collisions")]
    public int PlayerLayer;
    public int EnemyLayer;
    public int ProjectileLayer;
    float speed;
    [Header("Effects")]
    //Effects
    [SerializeField]CameraShaker shaker;
    GlobalFXManager gFXManager;
    [HideInInspector] public PlayerSFXManager SFXManager;
    void Start()
    {
        Invulnerable = false;
        speed = baseSpeed;
        gFXManager = GameManager.instance.GFXManager;
        SFXManager = GameManager.instance.PlayerSoundFXManager;
    }

    private void FixedUpdate()
    {
        if (PlayerController.instance.state != PlayerController.playerFSMMain.Dead)
        {
            transform.position = new Vector3(rb.position.x, 0f, rb.position.z);
        }
    }
    void Update()
    {
        MoveInput = new Vector2(
           Input.GetAxisRaw("Horizontal"),
           Input.GetAxisRaw("Vertical"));
    }
    public void RotateOnInput()
    {
        if (MoveInput.magnitude != 0 && Time.timeScale == 1f)
        {
            float yRot = 90f * MoveInput.x;
            if (yRot == 0 && MoveInput.y == -1)
            {
                yRot = 180f;
            }
            else if (yRot > 0)
            {
                yRot -= 45f * MoveInput.y;
            }
            else if (yRot < 0)
            {
                yRot += 45f * MoveInput.y;
            }
            rb.transform.rotation = Quaternion.Euler(0f, yRot, 0f);
        }
    }

    public void MoveWithForceMode(ForceMode mode, float multiplier = 1f)
    {
        float moveMag = 1f;
        if (MoveInput.magnitude == Mathf.Sqrt(2))
        {
            moveMag = 0.75f;
        }
        //Vector3 dir = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 dir = transform.forward;
        rb.AddForce(dir * speed * moveMag * multiplier, mode);
    }

    public void setRBDrag(string dragType)
    {
        switch (dragType)
        {
            case "idle":
                rb.drag = idleDrag;
                break;
            case "movement":
                rb.drag = moveDrag;
                break;
        }
    }
    public void setRBDrag(float drag, float angularDrag)
    {
        rb.drag = drag;
        rb.angularDrag = angularDrag;
    }

    public void EnableHurtBox()
    {
        hurtbox.SetActive(true);
    }
    public void DisableHurtBox()
    {
        hurtbox.SetActive(false);
    }
    public void SetDeadRB()
    {
        rb.useGravity = false;
        CapsuleCollider.enabled = false;
    }

    public void SetHurtInvulnerable()
    {
        StartCoroutine(SetInvulnerabilityTime());
    }

    IEnumerator SetInvulnerabilityTime()
    {
        EnableInvulnerable();
        yield return new WaitForSeconds(hurtInvulnerabilitySeconds);
        DisableInvulnerable();
    }

    public void DisableInvulnerable()
    {
        EnableHurtBox();
        Physics.IgnoreLayerCollision(PlayerLayer, EnemyLayer, false);
        Physics.IgnoreLayerCollision(PlayerLayer, ProjectileLayer, false);
        Invulnerable = false;
    }
    public void EnableInvulnerable()
    {
        DisableHurtBox();
        Physics.IgnoreLayerCollision(PlayerLayer, EnemyLayer, true);
        Physics.IgnoreLayerCollision(PlayerLayer, ProjectileLayer, true);
        Invulnerable = true;
    }

    public void InitCameraShake(float amt)
    {
        shaker.ShakeCamera(Vector3.one, amt);
    }
    
    public void InitHitLag(float offsetMod = 1f, float sustainMod = 1f)
    {
        gFXManager.PlayHitLag(offsetMod, sustainMod);
    }
    public void SetPlayerDamageSource()
    {
        int animatorDamageSRC = 0;
        switch (PlayerController.instance.lastDamageSource)
        {
            case "melee":
                animatorDamageSRC = 1;
                break;
            case "ranged":
                animatorDamageSRC = 2;
                break;
            case "explosion":
                animatorDamageSRC = 3;
                break;
        }
        PlayerController.instance.SetAnimatorDamageSource(animatorDamageSRC);
    }
}
