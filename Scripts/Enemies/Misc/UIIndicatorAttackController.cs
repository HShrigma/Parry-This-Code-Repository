using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIndicatorAttackController : MonoBehaviour
{
    enum IndicatorStates
    {
        disabled,
        enabled,
        animating
    }
    IndicatorStates state = IndicatorStates.disabled;
    [Header("Graphics")]
    [SerializeField] Sprite meleeSprite;
    [SerializeField] Sprite rangedSprite;
    [Header("Colors")]
    List<Color> colors;
    [Header("Animation")]
    [SerializeField] float targetMaxScale = 2f;
    [SerializeField] float targetMinScale = 0f;
    [SerializeField] float animationStepLength = 0.1f;
    [SerializeField] float animationStepAmount = 0.1f;
    [SerializeField] ParticleSystem meleeParticles;
    [SerializeField] ParticleSystem rangedParticles;
    //Reference to self to receive state
    [SerializeField] EnemyStateHelper helper;
    Enemy self;
    [SerializeField] Image img;
    void Start()
    {
        self = helper.self;
        SetColor();
    }

    void Update()
    {
        SetState();
        ActOnState();
    }
    #region Image Type and Visibility
    bool StateIsAttacking()
    {
        return self.state == Enemy.enemyStates.AttackingRanged || self.state == Enemy.enemyStates.AttackingMelee;
    }

    void SetStateBasedOnAttacking()
    {
        if (StateIsAttacking())
        {
            if (state == IndicatorStates.disabled)
            {
                state = IndicatorStates.enabled;
            }
        }
        if (!StateIsAttacking())
        {
            state = IndicatorStates.disabled;
        }
    }
    //used only in ToggleImageEnabled, assumes state is attacking ranged/melee
    void SetAttackIndicatorImage()
    {
        if (self.state == Enemy.enemyStates.AttackingRanged)
        {
            img.sprite = rangedSprite;
        }
        else
        {
            img.sprite = meleeSprite;
        }
    }
    #endregion

    #region Image Color Selection
    void SetColor()
    {
        colors = GlobalFXManager.EnemyUIColors;
        var enemyNamePrefixes = EnemyInitializer.EnemyNamePrefixes;
        for (int i = 0; i < enemyNamePrefixes.Count; i++)
        {
            if (self.name.StartsWith(enemyNamePrefixes[i]))
            {
                img.color = colors[i];
                return;
            }
        }
        //failSafe if not in prefixes
        img.color = colors[0];
    }
    #endregion
    #region State Processing
    void SetState()
    {
        SetStateBasedOnAttacking();
    }
    void ActOnState()
    {
        switch (state)
        {
            case IndicatorStates.enabled:
                SetAttackIndicatorImage();
                ResetAnimationParams();
                img.enabled = true;
                StartAnimation();
                state = IndicatorStates.animating;
                break;
            case IndicatorStates.disabled:
                img.enabled = false;
                ResetAnimationParams();
                break;
            case IndicatorStates.animating:
                break;
        }
    }
    #endregion
    #region Animation
    void ResetAnimationParams()
    {
        //reset scale
        img.transform.localScale = Vector3.one;
        PlayOrStopParticles(false);
    }
    void StartAnimation()
    {
        StartCoroutine(AnimateForTime());
    }
    IEnumerator AnimateForTime(bool up = true)
    {
        if (up)
        {
            while (img.transform.localScale.x < targetMaxScale)
            {
                yield return new WaitForSeconds(animationStepLength);
                float newScale = img.transform.localScale.x + animationStepAmount;
                img.transform.localScale = new Vector3(newScale, newScale, newScale);
            }
            img.transform.localScale = Vector3.one * targetMaxScale;
        }
        else
        {
            while (img.transform.localScale.x > targetMinScale)
            {
                yield return new WaitForSeconds(animationStepLength);
                float newScale = img.transform.localScale.x - animationStepAmount;
                img.transform.localScale = new Vector3(newScale, newScale, newScale);
            }
            img.transform.localScale = Vector3.one * targetMinScale;
        }
        
    }
    public void PlayOrStopParticles(bool play = true) 
    {
        if (play)
        {
            if(img.sprite == meleeSprite && !meleeParticles.isPlaying)
            {
                meleeParticles.Play();
            }
            else
            {
                if (!rangedParticles.isPlaying)
                {
                    rangedParticles.Play();
                }
            }
        }
        else 
        {
            meleeParticles.Stop();
            rangedParticles.Stop();
            if(img.transform.localScale.x > targetMinScale)
            {
                StartCoroutine(AnimateForTime(false));
            }
        }
    }
    #endregion
}
