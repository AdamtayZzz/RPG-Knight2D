using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 更新经验条显示
/// </summary>
public class ExpBar : MonoBehaviour
{

    public static ExpBar instance { get; private set; }

    [SerializeField]
    private Image content;

    [SerializeField]
    private Text statValue;

    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    public void UpdateBar(int currentAmount, int maxAmount)
    {
        content.fillAmount = (float)currentAmount / (float)maxAmount;

        //显示经验条的具体数值
        statValue.text = currentAmount + "/" + maxAmount;
    }
}
