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
     * �������� ������ ������, Ĩ �� ���� 
     * [0] -> enemy / [1]-> player
     * 
     */

}
