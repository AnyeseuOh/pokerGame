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

    public string dataPath;
    public List<Dictionary<string, object>> data;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadDataFromCSV()
    {
        data = CSVReader.Read(dataPath);

        string stageName = PlayerPrefs.GetString(curStage);
        int stageNum = int.Parse(stageName.Substring(7)); //stage1-2 -> 2
        Debug.Log($"StageName ::: {stageName}" +
                     $"StageNum ::: {stageNum}\n");

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

                Debug.Log($"{data[i]["stageDescri"]}" +
                         $"{data[i]["StageNum"]}\n");
            }
        }


    }
}
