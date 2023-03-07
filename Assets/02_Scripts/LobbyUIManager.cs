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
                //스테이지 설명
                descriText.text = data[i]["stageDescri"].ToString();
                stageText.text = PlayerPrefs.GetString(curStage).Substring(5, 3);


                //스테이지 정보 (적/칩개수/아이템 등)
                enemyChipText.text = $"Enemy 칩 개수 : {data[i]["chip_EN"].ToString()}";
                playerChipText.text = $"Player 칩 개수 {data[i]["chip_PL"].ToString()}";
                enemyName.text = $"ENEMY [ { data[i]["enemyName"].ToString() } ]";
                availItemCnt.text = "사용 가능 아이템 수\n" +
                                        data[i]["dcDeck_PL"].ToString() + " / " + data[i]["rndShuffle_PL"].ToString() + " / " +
                                        data[i]["otherPCDc_PL"].ToString() + " / " + data[i]["turnTimeInf_PL"].ToString();

                //Debug.Log($"{data[i]["stageDescri"]} {data[i]["StageNum"]}\n");
            }
        }


    }

    public void CheckClear()
    {
        //현재 스테이지(CUR_STAGE에 저장된)의 이름과 같은 단계가 isClear Y일 경우
        //Not Clear를 setActive false
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
