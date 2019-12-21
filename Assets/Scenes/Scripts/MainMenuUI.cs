using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private GameObject MainMenuPanel;

    [SerializeField]
    private GameObject SettingPanel;

    [SerializeField]
    private GameObject MessagePanel;

    [SerializeField]
    private GameObject StoryPanel;

    [SerializeField]
    private Slider m_slider;

    // Use this for initialization
    void Start()
    {
        ShowMainMenu();

        //打开主界面时，获取slider的值，默认为1
        m_slider.value = PlayerPrefs.GetFloat("AudioVolume", 1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //按下开始游戏按钮
    public void OnStartButton()
    {
        Debug.Log("Start button down.");

        //进入剧情界面
        MainMenuPanel.SetActive(false);
        SettingPanel.SetActive(false);
        MessagePanel.SetActive(false);
        StoryPanel.SetActive(true);
        // SceneManager.LoadScene(1); //tonwscene
    }

    //剧情界面按下开始游戏
    public void OnStartingGame()
    {
        Debug.Log("Game started.");

        //清除之前保存的全部数据
        PlayerPrefs.DeleteAll();

        SceneManager.LoadScene(1); //tonwscene
    }

    //按下继续游戏按钮
    public void OnContinueButton()
    {
        Debug.Log("Continue button down.");

        SceneManager.LoadScene(1);
    }

    //按下设置按钮
    public void OnSettingButton()
    {
        Debug.Log("Setting button down.");

        ShowSettingMenu();
    }

    //改变音量
    public void OnSliderValueChanged()
    {
        Debug.Log("Slider:" + m_slider.value);

        //将slider的值传递给AudioVolume
        PlayerPrefs.SetFloat("AudioVolume", m_slider.value);
    }

    //按下游戏讯息按钮
    public void OnMessageButton()
    {
        Debug.Log("Message button down.");

        ShowMessageMenu();
    }


    //按下结束游戏按钮
    public void OnEndButton()
    {
        Debug.Log("End button down.");

        Application.Quit();
    }

    //按下返回主界面按钮
    public void OnBackButton()
    {
        Debug.Log("Back button down.");

        PlayerPrefs.Save();
        ShowMainMenu();
    }

    //显示主界面
    private void ShowMainMenu()
    {
        MainMenuPanel.SetActive(true);
        SettingPanel.SetActive(false);
        MessagePanel.SetActive(false);
        StoryPanel.SetActive(false);
    }

    //显示设置界面
    private void ShowSettingMenu()
    {
        MainMenuPanel.SetActive(false);
        SettingPanel.SetActive(true);
        MessagePanel.SetActive(false);
        StoryPanel.SetActive(false);
    }

    //显示游戏讯息界面
    private void ShowMessageMenu()
    {
        MainMenuPanel.SetActive(false);
        SettingPanel.SetActive(false);
        MessagePanel.SetActive(true);
        StoryPanel.SetActive(false);
    }
}
