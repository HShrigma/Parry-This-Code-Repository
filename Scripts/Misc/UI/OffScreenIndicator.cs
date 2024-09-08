using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffScreenIndicator : MonoBehaviour
{
    enum indicatorStates
    {
        uninitialized,
        initialized,
        drawn,
        undrawn
    }
    indicatorStates state = indicatorStates.uninitialized;

    [Header("Colours")]
    List<Color> colors;

    [Header("Animation")]
    [SerializeField][Range(0f, 1f)] float minAlpha;
    [SerializeField][Range(0f, 1f)] float maxAlpha;
    [SerializeField][Range(1f, 2f)] float maxScale;
    [SerializeField] float duration;
    [SerializeField] ParticleSystem particle;
    float alphaRange;
    float scaleRange;

    List<string> enemyNamePrefixes = EnemyInitializer.EnemyNamePrefixes;

    [Header("Position")]
    [SerializeField][Range(0f, 1f)] float boundCoeff;
    [SerializeField] RectTransform rectTransform;
    RectTransform parentRect;
    Camera UICam;

    Vector2 boundsXY;
    Vector2 localPos;


    [SerializeField] Image img;
    Enemy enemy;
    Vector3 enemyScreenPos = Vector3.zero;
    Vector3 screenCenter;
    bool shrink;
    void Start()
    {
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
        alphaRange = maxAlpha - minAlpha;
        scaleRange = maxScale - 1f;

    }
    void Update()
    {
        if (enemy == null)
        {
            Destroy(gameObject);
        }
        SetState();
        ActOnState();
    }

    void SetColor()
    {
        colors = GlobalFXManager.EnemyUIColors;
        for (int i = 0; i < enemyNamePrefixes.Count; i++)
        {
            if (enemy.name.StartsWith(enemyNamePrefixes[i]))
            {
                img.color = colors[i];
                SetColorAlpha(maxAlpha);
                return;
            }
        }
        //failSafe if not in prefixes
        img.color = colors[0];
        SetColorAlpha(maxAlpha);
        return;
    }

    public void SetParams(Enemy _enemy, RectTransform _canvasRect, Camera _cam)
    {
        enemy = _enemy;
        parentRect = _canvasRect;
        UICam = _cam;
        boundsXY = new Vector2(parentRect.rect.width * boundCoeff, parentRect.rect.height * boundCoeff);
        SetColor();
        state = indicatorStates.initialized;
    }

    bool isOffScreen()
    {
        if (enemy != null && enemy.gameObject.activeSelf)
        {
            enemyScreenPos = Camera.main.WorldToScreenPoint(enemy.transform.position);

            return enemyScreenPos.z < 0 ||
                enemyScreenPos.x >= Screen.width ||
                enemyScreenPos.y >= Screen.height ||
                enemyScreenPos.x <= 0 ||
                enemyScreenPos.y <= 0;
        }
        return false;
    }
    void SetState()
    {
        if (state != indicatorStates.uninitialized)
        {
            if (isOffScreen())
            {
                state = indicatorStates.drawn;
            }
            else
            {
                state = indicatorStates.undrawn;
            }
        }
    }

    void ActOnState()
    {
        switch (state)
        {
            case indicatorStates.drawn:
                DrawIndicator();
                break;
            case indicatorStates.undrawn:
                DisableImage();
                break;
            default:
                break;
        }
    }
    void DisableImage()
    {
        if (img.enabled)
        {
            img.enabled = false;
        }
        if (particle.isPlaying)
        {
            particle.Stop();
            particle.Clear();        
        }
    }
    void DrawIndicator()
    {

        SetPosition();
        SetScaleForDistance();
        SetRotation();
        AnimateIndicator();
        if (!img.enabled)
        {
            img.enabled = true;
        }
        if (!particle.isPlaying)
        {
            particle.Play();
        }
    }

    void SetPosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, enemyScreenPos, UICam, out localPos);

        Vector2 clampedPos = GetClampedCanvasPos(localPos);
        rectTransform.anchoredPosition = clampedPos;
    }

    Vector2 GetClampedCanvasPos(Vector2 localPos)
    {
        float minBoundsX = (-parentRect.rect.width / 2) + boundsXY.x;
        float maxBoundsX = (parentRect.rect.width / 2) - boundsXY.x; 
        
        float minBoundsY = (-parentRect.rect.height/ 2) + boundsXY.y;
        float maxBoundsY = (parentRect.rect.height / 2) - boundsXY.y;
        
        return new Vector2(
           Mathf.Clamp(localPos.x,minBoundsX,maxBoundsX),
           Mathf.Clamp(localPos.y,minBoundsY, maxBoundsY));
    }
    void SetScaleForDistance()
    {
        //scale the arrow based on proximity to the screen bounds
        float screenDistance = Mathf.Min(
            Mathf.Abs(-localPos.x),
            Mathf.Abs(-localPos.y)
        );
        //adjust scale based on distance to the screen (inverse relationship)
        float proximityFactor = Mathf.Clamp01(1 - (screenDistance / (parentRect.rect.width / 2)));
        float newScale = Mathf.Lerp(1f, maxScale, proximityFactor);
        img.transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    void SetRotation()
    {
        Vector3 lookDir = enemyScreenPos - screenCenter;
        lookDir.Normalize();
        //get angle in radians (where tan = y/x)
        //towards the look direction and convert to degrees
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    void AnimateIndicator()
    {
        SetShrink();
        //get current values
        float alpha = img.color.a;
        float scale = img.transform.localScale.x;
        //get proportional values.
        //Ensures alpha and scale always reach min/max simultaneously
        float alphaRate = (alphaRange / scaleRange) * duration * Time.deltaTime;
        float scaleRate = (scaleRange / alphaRange) * duration * Time.deltaTime;
        if (shrink)
        {
            alpha -= alphaRate;
            scale -= scaleRate;
        }
        else
        {
            alpha += alphaRate;
            scale += scaleRate;
        }
        SetColorAlpha(alpha);
        img.transform.localScale = new Vector3(scale, scale, scale);
        particle.transform.localScale = new Vector3(scale,scale, scale);
    }

    void SetShrink()
    {
        if (img.color.a <= minAlpha)
        {
            SetColorAlpha(minAlpha);
            shrink = false;
        }
        if (img.color.a >= maxAlpha)
        {
            SetColorAlpha(maxAlpha);
            shrink = true;
        }
    }

    void SetColorAlpha(float a)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, a);
    }
}
