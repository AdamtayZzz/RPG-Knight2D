using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 角色属性的显示
/// </summary>
public class AttributeManager : MonoBehaviour
{
    public static AttributeManager instance { get; private set; }

    [SerializeField]
    private Text attack;

    [SerializeField]
    private Text defend;

    [SerializeField]
    private Text level;

    [SerializeField]
    private Text Blood;

    [SerializeField]
    private Text mana;

    [SerializeField]
    private Text money;

    [SerializeField]
    private Text healthPotion;

    [SerializeField]
    private Text manaPotion;

    [SerializeField]
    private Text bothPotion;


    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    public void UpdateAttribute(float Attack, float Defend, float Level,float currentHealth,float currentMana,int Money)
    {
        //显示攻击、防御、等级的具体数值
        attack.text = "攻击:"+Attack;
        defend.text = "防御:" + Defend;
        level.text = "等级：" + Level;
        Blood.text = "血量:" + currentHealth;
        mana.text = "蓝量:" + currentMana;
        money.text= "金钱:" + Money;
    }

    public void UpdateHealthPotion(int HealthPotion)
    {
        healthPotion.text = "" + HealthPotion;
    }

    public void UpdateManaPotion(int ManaPotion)
    {
        manaPotion.text = "" + ManaPotion;
    }

    public void UpdateBothPotion(int BothPotion)
    {
        bothPotion.text = "" + BothPotion;
    }
}
