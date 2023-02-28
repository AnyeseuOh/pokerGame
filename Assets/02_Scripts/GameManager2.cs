using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager2 : MonoBehaviour
{

    public GameObject optionPanel;
    public GameObject chooseStagePanel;
    public GameObject shopPanel;

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

    public void ExitTheGame()
    {
        Application.Quit();
    }
}
