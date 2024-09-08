using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPSlot : MonoBehaviour
{
    [SerializeField] Image fillIMG;
    [SerializeField] ParticleSystem particles;
    public void SetHPColor(Color color)
    {
        fillIMG.enabled = true;
        fillIMG.color = color;
    }
    public void Deactivate()
    {
        fillIMG.enabled = false;
        particles.Play();
    }
}
