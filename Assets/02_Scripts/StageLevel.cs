using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class StageLevel : MonoBehaviour
{
    [Serializable]
    public struct Item
    {
        public int discardDeck;
        public int randomShuffle;
        public int otherPCDiscard;
        public int turnTimeInfinite;
        public int chip;
    }
    public Item[] stageItem = new Item[2];

    /*
     * 스테이지 아이템 개수와, 칩 수 저장 
     * [0] -> enemy / [1]-> player
     * 
     */

}
