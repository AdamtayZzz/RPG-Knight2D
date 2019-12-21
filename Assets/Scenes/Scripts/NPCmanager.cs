using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// NPC交互
/// </summary>
public class NPCmanager : MonoBehaviour
{

    [SerializeField]
    public GameObject NPCPanel;

    [SerializeField]
    public GameObject FunctionPanel;

    [SerializeField]
    public GameObject LackMoneyPanel;

    [SerializeField]
    public GameObject ConfirmPanel1;

    [SerializeField]
    public GameObject ConfirmPanel2;

    [SerializeField]
    public GameObject ResultPanel1;

    [SerializeField]
    public GameObject ResultPanel2;

    [SerializeField]
    public Text ResultText1;

    [SerializeField]
    public Text ResultText2;


    //对话框
    [SerializeField]
    public GameObject dialogImage;

    public Knight pc;

    // Use this for initialization
    void Start()
    {
        NPCPanel.SetActive(false);

        //showTimer = -1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //按下交谈按钮，显示交谈内容
    public void OnDialogButton()
    {
        dialogImage.SetActive(true);  // 在此控件的关闭函数中添加解冻函数： Knight.SpeakOver();
        NPCPanel.SetActive(false);
    }

    public void OnFunctionButton()
    {
        Debug.Log("Function button down.");

        FunctionPanel.SetActive(true);
        NPCPanel.SetActive(false);
    }

    public void OnCloseDialogButton()
    {
        pc.SpeakOver();
        dialogImage.SetActive(false);
    }

    //按下离开按钮，关闭NPC选项框
    public void OnCloseButton()
    {
        pc.SpeakOver();//结束说话
        NPCPanel.SetActive(false);
    }

    //显示NPC选项框
    public void ShowNPCPanel()
    {
        pc.SpeakStart();//开始说话
        NPCPanel.SetActive(true);
    }

    /// <summary>
    /// 关闭缺少金币的提示框
    /// </summary>
    public void OnCloseLackMoneyButton()
    {
        pc.SpeakOver();//结束说话
        LackMoneyPanel.SetActive(false);
    }



    //旅馆功能的相关函数

    /// <summary>
    /// 按下确定休息按钮
    /// </summary>
    public void OnYesButton()
    {
        if (pc.MyMoney >= 100)
        {
            pc.ChangeAttribute(0, 0, 0, -100);
            pc.ChangeHealth(pc.MyMaxHealth);
            pc.ChangeMana(pc.MyMaxMana);
        }
        else
        {

            LackMoneyPanel.SetActive(true);
        }
    }

    public void OnNoButton()
    {
        pc.SpeakOver();//结束说话
        FunctionPanel.SetActive(false);
    }

    //药品商功能相关函数


    /// <summary>
    /// 购买血药
    /// </summary>
    public void OnBuyHealthPotion()
    {
        Debug.Log("Buy health potion button down.");

        if (pc.MyMoney >= 70)
        {
            pc.ChangeAttribute(0, 0, 0, -70);
            pc.ChangePotion(1, 0, 0);
        }
        else
        {
            //  FunctionPanel.SetActive(false);
            LackMoneyPanel.SetActive(true);
        }
    }

    /// <summary>
    /// 购买蓝药
    /// </summary>
    public void OnBuyManaPotion()
    {
        if (pc.MyMoney >= 80)
        {
            pc.ChangeAttribute(0, 0, 0, -80);
            pc.ChangePotion(0, 1, 0);
        }
        else
        {
            // FunctionPanel.SetActive(false);
            LackMoneyPanel.SetActive(true);
        }
    }

    /// <summary>
    /// 购买血蓝药
    /// </summary>
    public void OnBuyBothPotion()
    {
        if (pc.MyMoney >= 100)
        {
            pc.ChangeAttribute(0, 0, 0, -100);
            pc.ChangePotion(0, 0, 1);
        }
        else
        {
            //FunctionPanel.SetActive(false);
            LackMoneyPanel.SetActive(true);
        }
    }

    //强化系统相关的函数

    public void OnCloseFunctionButton()
    {
        pc.SpeakOver();//结束说话
        FunctionPanel.SetActive(false);
    }

    public void OnCloseResultPanel()
    {
        //pc.SpeakOver();//结束说话
        ResultPanel1.SetActive(false);
        ResultPanel2.SetActive(false);
    }

    /// <summary>
    /// 强化攻击力
    /// </summary>
    public void OnAttackUpButton()
    {
        ConfirmPanel1.SetActive(true);
    }

    public void OnConfirmYesButton1()
    {
        ConfirmPanel1.SetActive(false);
        if (pc.MyMoney >= 100)
        {
            pc.ChangeAttribute(0, 0, 0, -100);
            int rate = Random.Range(0, 9);
            if (rate >= 6)
            {
                pc.ChangeAttribute(3, 0, 0, 0);
                ResultPanel1.SetActive(true);
                ResultText1.text = "成功";
            }
            else
            {
                Debug.Log("失败");
                ResultPanel1.SetActive(true);
                ResultText1.text = "失败";
            }
        }
        else
        {
            LackMoneyPanel.SetActive(true);
        }
    }

    public void OnConfirmNoButton1()
    {
        ConfirmPanel1.SetActive(false);
    }

    /// <summary>
    /// 强化防御力
    /// </summary>
    public void OnDefendUpButton()
    {
        ConfirmPanel2.SetActive(true);
    }

    public void OnConfirmYesButton2()
    {
        ConfirmPanel2.SetActive(false);
        if (pc.MyMoney >= 120)
        {
            pc.ChangeAttribute(0, 0, 0, -120);
            int rate = Random.Range(0, 9);
            if (rate >= 6)
            {
                pc.ChangeAttribute(0, 2, 0, 0);
                ResultPanel2.SetActive(true);
                ResultText2.text = "成功";
            }
            else
            {
                Debug.Log("fail");
                ResultPanel2.SetActive(true);
                ResultText2.text = "fail";
            }
        }
        else
        {
            LackMoneyPanel.SetActive(true);
        }
    }

    public void OnConfirmNoButton2()
    {
        ConfirmPanel2.SetActive(false);
    }
}