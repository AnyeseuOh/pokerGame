using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager2 : MonoBehaviour
{
    public GameObject optionPanel;
    public GameObject chooseStagePanel;
    public GameObject shopPanel;

    public string curStage = "CUR_STAGE";

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void MoveToStage()
    {
        SceneManager.LoadScene("StageScene");
    }

    public void MoveToLobbyScene()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    public void MoveToScriptScene()
    {
        SceneManager.LoadScene("ScriptScene");
    }

    public void ExitTheGame()
    {
        Application.Quit();
    }

    public void SaveStage1()
    {
        PlayerPrefs.SetString(curStage, "stage1-1");
    }

    public void SaveStage2()
    {
        PlayerPrefs.SetString(curStage, "stage1-2");
    }

    public void SaveStage3()
    {
        PlayerPrefs.SetString(curStage, "stage1-3");
    }

    public void SaveStage4()
    {
        PlayerPrefs.SetString(curStage, "stage1-4");
    }

    public void SaveStage5()
    {
        PlayerPrefs.SetString(curStage, "stage1-5");
    }
}
