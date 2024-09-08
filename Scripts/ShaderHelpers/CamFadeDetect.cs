using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetInfo
{
    public GameObject Obj;
    public Collider Coll;
    public TargetInfo(GameObject _obj)
    {
        Obj = _obj;
        Coll = _obj.GetComponent<Collider>();
    }
}

public class CamFadeDetect : MonoBehaviour
{
    public static CamFadeDetect instance;

    RaycastHit[] hits;
    [SerializeField] int maxRaycastHits = 20;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        hits = new RaycastHit[maxRaycastHits];
    }
    List<TargetInfo> targets = new List<TargetInfo>();
    List<TargetInfo> projectileTargets = new List<TargetInfo>();
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] float detectInterval;
    [SerializeField] float projectileDetectRange;
    [SerializeField] List<DitheredTerrainFader> sceneFaders;
    void Start()
    {
        //targets[0] is always player
        targets.Add(new TargetInfo(PlayerController.instance.gameObject));
        InvokeRepeating("DetectAndProcessFaders", 0, detectInterval);
    }
    void DetectAndProcessFaders()
    {
        targets.RemoveAll(n => n.Obj == null);
        projectileTargets.RemoveAll(n => n.Obj == null);

        HashSet<DitheredTerrainFader> tempFaders = CollectAllObscuringFaders(targets);
        tempFaders.UnionWith(CollectAllObscuringFaders(projectileTargets, true));
        ProcessFaders(tempFaders);
    }
    HashSet<DitheredTerrainFader> CollectAllObscuringFaders(List<TargetInfo> _targets, bool projectile = false)
    {
        HashSet<DitheredTerrainFader> tempFaders = new HashSet<DitheredTerrainFader>();
        if (projectile)
        {
            _targets = projectileTargets.Where(n => Vector3.Distance(targets[0].Obj.transform.position, n.Coll.transform.position) < projectileDetectRange).ToList();
        }
        foreach (var target in _targets)
        {
            if (target.Coll != null)
            {
                Vector3 castPos = target.Coll.transform.position;
                castPos.y = 0f;
                Vector3 castDir = (castPos - transform.position).normalized;
                float dist = Vector3.Distance(transform.position, castPos);
                int hitCount = Physics.RaycastNonAlloc(transform.position, castDir, hits, dist);
                for (int i = 0; i < hitCount; i++)
                {
                    DitheredTerrainFader fader = hits[i].collider.GetComponent<DitheredTerrainFader>();
                    if (fader != null)
                    {
                        tempFaders.Add(fader);
                    }
                }
            }
        }

        return tempFaders;
    }
    void ProcessFaders(HashSet<DitheredTerrainFader> temp)
    {
        sceneFaders.RemoveAll(n => n == null);
        foreach (var fader in sceneFaders)
        {
            if (fader != null)
            {
                if (temp.Contains(fader))
                {
                    fader.Obscuring = true;
                    if(fader.State == DitheredTerrainFader.faderState.opaque)
                    {
                        fader.Fade();
                    }
                }
                else
                {
                    fader.Obscuring = false;
                    if (fader.State == DitheredTerrainFader.faderState.faded)
                    {
                        fader.Unfade();
                    }
                }
            }
        }
    }

    public void AddEnemyTargets()
    {
        foreach (var enemy in enemyManager.Enemies)
        {
            if (!targets.Any(t => t.Obj == enemy))
            {
                targets.Add(new TargetInfo(enemy));
            }
        }
    }

    public void AddProjectile(GameObject projectile)
    {
        projectileTargets.Add(new TargetInfo(projectile));
    }
}
