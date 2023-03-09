using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ShowScript : MonoBehaviour
{
    public string dataPath;
    public List<Dictionary<string, object>> data;
    public string curStageNum;
    public string curStage = "CUR_STAGE"; //prefs

    public Text talkerText;
    public Text scriptText;
    public InputField userNameText;

    public int startIndex = 0;
    public int lastIndex = 0;
    public int range;

    public int endIndex = 0;
    public string scriptState = "����";

    ArrayList indexArr = new ArrayList();
    ArrayList indexStrArr = new ArrayList();

    public string userName;

    public GameManager2 gameManager;
    public AudioSource clickSFX;
    public AudioSource chosenBGM;

    public GameObject stage0Panel;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager2>();

        int stageNum = int.Parse(PlayerPrefs.GetString(curStage).Substring(7)); //stage1-2 -> 2
        string bgmName = $"BGM{stageNum}";

        //BGM ����
        chosenBGM = GameObject.Find(bgmName).GetComponentInChildren<AudioSource>();
        chosenBGM.Play();

        data = CSVReader.Read(dataPath);

        curStageNum = PlayerPrefs.GetString(curStage);
        Debug.Log($"CurStage ::: {curStageNum} cur stageNum :: {stageNum}");

        if (PlayerPrefs.GetString("CUR_SCRIPT").Equals("END"))
        {
            scriptState = "����";
        }
        else
        {
            scriptState = "����";
        }

        for (int i=0; i<data.Count; i++)
        {
            string curStNm = data[i]["StageNum"].ToString(); //stage1-0
            if (curStNm.Equals("stage1-0"))
            {
                continue;
            }
            else if (!indexStrArr.Contains(curStNm))
            {
                indexStrArr.Add(curStNm);
                indexArr.Add(i);
                Debug.Log($"�߰� ::: index: {i} - �������� :{curStNm}");
            }
        }


        if (stageNum == 0)
        {
            startIndex = 0;
            PlayerPrefs.SetString("CUR_SCRIPT", "START");
            scriptState = "����";
        }

        else if (PlayerPrefs.GetInt("END_INDEX") == 0) //start
        {
            startIndex = int.Parse(indexArr[stageNum - 1].ToString());
            Debug.Log($"start index ::: {startIndex}");
        }
        
        else
        {
            startIndex = PlayerPrefs.GetInt("END_INDEX");
        }
        
        lastIndex = int.Parse(indexArr[stageNum].ToString());
        Debug.Log($"startIndex: {startIndex}\nlastIndex: {lastIndex}");

        talkerText.text = " ";
        scriptText.text = " ";
        
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && scriptState.Equals("����"))
        {
            if (curStageNum == "stage1-0" && startIndex == 2)
            {
                Debug.Log("�̰� ����???");
                stage0Panel.SetActive(true);
                startIndex++;
            }
            else if (curStageNum == "stage1-0" && startIndex == 3)
            {
                clickSFX.Play();
                Debug.Log("�ϴ� �ѱ�� ��");
            }
            else if (curStageNum == "stage1-0" && startIndex == 4)
            {
                Debug.Log("������ ��ũ��Ʈ");
                clickSFX.Play();
                LoadDataFromCSV0(2);
                startIndex++;
            }
            else if (curStageNum == "stage1-0" && startIndex == 5)
            {
                clickSFX.Play();
                Debug.Log("�κ�� �Ѿ��1");
                gameManager.MoveToLobbyScene();
            }
            else
            {
                clickSFX.Play();
                LoadDataFromCSV(startIndex, lastIndex);
                startIndex++;
            }
            
        }
        
        else if (Input.GetMouseButtonDown(0) && scriptState.Equals("����"))
        {
            clickSFX.Play();
            LoadDataFromCSVEnd(startIndex, lastIndex);
            startIndex++;
        }
    }

    public void LoadDataFromCSV0(int cur)
    {
        data = CSVReader.Read(dataPath);

        talkerText.text = data[cur]["Talker"].ToString();
        if (PlayerPrefs.GetString("USER_NAME") != null)
        {
            talkerText.text = PlayerPrefs.GetString("USER_NAME");
        }
        scriptText.text = data[cur]["Script"].ToString();
        if (scriptText.text.Contains("/"))
        {
            string[] dText = scriptText.text.Split("/");
            scriptText.text = dText[0] + "\n" + dText[1];
        }
        Debug.Log($"{data[cur]["Talker"]} : {data[cur]["Script"]}\n");

    }


    public void LoadDataFromCSV(int cur, int last)
    {
        data = CSVReader.Read(dataPath);

        if (cur < last)
        {
            if (data[cur]["StageNum"].Equals(curStageNum) && data[cur]["Start/End"].Equals("����"))
            {
                talkerText.text = data[cur]["Talker"].ToString();
                if (PlayerPrefs.GetString("USER_NAME") != null && talkerText.text.Contains("User"))
                {
                    talkerText.text = talkerText.text.Replace("User", PlayerPrefs.GetString("USER_NAME"));
                }
                scriptText.text = data[cur]["Script"].ToString();
                scriptText.text = scriptText.text.Replace("User", PlayerPrefs.GetString("USER_NAME"));
                if (scriptText.text.Contains("/"))
                {
                    string[] dText = scriptText.text.Split("/");
                    scriptText.text = dText[0] + "\n" + dText[1];
                }
                Debug.Log($"{data[cur]["Talker"]} : {data[cur]["Script"]}\n");
            }
            else
            {
                Debug.Log("�������� �Ѿ��");
                gameManager.MoveToStage();
                PlayerPrefs.SetInt("END_INDEX", cur);
                PlayerPrefs.SetString("CUR_SCRIPT", "END");
            }
        }
        
    }



    public void LoadDataFromCSVEnd(int cur, int last)
    {
        data = CSVReader.Read(dataPath);

        if (cur < last) 
        {
            if (data[cur]["StageNum"].Equals(curStageNum) && data[cur]["Start/End"].Equals("����"))
            {
                talkerText.text = data[cur]["Talker"].ToString();
                if (PlayerPrefs.GetString("USER_NAME") != null && talkerText.text.Contains("User"))
                {
                    talkerText.text = talkerText.text.Replace("User", PlayerPrefs.GetString("USER_NAME"));
                }
                scriptText.text = data[cur]["Script"].ToString();
                scriptText.text = scriptText.text.Replace("User", PlayerPrefs.GetString("USER_NAME"));
                if (scriptText.text.Contains("/"))
                {
                    string[] dText = scriptText.text.Split("/");
                    scriptText.text = dText[0] + "\n" + dText[1];
                }
                Debug.Log($"{data[cur]["Talker"]} : {data[cur]["Script"]}\n");
            }
        }
        else
        {
            Debug.Log("�κ�� �Ѿ��");
            gameManager.MoveToLobbyScene();
            PlayerPrefs.SetInt("END_INDEX", 0);
            PlayerPrefs.SetString("CUR_SCRIPT", "START");
        }
    }

    public void ShowOffUserNamePanel()
    {
        userName = userNameText.text.ToString();
        Debug.Log($"userName: {userName}");
        PlayerPrefs.SetString("USER_NAME", userName);
        stage0Panel.SetActive(false);
        startIndex++;
    }
}
