using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Class used for collecting collision data to avoid double hitting and to decide whether to prioritise parried or hurt state
public class HitboxCollisionInfo
{
    public HitboxCollisionInfo(GameObject _obj, string _tag)
    {
        this.obj = _obj;
        this.name = this.obj.name;
        this.tag = _tag;
    }
    public HitboxCollisionInfo(GameObject _obj, string _tag, Enemy _enemy)
    {
        this.obj = _obj;
        this.name = this.obj.name;
        this.tag = _tag;
        this.enemy = _enemy;
    }
    public string name { get; private set; }
    public string tag { get; private set; }
    public GameObject obj { get; private set; }

    public Enemy enemy { get; private set; }
}
public class PlayerHitbox : MonoBehaviour
{
    Dictionary<GameObject, HitboxCollisionInfo> collisionMap = new Dictionary<GameObject, HitboxCollisionInfo>();
    [SerializeField] Collider coll;
    GlobalFXManager gFXManager;
    void Awake()
    {
        gFXManager = GameManager.instance.GFXManager;
    }

    void OnEnable()
    {
        collisionMap.Clear();
    }

    void FixedUpdate()
    {
        if (collisionMap.Count > 0)
        {
            //process each unique collision
            foreach (var entry in collisionMap)
            {
                HitboxCollisionInfo collisionInfo = entry.Value;
                if (collisionInfo.obj != null)
                {
                    switch (collisionInfo.tag)
                    {
                        case "Enemy Hurtbox":
                            collisionInfo.enemy.TakeDamage();
                            break;
                        case "Enemy Hitbox":
                            collisionInfo.enemy.SetState(Enemy.enemyStates.Parried);
                            PlayerController.instance.SetParryBox();
                            break;
                    }
                }
            }
            //clear map after processing
            collisionMap.Clear();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Enemy Hitbox":
            case "Enemy Hurtbox":
                Vector3 impactPoint = coll.ClosestPoint(other.transform.position);
                Enemy enemy = other.GetComponentInParent<Enemy>();
                if (enemy.ParryVFXManager != null)
                {
                    enemy.ParryVFXManager.GetImpactPoint(impactPoint);
                }

                string tag = other.tag;
                GameObject obj = enemy.gameObject;

                //add or update collision info
                collisionMap[obj] = new HitboxCollisionInfo(obj, tag, enemy);

                gFXManager.PlayHitLag(0.3f, 0.2f);
                break;
            case "Projectile":
                PlayerController.instance.SetParryBox();
                other.GetComponent<Bullet>().Parry();
                gFXManager.PlayHitLag(0.3f, 0.2f);
                break;
            case "Explosive":
                other.GetComponent<Explosive>().HandlePlayerHit();
                break;
            case "Destructible":
                other.GetComponent<DestructibleObject>().TriggerDestruction();
                break;
        }
    }
}