using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    //public InputField chipField;
    public StageManager stageManager;
    public Text chipText; //player input용
    public Text overChipText;
    public Text[] plInfo;
    public Text[] enInfo;
    public Text turnTime;
    public Text comBetChipText;
    public Text turnCnt;
    public Text matchCnt;
    public Text curTurnPlayer;
    public Button submitBtn;
    public Button overSubmitBtn;

    public GameObject overBetBtn;

    public int overBetChips;
    public int playerBetMinRange;
    public int playerBetMaxRange;

    void Start()
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        plInfo = GameObject.Find("PlayerInfo").GetComponentsInChildren<Text>();
        enInfo = GameObject.Find("EnemyInfo").GetComponentsInChildren<Text>();
        //turnTime = GameObject.Find("TurnTime").GetComponent<Text>();

        submitBtn.interactable = false;
        overSubmitBtn.interactable = false;
    }

    void Update()
    {
        if (playerBetMinRange == playerBetMaxRange || playerBetMaxRange < playerBetMinRange)
        {
            overBetBtn.SetActive(false);
        } else
        {
            overBetBtn.SetActive(true);
        }

        plInfo[3].text = $"X {stageManager.playerChip}";
        enInfo[2].text = $"X {stageManager.enemyChip}";
        //turnTime.text = $"제한시간: 00.00";
        comBetChipText.text = $"X {stageManager.bettingChip}";
        turnCnt.text = $"{stageManager.turnCnt}";
        curTurnPlayer.text = $"{stageManager.curTurnPlayer.ToUpper()}";
        matchCnt.text = $"{stageManager.matchCnt}";

    }

    public void IsAvailRange()
    {
        submitBtn.interactable = false;
        int inputValue = int.Parse(chipText.text);
        if (stageManager.turnCnt == 1 && inputValue >= playerBetMinRange && inputValue <= playerBetMaxRange)
        {
            submitBtn.interactable = true;
        }
        else if (inputValue > playerBetMinRange && inputValue <= playerBetMaxRange) //범위 내
        {
            submitBtn.interactable = true;
        }
    }

    public void IsOverAvailRange()
    {
        overSubmitBtn.interactable = false;
        int inputValue = int.Parse(overChipText.text);
        if (stageManager.turnCnt == 1 && inputValue >= playerBetMinRange && inputValue <= playerBetMaxRange)
        {
            overSubmitBtn.interactable = true;
        }
        else if (inputValue > playerBetMinRange && inputValue <= playerBetMaxRange) //범위 내
        {
            overSubmitBtn.interactable = true;
        }
    }

    public void ClickYesBtn()
    {
        stageManager.resetDeckBtn.GetComponent<Button>().interactable = true;
        stageManager.rndShuffleBtn.GetComponent<Button>().interactable = true;
        stageManager.changeCardBtn.GetComponent<Button>().interactable = true;
        stageManager.timeInfiniteBtn.GetComponent<Button>().interactable = true;
        Time.timeScale = 1f;
    }
    public void ClickNoBtn()
    {
        Time.timeScale = 1f;
    }
    public void PauseBtn()
    {
        Time.timeScale = 0;
    }

    public void DecideRange()
    {
        overBetChips = stageManager.overBetChip;
        //InputField 입력은 두가지 경우의 수 (플레이어 턴에만 유효)
        /*
         *  1. 선플레이어 == PL && 배팅 선택 =>(배팅 가능 범위) 1~ 플레이어 소지 칩
         *  2. 후플레이어 == PL && 오버배팅 =>(배팅 가능 범위) 이전 배팅 칩 ~ 플레이어 소지 칩
         */
        playerBetMaxRange = stageManager.playerChip; //maxrange 초기화

        if (stageManager.turnCnt == 1)
        {
            playerBetMinRange = 1;
            if (stageManager.enemyChip < stageManager.playerChip)
            {
                playerBetMaxRange = stageManager.enemyChip;
            }
            else
            {
                playerBetMaxRange = stageManager.playerChip;
            }
        }
        else
        {
            if (overBetChips == 0)
            {
                playerBetMinRange = stageManager.curBetChip;
            }
            else
            {
                playerBetMinRange = overBetChips;
            }

            if (stageManager.enemyChip < stageManager.playerChip)
            {
                if (overBetChips != 0)
                {
                    playerBetMaxRange = stageManager.enemyChip + overBetChips;
                }
                else
                {
                    if (stageManager.enemyChip + stageManager.curBetChip < playerBetMaxRange)
                    {
                        playerBetMaxRange = stageManager.enemyChip + stageManager.curBetChip;
                    }
                    else
                    {
                        playerBetMaxRange = stageManager.playerChip;
                    }
                }
            }
            else
            {
                playerBetMaxRange = stageManager.playerChip;
            }

        }
    }


}
