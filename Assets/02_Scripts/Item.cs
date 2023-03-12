using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int resetDeck;
    public int rndShuffle;
    public int changeCard;
    public int timeInfinite;

    public string dataPath;

    public List<Dictionary<string, object>> data;

    void Start()
    {
        
    }

    public int LoadStageItemInfo(int i)
    {
        data = CSVReader.Read(dataPath);

        int stageNum = int.Parse(PlayerPrefs.GetString("CUR_STAGE").Substring(7)); //stage1-2 -> 2
        int returnValue;

        switch (i)
        {
            case 1:
                resetDeck = int.Parse(data[stageNum]["dcDeck_PL"].ToString());
                returnValue = resetDeck;
                break;
            case 2:
                rndShuffle = int.Parse(data[stageNum]["rndShuffle_PL"].ToString());
                returnValue = rndShuffle;
                break;
            case 3:
                changeCard = int.Parse(data[stageNum]["otherPCDc_PL"].ToString());
                returnValue = changeCard;
                break;

            //case 4:
            //timeInfinite = int.Parse(data[stageNum]["turnTimeInf_PL"].ToString());
            //returnValue = timeInfinite;
            //break;
            default:
                returnValue = 0;
                break;
        }


        return returnValue;

    }

    public int LoadItemInfo(int i)
    {
        int returnValue = 0;

        switch (i)
        {
            case 1:
                resetDeck = PlayerPrefs.GetInt("USER_ITEM_RD_CNT");
                returnValue = resetDeck;
                break;
            case 2:
                rndShuffle = PlayerPrefs.GetInt("USER_ITEM_RS_CNT");
                returnValue = rndShuffle;
                break;
            case 3:
                changeCard = PlayerPrefs.GetInt("USER_ITEM_CC_CNT");
                returnValue = changeCard;
                break;

            //case 4:
            //timeInfinite = PlayerPrefs.GetInt("USER_ITEM_TI_CNT");
            //returnValue = timeInfinite;
            //break;
        }

        return returnValue;
    }
}
