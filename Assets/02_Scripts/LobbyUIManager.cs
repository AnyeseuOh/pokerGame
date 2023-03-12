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

    public Text resetDeckText;
    public Text rndShuffleText;
    public Text changeCardText;
    public Text timeInfiniteText;

    public GameObject ExplainText_1;
    public GameObject ExplainText_2;

    public bool isClear = false; //prefs

    public GameObject notClearImg;
    public AudioSource clickSFX;

    public string dataPath;
    public List<Dictionary<string, object>> data;

    void Start()
    {
        if (PlayerPrefs.GetInt("USER_ITEM_RD_CNT").ToString() == null)
        {
            PlayerPrefs.SetInt("USER_ITEM_RD_CNT", 0);
        }

        if (PlayerPrefs.GetInt("USER_ITEM_RS_CNT").ToString() == null)
        {
            PlayerPrefs.SetInt("USER_ITEM_RS_CNT", 0);
        }

        if (PlayerPrefs.GetInt("USER_ITEM_CC_CNT").ToString() == null)
        {
            PlayerPrefs.SetInt("USER_ITEM_CC_CNT", 0);
        }

        if (PlayerPrefs.GetInt("USER_ITEM_TI_CNT").ToString() == null)
        {
            PlayerPrefs.SetInt("USER_ITEM_TI_CNT", 0);
        }

        resetDeckText.text = PlayerPrefs.GetInt("USER_ITEM_RD_CNT").ToString();
        rndShuffleText.text = PlayerPrefs.GetInt("USER_ITEM_RS_CNT").ToString();
        changeCardText.text = PlayerPrefs.GetInt("USER_ITEM_CC_CNT").ToString();
        timeInfiniteText.text = PlayerPrefs.GetInt("USER_ITEM_TI_CNT").ToString();
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

        if (stageNum == 0)
        {
            descriText.text = data[stageNum]["stageDescri"].ToString();
            if (descriText.text.Contains("/"))
            {
                string[] dText = descriText.text.Split("/");
                descriText.text = dText[0] + "\n" + dText[1];
            }
            stageText.text = "꿈?";

            enemyName.text = $"ENEMY [ { data[stageNum]["enemyName"].ToString() } ]";
            enemyChipText.text = "???";
            playerChipText.text = "???";
            availItemCnt.text = "???";
        }
        else
        {
            //스테이지 설명
            descriText.text = data[stageNum]["stageDescri"].ToString();
            if (descriText.text.Contains("/"))
            {
                string[] dText = descriText.text.Split("/");
                descriText.text = dText[0] + "\n" + dText[1];
            }
            
            stageText.text = PlayerPrefs.GetString(curStage).Substring(5, 3);


            //스테이지 정보 (적/칩개수/아이템 등)
            enemyChipText.text = $"Enemy 칩 개수 : {data[stageNum]["chip_EN"].ToString()}";
            playerChipText.text = $"Player 칩 개수 {data[stageNum]["chip_PL"].ToString()}";
            enemyName.text = $"ENEMY [ { data[stageNum]["enemyName"].ToString() } ]";
            availItemCnt.text = "사용 가능 아이템 수\n" +
                                    data[stageNum]["dcDeck_PL"].ToString() + " / " + data[stageNum]["rndShuffle_PL"].ToString() + " / " +
                                    data[stageNum]["otherPCDc_PL"].ToString() + " / " + data[stageNum]["turnTimeInf_PL"].ToString();

            //Debug.Log($"{data[stageNum]["stageDescri"]} {data[stageNum]["StageNum"]}\n");
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

    public void MoveToNextPage()
    {
        ExplainText_1.SetActive(false);
        ExplainText_2.SetActive(true);
    }

    public void MoveToPrePage()
    {
        ExplainText_1.SetActive(true);
        ExplainText_2.SetActive(false);
    }
}
