using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapMenuUI : MonoBehaviour
{
    [SerializeField]
    private GameObject AttributePanel;

    [SerializeField]
    private GameObject SettingPanel;

    [SerializeField]
    private GameObject RolePanel;

    [SerializeField]
    public GameObject Speak_map;

    [SerializeField]
    private Slider m_slider;

    public Knight pc;

    private int location;
    // Use this for initialization
    void Start()
    {
        SettingPanel.SetActive(false);
        location = SceneManager.GetActiveScene().buildIndex;
        //进入游戏时，获取slider的值，默认为1
        m_slider.value = PlayerPrefs.GetFloat("AudioVolume", 1);
    }

    // Update is called once per frame
    void Update()
    {

    }


    //进入地图对话框
    public void OnSpeak_map()
    {


        Debug.Log("Map Entered.");

        //显示设置界面
        Speak_map.SetActive(false);

        pc.SpeakOver();

    }


    //按下设置按钮
    public void OnSettingButton()
    {
        Debug.Log("Setting button down.");

        //显示设置界面
        ShowSettingPanel();

        //待实现禁止地图中的其他操作


    }

    //按下角色头像按钮
    public void OnRoleButton()
    {
        Debug.Log("Role button down.");

        ShowRolePanel();
    }

    //按下继续游戏按钮
    public void OnContinueButton()
    {
        Debug.Log("Continue button down.");

        ClosePanel();
    }

    //按下保存游戏按钮
    public void OnSaveButton()
    {
        Debug.Log("Save button down.");

        //保存游戏数据
        PlayerPrefs.SetFloat("maxHealth", pc.MyMaxHealth);
        PlayerPrefs.SetFloat("maxMana", pc.MyMaxMana);
        PlayerPrefs.SetFloat("currentHealth", pc.MyCurrentHealth);
        PlayerPrefs.SetFloat("currentMana", pc.MyCurrentMana);
        PlayerPrefs.SetInt("attack", pc.MyAttack);
        PlayerPrefs.SetInt("defend", pc.MyDefend);
        PlayerPrefs.SetInt("level", pc.MyLevel);
        PlayerPrefs.SetInt("exp", pc.MyExp);
        PlayerPrefs.SetInt("maxExp", pc.MyMaxExp);
        PlayerPrefs.SetInt("money", pc.MyMoney);
        PlayerPrefs.SetInt("healthPotion", pc.MyHealthPotion);
        PlayerPrefs.SetInt("manaPotion", pc.MyManaPotion);
        PlayerPrefs.SetInt("bothPotion", pc.MyBothPotion);
        PlayerPrefs.SetInt("stage", pc.Mystage);
    }

    //按下回主菜单按钮
    public void OnReturnButton()
    {
        Debug.Log("Return button down.");

        SceneManager.LoadScene(0);
    }

    //按下关闭角色信息按钮
    public void OnCloseButton()
    {
        Debug.Log("Close button down.");

        ClosePanel();
    }

    //改变音量
    public void OnSliderValueChanged()
    {
        Debug.Log("Slider:" + m_slider.value);

        //将slider的值传递给AudioVolume
        PlayerPrefs.SetFloat("AudioVolume", m_slider.value);
    }

    //显示设置界面
    private void ShowSettingPanel()
    {
        SettingPanel.SetActive(true);
        RolePanel.SetActive(false);
    }

    //显示角色面板界面
    private void ShowRolePanel()
    {
        RolePanel.SetActive(true);
        SettingPanel.SetActive(false);
        if(location != 1)
        AttributePanel.SetActive(false);

    }

    //返回游戏地图界面
    private void ClosePanel()
    {
        SettingPanel.SetActive(false);
        RolePanel.SetActive(false);
        if(location!=1)
            AttributePanel.SetActive(true);
    }

}
