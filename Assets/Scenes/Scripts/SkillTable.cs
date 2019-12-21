using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 技能栏显示
/// </summary>
public class SkillTable : MonoBehaviour
{
    public Knight pc;
    public Image sword;
    public Image shield;
    public Text health;
    public Text mana;
    public Text both;
    //从Knight脚本中的到信息
    private float timer_sword = 0.0f;
    private float timer_shield = 0.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer_sword = pc.timer_sword;
        timer_shield = pc.timer_shield;
        this.sword.fillAmount = (3.0f - timer_sword) / 3.0f;
        this.shield.fillAmount = (2.0f - timer_shield) / 2.0f;
        health.text = "" + pc.MyHealthPotion;
        mana.text = "" + pc.MyManaPotion;
        both.text = "" + pc.MyBothPotion;
    }
}
