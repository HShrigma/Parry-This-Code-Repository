using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBarController : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [Header("Graphics")]
    [SerializeField] Color[] HpColors;
    [SerializeField] EnemyHPSlot[] HPSlots;

    [Header("Animation")]
    [SerializeField] float yShiftSpeed;
    [Header("References")]
    [SerializeField] Enemy self;
    float defaultYPos;
    float newYPos;
    void Start()
    {
        canvas.worldCamera = Camera.main;
        transform.rotation = Quaternion.Euler(90, 0, 0);
        defaultYPos = transform.position.y;
        newYPos = defaultYPos * 2;
    }

    void Update()
    {
        ShiftY();
    }

    void ShiftY()
    {
        if (self.state == Enemy.enemyStates.AttackingMelee && transform.position.y != newYPos)
        {
            if (transform.position.y < newYPos)
            {
                float toSet = transform.position.y + yShiftSpeed * Time.deltaTime;
                transform.position = new Vector3(transform.position.x, toSet, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, newYPos, transform.position.z);
            }
        }
        else if (self.state != Enemy.enemyStates.AttackingMelee && transform.position.y != defaultYPos)
        {
            if (transform.position.y > defaultYPos)
            {
                float toSet = transform.position.y - yShiftSpeed * Time.deltaTime;
                transform.position = new Vector3(transform.position.x, toSet, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, defaultYPos, transform.position.z);
            }
        }

        transform.rotation = Quaternion.Euler(90f, Quaternion.identity.y, Quaternion.identity.z);
    }

    public void InitHP(int HP)
    {
        //HP Slots length is always 4
        int endIndex = HP - 1;
        //Loop from last index to the actual HP index
        for (int i = HPSlots.Length - 1; i > endIndex; i--)
        {
            //disable unused HP Slot
            HPSlots[i].gameObject.SetActive(false);
        }
        for (int i = 0; i <= endIndex; i++)
        {
            if (!HPSlots[i].gameObject.activeSelf)
            {
                HPSlots[i].gameObject.SetActive(true);
            }
        }
        SetSlotsColor();
    }

    void SetSlotsColor()
    {
        int HP = self.GetHP();
        for (int i = 0; i < HP; i++)
        {
            HPSlots[i].SetHPColor(HpColors[HP - 1]);
        }
    }

    public void OnTakeDamage()
    {
        HPSlots[self.GetHP()].Deactivate();
        SetSlotsColor();
    }
}
