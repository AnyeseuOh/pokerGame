using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Example_Coroutine : MonoBehaviour
{
    public bool isMyTurn;
    public Button btn;

    void Start()
    {

        isMyTurn = true;
        StartCoroutine(Coroutine1());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Coroutine1 ()
    {
        Debug.Log("����");
        yield return Coroutine2();
        Debug.Log("���� ���� �Ϸ�.");
    }

    IEnumerator Coroutine2()
    {
        //yield return null;
        Debug.Log("���� �� ����.");
        yield return new WaitForSeconds(3.0f);
        Debug.Log("3�� ��� �Ϸ�");
    }

    public void OnBtnClick()
    {
        Debug.Log("�����");

    }
}
