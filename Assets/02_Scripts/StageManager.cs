using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public Dice dice;
    public Item item;
    public GameObject dicePrefab;
    public Movement movement;

    public GameObject enemyCard;
    public GameObject plCard;
    public GameObject[] initCards = new GameObject[40];
    public Stack<GameObject> playCard = new Stack<GameObject>();

    public GameObject[] diceReseult;
    public GameObject[] diceReseultEn;
    public GameObject firstDiceWin;
    public GameObject firstDiceLose;

    public GameObject playerBetPanel;
    public GameObject playerBetPanel_2;
    public GameObject bettingPanel;
    public GameObject useItemPanel;
    public GameObject gameOverPanelWin;
    public GameObject gameOverPanelLose;
    public GameObject enemyTurnPanel;

    public InputField chipValue;
    public InputField overChipValue;
    public Text winText;
    public Text loseText;
    public Text enemyState;

    public GameObject plCardImg;
    public GameObject enCardImg;
    public Sprite[] cardImgs;
    public Image plRCard;
    public Image enRCard;

    public Button resetDeckBtn;
    public Button rndShuffleBtn;
    public Button changeCardBtn;
    public Button timeInfiniteBtn;

    public GameObject win;
    public GameObject lose;
    public GameObject draw;

    public GameObject lightEffectPrefab;
    public GameObject hitEffectPrefab;

    public string curTurnPlayer;
    public string curEnemyBetting;
    public Timer timer;
    public Shuffle shuffle;
    public UIManager uiManager;
    public Animator anim;
    int result;

    public int playerChip;
    public int enemyChip;
    public int publicChip;
    public int bettingChip;
    public int curBetChip;
    public int prvBetChip;
    public int overBetChip;
    public int choice;
    public int turnCnt = 0;
    public int matchCnt = 0;
    public int playerDice;
    public int enemyDice;
    public int gameResult = -1;
    public float turnTime = 60.0f;
    int cardNum;
    int enCardNum;
    int plCardNum;

    public enum MatchState
    {
        Idle,
        win,
        lose
    }
    public MatchState matchState;

    public bool isTurnOver = false;
    public bool isMatchOver = false;
    public bool isGameOver = false;
    public bool isClick = false;
    public bool isDraw = false;
    public bool isPlOverBet = false;
    public bool isEnemyTurn = false;

    public string dataPath;
    public List<Dictionary<string, object>> data;

    IEnumerator Start()
    {
        //MatchState.Idle;
        
        //임시로 숫자 부여
        enemyChip = 20;
        playerChip = 20;

        data = CSVReader.Read(dataPath);
        int stageNum = int.Parse(PlayerPrefs.GetString("CUR_STAGE").Substring(7)); //stage1-2 -> 2
        enemyChip = int.Parse(data[stageNum]["chip_EN"].ToString());
        playerChip = int.Parse(data[stageNum]["chip_PL"].ToString());

        Debug.Log($"enChip :: {enemyChip}\nplChip :: {playerChip}");

        /*item.resetDeck = 2;
        item.rndShuffle = 2;
        item.changeCard = 1;
        item.timeInfinite = 2;*/

        //버튼 비활성화
        resetDeckBtn.GetComponent<Button>().interactable = false;
        rndShuffleBtn.GetComponent<Button>().interactable = false;
        changeCardBtn.GetComponent<Button>().interactable = false;
        timeInfiniteBtn.GetComponent<Button>().interactable = false;

        gameOverPanelWin.SetActive(false);
        gameOverPanelLose.SetActive(false);


        //선공 후공 다이스
        DecideFirst();
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        GameObject dice = Instantiate(dicePrefab, new Vector3(-0.3f, 1.2f, 0), new Quaternion(0, 0, 0, 0));
        yield return new WaitForSeconds(3f);
        Destroy(dice);

        if (result == 0)
        {
            firstDiceLose.SetActive(true);
        } else
        {
            firstDiceWin.SetActive(true);
        }

        ShowDiceNum(diceReseult, playerDice);
        ShowDiceNum(diceReseultEn, enemyDice);
        yield return new WaitForSeconds(3f);
        diceReseult[playerDice].SetActive(false);
        diceReseultEn[enemyDice].SetActive(false);

        if (result == 0)
        {
            firstDiceLose.SetActive(false);
        }
        else
        {
            firstDiceWin.SetActive(false);
        }

        //최초 셔플
        initCards = GameObject.FindGameObjectsWithTag("Card");
        StartCoroutine(shuffle.RandomShuffleStack(initCards, initCards.Length, playCard));

        // Match -----------------------
        while (isGameOver == false) //게임이 끝나지 않았다면
        {

            if (playCard.Count == 0)
            {
                shuffle.RandomShuffleStack(initCards, initCards.Length, playCard);
            }

            Debug.Log("Match 시작");
            matchCnt++;

            turnCnt = 1; //턴 초기화
            isMatchOver = false;
            isTurnOver = false;

            curBetChip = 0;
            prvBetChip = 0;
            overBetChip = 0;

            // 승부 전 공동 배팅
            playerChip -= 1;
            enemyChip -= 1;
            bettingChip += 2;

            //카드 부여
            enemyCard = shuffle.GiveCard(playCard);

            enemyCard = Instantiate(enemyCard, new Vector3(-0.15f, 0, -2.5f), new Quaternion(0, 0, 0, 0));
            enemyCard.transform.localRotation = new Quaternion(0, -0.1f, 0, 0.99f);
            enemyCard.name = enemyCard.name.Replace("(Clone)", "");

            plCard = shuffle.GiveCard(playCard);
            plCard = Instantiate(plCard, new Vector3(-0.98f, 1.4f, 1f), new Quaternion(0, 0.94f, 0, 0.34f));
            plCard.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
            plCard.name = plCard.name.Replace("(Clone)", "");

            Debug.Log($"Enemy Card ::: {enemyCard.ToString()}");
            Debug.Log($"Player Card ::: {plCard.ToString()}");

            enCardNum = int.Parse(Regex.Replace(enemyCard.ToString(), @"[^0-9]", ""));
            plCardNum = int.Parse(Regex.Replace(plCard.ToString(), @"[^0-9]", ""));

            Debug.Log("=============Match=============");
            PrintChips();

            if (playerChip == 0 || enemyChip == 0)
            {
                yield return StartCoroutine(LastCompare(enCardNum, plCardNum));
            }

            //최초 턴플레이어 표시
            if (result == 0) { 
                curTurnPlayer = "Enemy";
                anim.SetBool("isEnemyTurn", true);
                enemyTurnPanel.SetActive(true);
                enemyState.text = "베팅 중 . . .";
            }
            else if (result == 1) {
                curTurnPlayer = "Player";
                anim.SetBool("isEnemyTurn", false);
                enemyTurnPanel.SetActive(false);
            }

            while (isMatchOver == false) //MatchOver가 아니라면
            {
                plCardImg.SetActive(false);
                enCardImg.SetActive(false);

                Debug.Log($"TURN CNT : {turnCnt}");
                yield return StartCoroutine(TurnFlow());

                enemyTurnPanel.SetActive(false);
                enemyState.text = "베팅 중 . . ."; //초기화

                plCardImg.SetActive(false);
                enCardImg.SetActive(false);
                //카드 결과 OFF

                win.SetActive(false);
                lose.SetActive(false);
                draw.SetActive(false);
                chipValue.text = null;
                overChipValue.text = null;

                yield return new WaitForSeconds(1.0f);
                matchState = MatchState.Idle;
                Debug.Log("TurnFlow 후 isTurnOver 진행");
                isTurnOver = true;
                yield return null;
            }

            Destroy(enemyCard.gameObject);
            Destroy(plCard.gameObject);

            if (enemyChip == 0 || playerChip == 0)
            {
                isGameOver = true;
            }
        }

        Debug.Log("Game OVER");
        yield return null;
        //게임 종료 panel과 승패 
        if (gameResult == 0)
        {
            ShowWinResult();
            Debug.Log("승리");
            gameOverPanelWin.SetActive(true);
        }
        else
        {
            ShowLoseResult();
            Debug.Log("패배");
            gameOverPanelLose.SetActive(true);
        }

    }


    void Update()
    {
        switch (matchState)
        {
            case MatchState.Idle:
                anim.SetInteger("result", 0);
                break;

            case MatchState.win:
                anim.SetInteger("result", 1);
                break;

            case MatchState.lose:
                anim.SetInteger("result", 2);
                break;
        }

        if (isGameOver)
        {
            bettingChip = 0;
            curBetChip = 0;
            prvBetChip = 0;
            overBetChip = 0;

            if (playerChip > enemyChip)
            {
                gameResult = 0; //0이 플레이어 승리
                if (PlayerPrefs.GetString("CUR_STAGE").Equals("stage1-1"))
                {
                    PlayerPrefs.SetString("IS_CLEAR_1", "Y");
                }
                else if (PlayerPrefs.GetString("CUR_STAGE").Equals("stage1-2"))
                {
                    PlayerPrefs.SetString("IS_CLEAR_2", "Y");
                }
                else if (PlayerPrefs.GetString("CUR_STAGE").Equals("stage1-3"))
                {
                    PlayerPrefs.SetString("IS_CLEAR_3", "Y");
                }
                else if (PlayerPrefs.GetString("CUR_STAGE").Equals("stage1-4"))
                {
                    PlayerPrefs.SetString("IS_CLEAR_4", "Y");
                }
                else
                {
                    PlayerPrefs.SetString("IS_CLEAR_5", "Y");
                }

            }
            else
            {
                gameResult = 1;
            }
        }

        if (isMatchOver)
        {
            turnCnt = 1;
        }

        if (isTurnOver && enemyChip != 0 && playerChip !=0)
        {
            Debug.Log($"isTurnOver ::: 현재턴 {curTurnPlayer}");
            turnCnt++; //턴증가
            if (curTurnPlayer == "Enemy")
            {
                curTurnPlayer = "Player";
                anim.SetBool("isEnemyTurn", false);
            }
            else
            {
                curTurnPlayer = "Enemy";
                anim.SetBool("isEnemyTurn", true);
            }
            Debug.Log($"isTurnOver ::: 변경턴 {curTurnPlayer}");
            isTurnOver = false;
        }

    }

    IEnumerator TurnFlow()
    {
        //턴 시작 / 현재 턴플레이어 표시
        Debug.Log($"TurnFlow 최초 Current Turn Player ::: {curTurnPlayer}");
        PrintChips();

        if (playerChip <= 0 || enemyChip <= 0)
        {
            ShowCard(enemyCard, plCard);
            yield return StartCoroutine(LastCompare(enCardNum, plCardNum));
            yield return null;
        }
        else
        {
            if (curTurnPlayer == "Player") //현재 턴플레이어가 플레이어라면 버튼 활성화
            {
                uiManager.DecideRange();
                useItemPanel.SetActive(true);
                //아이템버튼 활성화
                Time.timeScale = 0;

                if (turnCnt == 1) { playerBetPanel.SetActive(true); }
                else { playerBetPanel_2.SetActive(true); }

                //클릭 전까지 대기
                yield return StartCoroutine(ReturnIsBtnClick());

                yield return new WaitForSeconds(5f);
            }
            else //curTurnPlayer == "Enemy"
            {
                enemyTurnPanel.SetActive(true);
                yield return new WaitForSeconds(5f);
                Debug.Log("3초 대기1");
                if (turnCnt == 1)
                {
                    //배팅 or 포기
                    int bettingPer = (10 - int.Parse(Regex.Replace(plCard.ToString(), @"[^0-9]", ""))) * 10;
                    //N0%퍼의 확률로 배팅 (100-N0)퍼의 확률로 포기

                    int rndBetNum = Random.Range(0, 100);
                    Debug.Log($"rndBetNum : {rndBetNum}, bettingPer : {bettingPer + 10}");

                    if (rndBetNum < bettingPer + 10)
                    {
                        yield return StartCoroutine(NormalBetting());
                    }
                    else
                    {
                        //포기
                        Debug.Log("::: DIE :::");
                        yield return new WaitForSeconds(3.0f);
                        yield return StartCoroutine(DieBetting());
                    }
                }
                else //오버배팅하거나, 동률배팅하거나 포기하거나
                {
                    //선택지 확률부터
                    int choice = Random.Range(0, 3);
                    PrintChips();
                    if (overBetChip == enemyChip || curBetChip == enemyChip)
                    {
                        choice = Random.Range(1, 3);
                        Debug.Log($"선택 변경 overbetting -> {choice}");
                    }

                    Debug.Log($"CHOICE ::: {choice}");
                    if (choice == 0) // overBetting
                    {
                        Debug.Log("::: OverBetting :::");
                        yield return StartCoroutine(ReturnOverBetting());

                    }
                    else if (choice == 1) //SameBetting
                    {
                        Debug.Log("::: SameBetting :::");
                        Debug.Log($"CHIP BETTING :: {overBetChip}");
                        yield return StartCoroutine(ReturnSameBetting());
                    }
                    else //Die
                    {
                        //포기
                        Debug.Log("::: DIE :::");
                        yield return StartCoroutine(DieBetting());
                        PrintChips();
                    }
                }
                Debug.Log("1초 대기2");
                yield return new WaitForSeconds(1f);
            }
        }

        yield return new WaitForSeconds(3.0f);

        if (playerChip == 0 || enemyChip == 0)
        {
            isMatchOver = true;
            yield return null;
        }
    }


    void DecideFirst()
    {
        enemyDice = Random.Range(0, 10);
        playerDice = Random.Range(0, 10);


        if (enemyDice > playerDice)
        {
            result = 0; //0를 리턴하면 적플레이어가 선공
            Debug.Log($"EDice : {enemyDice} / PDice : {playerDice}\n === result : Enemy First ===");
        }
        else if (enemyDice < playerDice)
        {
            result = 1;
            Debug.Log($"EDice : {enemyDice} / PDice : {playerDice}\n === result : Player First ===");
        } else
        {
            Debug.Log($"EDice : {enemyDice} / PDice : {playerDice}\n === result : REMATCH ===");
            DecideFirst();
        }
    }

    void BattleCard(GameObject enemyCard, GameObject playerCard)
    {
        int enemyCardNum = int.Parse(Regex.Replace(enemyCard.ToString(), @"[^0-9]", ""));
        int playerCardNum = int.Parse(Regex.Replace(playerCard.ToString(), @"[^0-9]", ""));

        Debug.Log($"BattleCard 진입 :: en -> {enemyCardNum} / pl -> {playerCardNum}");

        if (enemyCardNum > playerCardNum)
        {
            enemyChip = ComputeChip(enemyChip, bettingChip);
            isTurnOver = true;
            Debug.Log($"=============== [BT] Enemy WIN! ================");
            matchState = MatchState.lose;
            enemyTurnPanel.SetActive(false);
            lose.SetActive(true);
            HitPlayerEffect();

            PrintChips();
            bettingChip = 0;
        }
        else if (enemyCardNum < playerCardNum)
        {
            playerChip = ComputeChip(playerChip, bettingChip);
            isTurnOver = true;
            Debug.Log($"=============== [BT] Player WIN! ================");
            matchState = MatchState.win;
            enemyTurnPanel.SetActive(false);
            win.SetActive(true);
            HitEnemyEffect();
            PrintChips();
            bettingChip = 0;
        }
        else
        {
            isTurnOver = true;
            Debug.Log($"=============== [BT] DRAW! ================");
            isDraw = true;
            enemyTurnPanel.SetActive(false);
            draw.SetActive(true);
            PrintChips();
        }
    }

    IEnumerator LastCompare(int enCardNum, int plCardNum)
    {
        if (playerChip == 0)
        {
            if (enCardNum > plCardNum)
            {
                matchState = MatchState.lose;
                enemyChip += bettingChip;
                isGameOver = true;
            }
            else if (plCardNum > enCardNum)
            {
                matchState = MatchState.win;
                playerChip = bettingChip;
                bettingChip = 0;
                enemyTurnPanel.SetActive(false);
                win.SetActive(true);
                HitEnemyEffect();
                PrintChips();
                isMatchOver = true;
            }
            else
            {
                //무승부시 카드 부여
                enemyCard = shuffle.GiveCard(playCard);
                enemyCard = Instantiate(enemyCard, new Vector3(-0.15f, 0, -2.5f), new Quaternion(0, -17f, 0, 0));
                enemyCard.transform.localRotation = new Quaternion(0, -0.1f, 0, 0.99f);
                enemyCard.name = enemyCard.name.Replace("(Clone)", "");

                plCard = shuffle.GiveCard(playCard);
                plCard = Instantiate(plCard, new Vector3(-0.98f, 1.4f, 1f), new Quaternion(0, 0.94f, 0, 0.34f));
                plCard.name = plCard.name.Replace("(Clone)", "");

                Debug.Log($"Enemy Card ::: {enemyCard.ToString()}");
                Debug.Log($"Player Card ::: {plCard.ToString()}");

                enCardNum = int.Parse(Regex.Replace(enemyCard.ToString(), @"[^0-9]", ""));
                plCardNum = int.Parse(Regex.Replace(plCard.ToString(), @"[^0-9]", ""));
                yield return StartCoroutine(LastCompare(enCardNum, plCardNum));
            }
            yield return null;
        }

        else
        {
            if (plCardNum > enCardNum)
            {
                matchState = MatchState.win;
                playerChip += bettingChip;
                isGameOver = true;
            }
            else if (enCardNum > plCardNum)
            {
                matchState = MatchState.lose;
                enemyChip = bettingChip;
                bettingChip = 0;
                enemyTurnPanel.SetActive(false);
                lose.SetActive(true);
                HitPlayerEffect();

                isMatchOver = true;
            } 
            else
            {
                //무승부시 카드 부여
                enemyCard = shuffle.GiveCard(playCard);
                enemyCard = Instantiate(enemyCard, new Vector3(-0.15f, 0, -2.5f), new Quaternion(0, -17f, 0, 0));
                enemyCard.transform.localRotation = new Quaternion(0, -0.1f, 0, 0.99f);
                enemyCard.name = enemyCard.name.Replace("(Clone)", "");

                plCard = shuffle.GiveCard(playCard);
                plCard = Instantiate(plCard, new Vector3(-0.9f, 1.2f, 0.9f), new Quaternion(0, 0.9f, 0, 0.4f));
                plCard.name = plCard.name.Substring(plCard.name.Length - 7);

                Debug.Log($"Enemy Card ::: {enemyCard.ToString()}");
                Debug.Log($"Player Card ::: {plCard.ToString()}");

                enCardNum = int.Parse(Regex.Replace(enemyCard.ToString(), @"[^0-9]", ""));
                plCardNum = int.Parse(Regex.Replace(plCard.ToString(), @"[^0-9]", ""));
                yield return StartCoroutine(LastCompare(enCardNum, plCardNum));
            }
            yield return null;
        }
    }

    public void SubmitChipClick()
    {
        isClick = true;
        StartCoroutine(ReturnLoadChip());
        Debug.Log($"SubmitClick");
        PrintChips();
    }

    IEnumerator ReturnLoadChip()
    {
        if (isPlOverBet == true)
        {
            yield return StartCoroutine(OverLoadChipValue());
        }
        else
        {
            yield return StartCoroutine(LoadChipValue());
            yield return StartCoroutine(NormalBetting());
        }
        
    }

    IEnumerator LoadChipValue()
    {
        if (turnCnt != 1)
        {
            prvBetChip = curBetChip;
            curBetChip = int.Parse(chipValue.text);
            overBetChip = curBetChip - prvBetChip;
        }
        else //turnCnt == 1
        {
            curBetChip = int.Parse(chipValue.text);
        }

        Debug.Log("=============LoadChipValue=============");
        PrintChips();
        yield return null;
    }

    IEnumerator OverLoadChipValue()
    {
        prvBetChip = curBetChip;
        curBetChip = int.Parse(overChipValue.text);
        yield return null;
    }

    IEnumerator NormalBetting()
    {
        if (curTurnPlayer == "Enemy")
        {
            enemyState.text = "베팅 중 . . .";
            int max = enemyChip;
            if (max > playerChip)
            {
                max = playerChip;
            }
            int rndChip = Random.Range(1, max + 1);
            curBetChip = rndChip;
            enemyChip -= rndChip;
            bettingChip += curBetChip;
            //prvBetChip = curBetChip;
            Debug.Log("=============NormalBetting=============");
            Debug.Log($"CURBET ::: {curBetChip} / max ::: {max}");
            PrintChips();
        } else
        {
            playerChip -= curBetChip;
            Debug.Log($"Player BETTING CHIP VALUE ::: {curBetChip}");
            bettingChip += curBetChip;
        }
        yield return null;
    }

    public void IsOverBetBtnClick()
    {
        isClick = true;
        isPlOverBet = true;
        Debug.Log("::: Player Betting :::");
        Debug.Log($"Current Turn Player ::: {curTurnPlayer}");
        StartCoroutine(ReturnOverBetting());
    }

    IEnumerator ReturnOverBetting()
    {
        if (isPlOverBet == true)
        {
            yield return StartCoroutine(ReturnLoadChip());
        }
        Debug.Log("오버배팅 시작 - ReturnOverBetting()");
        yield return StartCoroutine(OverBetting());
        isPlOverBet = false;
        yield return null;
    }

    IEnumerator OverBetting()
    {
        //적일 경우
        if (curTurnPlayer == "Enemy")
        {
            enemyState.text = "초과 배팅";
            yield return new WaitForSeconds(3.0f);
            if (prvBetChip == 0)
            {
                prvBetChip = curBetChip;
            }
            else
            {
                prvBetChip = overBetChip;
            }

            int minChip = enemyChip - prvBetChip;
            if (minChip > playerChip)
            {
                minChip = playerChip;
            }

            overBetChip = Random.Range(1, minChip + 1);
            Debug.Log($"OverBetChip ::: {overBetChip}");
            //최소 범위; 이전 배팅 수 ~ 최대범위 : 최대 소지수
            curBetChip = prvBetChip + overBetChip;
            overBetChip = curBetChip - prvBetChip;

            enemyChip -= curBetChip;
            bettingChip += curBetChip;

            Debug.Log("=============EN OverBetting=============");
            PrintChips();
            yield return new WaitForSeconds(2.0f);
        }

        //플레이어일 경우
        else
        {
            if (overBetChip != 0)
            {
                overBetChip = curBetChip - overBetChip;
            }
            else
            {
                overBetChip = curBetChip - prvBetChip;
            }

            playerChip -= curBetChip;
            Debug.Log($"Player BETTING CHIP VALUE ::: {curBetChip}");
            bettingChip += curBetChip;

            Debug.Log("=============OverBetting=============");
            PrintChips();
        }
        yield return null;
    }

    public void IsSameBetBtnClick()
    {
        isClick = true;
        Debug.Log("::: Player Betting :::");
        Debug.Log($"Current Turn Player ::: {curTurnPlayer}");
        StartCoroutine(ReturnSameBetting());
    }

    IEnumerator ReturnSameBetting()
    {
        yield return StartCoroutine(SameBetting());
    }

    IEnumerator SameBetting()
    {
        //클릭 전까지 대기
        Debug.Log($"Samebetting 현재 턴플레이어 ::: {curTurnPlayer}");
        PrintChips();
        if (overBetChip != 0)
        {
            curBetChip = overBetChip;
        }

        if (curTurnPlayer == "Player")
        {
            yield return StartCoroutine(ReturnIsBtnClick());
            playerChip -= curBetChip;
        }
        else if (curTurnPlayer == "Enemy")
        {
            enemyState.text = "동률배팅";
            yield return new WaitForSeconds(3.0f);
            enemyChip -= curBetChip;
            /*if (overBetChip != 0)
            {
                enemyChip -= overBetChip;
            } else
            {
                enemyChip -= curBetChip;
            }*/

        }
        bettingChip += curBetChip;

        enemyTurnPanel.SetActive(false);
        ShowCard(enemyCard, plCard); //카드 결과 보여주기
        yield return new WaitForSeconds(5f);
        plCardImg.SetActive(false);
        enCardImg.SetActive(false);
        //카드 결과 OFF

        Debug.Log("Samebetting BattleCard 진입전");
        BattleCard(enemyCard, plCard); //카드배틀 
        isMatchOver = true; //동률일 때는 무조건 승부 종료
        yield return null;

        if (isDraw)
        {
            Debug.Log("Draw if문 진입");
            curBetChip = 0;
            prvBetChip = 0;
            overBetChip = 0;
            isMatchOver = true;
            yield return null;
        }
        
        else
        {
            bettingChip = 0;
            Debug.Log("=============SameBetting=============");
            PrintChips();
            yield return new WaitForSeconds(3.0f);
        }
        
    }

    public void IsDieBtnClick()
    {
        isClick = true;
        Debug.Log("::: Player DIE :::");
        Debug.Log($"Current Turn Player ::: {curTurnPlayer}");
        StartCoroutine(ReturnDieBetting());
    }

    IEnumerator ReturnDieBetting()
    {
        yield return StartCoroutine(DieBetting());
    }

    IEnumerator DieBetting()
    {
        if (curTurnPlayer == "Enemy")
        {
            enemyState.text = "포기";
            yield return new WaitForSeconds(3.0f);
            cardNum = int.Parse(Regex.Replace(enemyCard.ToString(), @"[^0-9]", ""));
            //enemyCard에서 숫자만 추출해서 10인지 확인
            if (cardNum == 10)
            {
                if ((enemyChip - 10) <= 0)
                {
                    bettingChip += enemyChip;
                    enemyChip = 0;
                    playerChip = ComputeChip(playerChip, bettingChip);//승리한 플레이어가 배팅칩을 가져감
                    isGameOver = true;
                }
                else
                {
                    enemyChip -= 10; //패널티 부여
                    bettingChip += 10;
                    playerChip = ComputeChip(playerChip, bettingChip);//승리한 플레이어가 배팅칩을 가져감
                }
                
            }
            else
            {
                playerChip = ComputeChip(playerChip, bettingChip);//승리한 플레이어가 배팅칩을 가져감
            }

            bettingChip = 0;
            PrintChips();

            enemyTurnPanel.SetActive(false);
            ShowCard(enemyCard, plCard);
            yield return new WaitForSeconds(5.0f);
            plCardImg.SetActive(false);
            enCardImg.SetActive(false);
            //카드 결과 OFF

            Debug.Log($"=============== Player WIN! ================");
            matchState = MatchState.win;
            win.SetActive(true);
            HitEnemyEffect();
        }

        else
        {
            cardNum = int.Parse(Regex.Replace(plCard.ToString(), @"[^0-9]", ""));
            //enemyCard에서 숫자만 추출해서 10인지 확인
            Debug.Log($"PLAYER Card Num ::: {cardNum}");
            if (cardNum == 10)
            {
                if ((playerChip - 10) <= 0)
                {
                    bettingChip += playerChip;
                    playerChip = 0;
                    enemyChip = ComputeChip(enemyChip, bettingChip); //승리한 적플레이어가 배팅칩을 가져감
                    isGameOver = true;
                }
                else
                {
                    playerChip -= 10; //패널티 부여
                    bettingChip += 10;
                    enemyChip = ComputeChip(enemyChip, bettingChip); //승리한 적플레이어가 배팅칩을 가져감
                }
            }
            else
            {
                Debug.Log("PL die Betting 진입");
                enemyChip = ComputeChip(enemyChip, bettingChip); //승리한 적플레이어가 배팅칩을 가져감
            }

            bettingChip = 0;
            PrintChips();

            enemyTurnPanel.SetActive(false);
            ShowCard(enemyCard, plCard);
            yield return new WaitForSeconds(5.0f);
            plCardImg.SetActive(false);
            enCardImg.SetActive(false);
            //카드 결과 OFF

            Debug.Log($"=============== Enemy WIN! ================");
            matchState = MatchState.lose;
            lose.SetActive(true);
            HitPlayerEffect();
        }
        isMatchOver = true;
        
        yield return null;
        yield return new WaitForSeconds(3.0f);
    }

    public int ComputeChip(int vChip, int betChip)
    {
        return vChip += betChip;
    }


    IEnumerator ReturnIsBtnClick()
    {
        yield return new WaitUntil(() => isClick == true);
        isClick = false;
    }

    public void PrintChips()
    {
        Debug.Log(  $"CURRENT Betting CHIPS ::: {curBetChip}\nPLAYER CURRENT CHIPS ::: {playerChip}\nENEMY CURRENT CHIPS ::: {enemyChip}\nTotal Betting CHIPS ::: {bettingChip}\nprevent Betting CHIPS ::: {prvBetChip}\nover Betting CHIPS ::: {overBetChip}\n");
    }

    public void RndShuffleItem()
    {
        shuffle.ItemRandomShuffle(playCard);
        Debug.Log("========================");
        /*foreach (GameObject i in playCard)
        {
            Debug.Log($"playCard :: {i.ToString()}");
        }*/
        Debug.Log("========================");
        Debug.Log($"Count : {playCard.Count}");
    }

    public void DiscardDeck()
    {
        playCard.Clear();
        StartCoroutine(shuffle.RandomShuffleStack(initCards, initCards.Length, playCard));
        Debug.Log("========================");
        /*foreach (GameObject i in playCard)
        {
            Debug.Log($"playCard :: {i.ToString()}");
        }*/
        Debug.Log("========================");
        Debug.Log($"Count : {playCard.Count}");
    }

    public void ChangeEnCard()
    {
        Destroy(enemyCard);
        enemyCard = shuffle.GiveCard(playCard);
        enemyCard = Instantiate(enemyCard, new Vector3(-0.15f, 0, -2.5f), new Quaternion(0, -17f, 0, 0));
        enemyCard.transform.localRotation = new Quaternion(0, -0.1f, 0, 0.99f);
        enemyCard.name = enemyCard.name.Replace("(Clone)", "");
    }

    public void ShowDiceNum(GameObject[] objs, int num)
    {
        objs[num].SetActive(true);
    }

    public void ShowWinResult()
    {
        winText.text = $"승부 수 : {matchCnt}" +
            $"\n플레이어 칩 개수: {playerChip}" +
            $"\n적 칩 개수: {enemyChip}";
            //+$"\n보상: 아이템 개수";
    }

    public void ShowLoseResult()
    {
        loseText.text = $"승부 수 : {matchCnt}" +
            $"\n플레이어 칩 개수: {playerChip}" +
            $"\n적 칩 개수: {enemyChip}";
    }

    public void ShowCard(GameObject enemyCard, GameObject playerCard)
    {
        plCardImg.SetActive(true);
        enCardImg.SetActive(true);

        foreach (Sprite spCard in cardImgs)
        {
            if(string.Equals(spCard.name, enemyCard.name))
            {
                enRCard.sprite = spCard;
            }

            if (string.Equals(spCard.name, playerCard.name))
            {
                plRCard.sprite = spCard;
            }

        }
    }

    public void HitPlayerEffect()
    {
        GameObject hitEffect = Instantiate(hitEffectPrefab, new Vector3(-0.65f, 0.3f, -0.08f), new Quaternion(0, 0, 0, 1f));
        Destroy(hitEffect.gameObject, 3f);
    }

    public void HitEnemyEffect()
    {
        GameObject lightEffect = Instantiate(lightEffectPrefab, new Vector3(-0.57f, 0.8f, 0.56f), new Quaternion(0, 0, 0, 1f));
        Destroy(lightEffect.gameObject, 3f);
    }
}
