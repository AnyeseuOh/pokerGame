using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuffle : MonoBehaviour
{
    //���ð� ������ �Լ��� �ִ�.
    /*
     * RandomShuffleStack(GameObject[] shuffleArr, int shuffleArrLength) : GameObject[]�� �����Ѵ�.
     * ���ñ�: ���� ���� or ī�嵦 ��� ������ ���
     * 
     * ItemRandomShuffle(Stack<GameObject> playCard, int playCardCnt) : ���� �����ִ� ī��(����)�� �����Ѵ�.
     * ���ñ�: ���� ���� ������ ���
     * 
     * GiveCard(Stack<GameObject> playCard) : ���� ���� �ִ� ī�带 �ο��Ѵ�.
     * ���ñ�: �º� ���� [����>�º�>��]
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
            choiceIdx = Random.Range(0, length); //���õ� �ε���
            choiceCard = initCards[choiceIdx]; //���õ� �ε����� ī�� ���� String ������ �־��ش�.

            //Debug.Log($"Choose One : {choiceCard}"); //��� ī�尡 ���õǾ����� Ȯ��
            if (!playCard.Contains(choiceCard)) //���õ� ī�尡 shuffleCardString�� �������� �ʴ´ٸ�
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
            choiceIdx = Random.Range(0, playCard.Count); //���õ� �ε���
            choiceCard = tempArr[choiceIdx]; //���õ� �ε����� ī�� ���� String ������ �־��ش�.

            //Debug.Log($"Choose One : {choiceCard}"); //��� ī�尡 ���õǾ����� Ȯ��
            if (!tempCard.Contains(choiceCard)) //���õ� ī�尡 shuffleCardString�� �������� �ʴ´ٸ�
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
     * �Լ� ����
     * ����� ���ÿ��� ���� ���� �ִ� ī�带 ������.
     */
    public GameObject GiveCard(Stack<GameObject> playCard)
    {
        GameObject popCard;
        popCard = playCard.Pop();

        return popCard;
    } 
}


