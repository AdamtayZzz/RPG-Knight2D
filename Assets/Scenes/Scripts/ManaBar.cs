using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour {

    public static ManaBar instance { get; private set; }

    [SerializeField]
    private Image content;

    [SerializeField]
    private Text statValue;

    [SerializeField]
    private float lerpSpeed;

    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    public void UpdateBar(float currentAmount, float maxAmount)
    {
        content.fillAmount = Mathf.Lerp(content.fillAmount, currentAmount / maxAmount, Time.deltaTime * lerpSpeed);

        //显示蓝条的具体数值
        statValue.text = currentAmount + "/" + maxAmount;
    }

}
