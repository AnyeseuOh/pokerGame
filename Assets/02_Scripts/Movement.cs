using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject dice;
    public float x, y, z;
    public float seconds;
    void Start()
    {
        //dice = GameObject.Find("d10").GetComponent<GameObject>();
    }

    
    void Update()
    {
        x = Random.Range(-12, 12);
        y = Random.Range(-12, 12);
        z = Random.Range(-12, 12);
        dice.transform.Rotate(x * Time.deltaTime * seconds, y * Time.deltaTime * seconds, z * Time.deltaTime * seconds);
    }

    public void CardTranslate(GameObject card, Vector3 playerPos)
    {
        //���õ� ī�尡 �÷��̾��� ��ġ�� �̵�
    }
}
