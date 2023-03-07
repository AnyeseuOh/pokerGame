using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    public string curStage = "CUR_STAGE"; //prefs
    public Text stageText;
    public Text descriText;
    public Text enemyChipText;
    public Text playerChipText;
    public Text enemyName;
    public Text availItemCnt;
    public bool isClear = false; //prefs

    public GameObject notClearImg;
    public AudioSource clickSFX;

    public string dataPath;
    public List<Dictionary<string, object>> data;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickSFX.Play();
        }
    }

    public void LoadDataFromCSV()
    {
        data = CSVReader.Read(dataPath);

        string stageName = PlayerPrefs.GetString(curStage);
        int stageNum = int.Parse(stageName.Substring(7)); //stage1-2 -> 2
        //1Debug.Log($"StageName ::: {stageName} StageNum ::: {stageNum}\n");

        for (int i = 0; i < data.Count; i++)
        {
            if (data[i]["StageNum"].Equals(stageName))
            {
                //�������� ����
                descriText.text = data[i]["stageDescri"].ToString();
                stageText.text = PlayerPrefs.GetString(curStage).Substring(5, 3);


                //�������� ���� (��/Ĩ����/������ ��)
                enemyChipText.text = $"Enemy Ĩ ���� : {data[i]["chip_EN"].ToString()}";
                playerChipText.text = $"Player Ĩ ���� {data[i]["chip_PL"].ToString()}";
                enemyName.text = $"ENEMY [ { data[i]["enemyName"].ToString() } ]";
                availItemCnt.text = "��� ���� ������ ��\n" +
                                        data[i]["dcDeck_PL"].ToString() + " / " + data[i]["rndShuffle_PL"].ToString() + " / " +
                                        data[i]["otherPCDc_PL"].ToString() + " / " + data[i]["turnTimeInf_PL"].ToString();

                //Debug.Log($"{data[i]["stageDescri"]} {data[i]["StageNum"]}\n");
            }
        }


    }

    public void CheckClear()
    {
        //���� ��������(CUR_STAGE�� �����)�� �̸��� ���� �ܰ谡 isClear Y�� ���
        //Not Clear�� setActive false
        string stageName = PlayerPrefs.GetString(curStage); //stage1-2
        int stageNum = int.Parse(stageName.Substring(7)); //stage1-2 -> 2

        string stageClear = "IS_CLEAR_" + stageNum;
        if (PlayerPrefs.GetString(stageClear) == "Y")
        {
            notClearImg.SetActive(false);
        }
        else
        {
            notClearImg.SetActive(true);
        }
    }
}
