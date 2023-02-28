using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuffle : MonoBehaviour
{
    //셔플과 관련한 함수가 있다.
    /*
     * RandomShuffleStack(GameObject[] shuffleArr, int shuffleArrLength) : GameObject[]를 셔플한다.
     * 사용시기: 게임 최초 or 카드덱 폐기 아이템 사용
     * 
     * ItemRandomShuffle(Stack<GameObject> playCard, int playCardCnt) : 현재 남아있는 카드(스택)를 셔플한다.
     * 사용시기: 랜덤 셔플 아이템 사용
     * 
     * GiveCard(Stack<GameObject> playCard) : 가장 위에 있는 카드를 부여한다.
     * 사용시기: 승부 최초 [게임>승부>턴]
     */

    public GameObject[] initCards = new GameObject[40];
    public Stack<GameObject> playCard = new Stack<GameObject>();
    public GameObject currCard;

    void Start()
    {

        /*initCards = GameObject.FindGameObjectsWithTag("Card");

        StartCoroutine(RandomShuffleStack(initCards, initCards.Length, playCard));

        currCard = GiveCard(playCard);
        Debug.Log($"================Player's card : {currCard}");
        currCard = GiveCard(playCard);
        Debug.Log($"================Enemy's card : {currCard}");*/

        //ItemRandomShuffle(playCard);
    }


    void Update()
    {

    }


    public IEnumerator RandomShuffleStack(GameObject[] initCards, int length, Stack<GameObject> playCard)
    {
        int idxCnt = 0;
        int choiceIdx;
        GameObject choiceCard;

        while (idxCnt < length)
        {
            choiceIdx = Random.Range(0, length); //선택된 인덱스
            choiceCard = initCards[choiceIdx]; //선택된 인덱스의 카드 값을 String 변수에 넣어준다.

            //Debug.Log($"Choose One : {choiceCard}"); //어느 카드가 선택되었는지 확인
            if (!playCard.Contains(choiceCard)) //선택된 카드가 shuffleCardString에 존재하지 않는다면
            {
                playCard.Push(choiceCard);
                //Debug.Log($"playCard.Push(choiceCard) : {choiceCard.ToString()} , INDEX CNT : {idxCnt}");
                idxCnt++;
            }
            else
            {
                continue;
            }
        }

        yield return new WaitForSeconds(3f);
    }

    public Stack<GameObject> ItemRandomShuffle(Stack<GameObject> playCard)
    {
        int idxCnt = 0;
        int choiceIdx;
        GameObject choiceCard;
        GameObject[] tempArr = playCard.ToArray();
        Stack<GameObject> tempCard = new Stack<GameObject>();

        Debug.Log("======================================");
        while (idxCnt < playCard.Count)
        {
            choiceIdx = Random.Range(0, playCard.Count); //선택된 인덱스
            choiceCard = tempArr[choiceIdx]; //선택된 인덱스의 카드 값을 String 변수에 넣어준다.

            //Debug.Log($"Choose One : {choiceCard}"); //어느 카드가 선택되었는지 확인
            if (!tempCard.Contains(choiceCard)) //선택된 카드가 shuffleCardString에 존재하지 않는다면
            {
                tempCard.Push(choiceCard);
                //Debug.Log($"ItemRandom : {choiceCard.ToString()} , INDEX CNT : {idxCnt}");
                idxCnt++;
            }
            else
            {
                continue;
            }
        }
        return tempCard;
    }

    /*
     * 함수 설명
     * 저장된 스택에서 가장 위에 있는 카드를 꺼낸다.
     */
    public GameObject GiveCard(Stack<GameObject> playCard)
    {
        GameObject popCard;
        popCard = playCard.Pop();

        return popCard;
    } 
}


