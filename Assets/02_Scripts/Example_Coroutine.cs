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
        Debug.Log("내턴");
        yield return Coroutine2();
        Debug.Log("내턴 실행 완료.");
    }

    IEnumerator Coroutine2()
    {
        //yield return null;
        Debug.Log("적의 턴 실행.");
        yield return new WaitForSeconds(3.0f);
        Debug.Log("3초 대기 완료");
    }

    public void OnBtnClick()
    {
        Debug.Log("계산중");

    }
}
