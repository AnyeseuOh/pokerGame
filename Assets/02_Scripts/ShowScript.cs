using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowScript : MonoBehaviour
{
    public string dataPath;
    public List<Dictionary<string, object>> data;
    public string curStageNum;
    public string curStage = "CUR_STAGE"; //prefs

    public Text talkerText;
    public Text scriptText;

    public int i = 0;
    public int range;

    public GameManager2 gameManager;
    public AudioSource clickSFX;
    public AudioSource chosenBGM;

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
        talkerText.text = data[0]["Talker"].ToString();
        scriptText.text = data[0]["Script"].ToString();
        Debug.Log($"{data[0]["Talker"]} : {data[0]["Script"]}\n");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickSFX.Play();
            LoadDataFromCSV(i);
            i++;
        }
    }

    public void LoadDataFromCSV(int i)
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
    }
}
