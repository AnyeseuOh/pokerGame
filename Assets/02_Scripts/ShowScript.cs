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

    public int startIndex = 0;
    public int lastIndex = 0;
    public int range;

    public int endIndex = 0;
    public string scriptState = "����";

    ArrayList indexArr = new ArrayList();
    ArrayList indexStrArr = new ArrayList();


    public GameManager2 gameManager;
    public AudioSource clickSFX;
    public AudioSource chosenBGM;

    public GameObject stage0Panel;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager2>();

        int stageNum = int.Parse(PlayerPrefs.GetString(curStage).Substring(7)); //stage1-2 -> 2
        string bgmName = $"BGM{stageNum}";

        chosenBGM = GameObject.Find(bgmName).GetComponentInChildren<AudioSource>();
        chosenBGM.Play();

        data = CSVReader.Read(dataPath);

        curStageNum = PlayerPrefs.GetString(curStage);
        Debug.Log($"CurStage ::: {curStageNum} cur stageNum :: {stageNum}");

        //curStageNum�̶� ��ġ�ϴ� ���� ���� ���� �ͺ��� ����ϰ� �������ʹ� �Լ��� ����ϰ�
        //index�� ���ؼ� i�� ��������ش�.

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
            if (curStNm.Equals("stage0"))
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



        if (PlayerPrefs.GetInt("END_INDEX") == 0) //start
        {
            startIndex = int.Parse(indexArr[stageNum - 1].ToString());
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
            clickSFX.Play();
            LoadDataFromCSV(startIndex, lastIndex);
            startIndex++;
        }
        else if (Input.GetMouseButtonDown(0) && scriptState.Equals("����"))
        {
            clickSFX.Play();
            LoadDataFromCSVEnd(startIndex, lastIndex);
            startIndex++;
        }
    }

    /*    public void LoadDataFromCSV(int i)
        {
            data = CSVReader.Read(dataPath);

            if (i < data.Count) //���� num �� ������ �˾Ƽ� ���ľ� ��
            {
                if (data[i]["StageNum"].Equals(curStageNum))
                {
                    talkerText.text = data[i]["Talker"].ToString();
                    scriptText.text = data[i]["Script"].ToString();
                    Debug.Log($"{data[i]["Talker"]} : {data[i]["Script"]}\n");
                }
            }
            else
            {
                Debug.Log("�������� �Ѿ��");
                gameManager.MoveToStage();
            }
        }*/


    public void LoadDataFromCSV(int cur, int last)
    {
        data = CSVReader.Read(dataPath);

        if (cur < last)
        {
            if (data[cur]["StageNum"].Equals(curStageNum) && data[cur]["Start/End"].Equals("����"))
            {
                talkerText.text = data[cur]["Talker"].ToString();
                scriptText.text = data[cur]["Script"].ToString();
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
                scriptText.text = data[cur]["Script"].ToString();
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

    public void ShowUserNamePanel()
    {
        stage0Panel.SetActive(true);
    }
}
