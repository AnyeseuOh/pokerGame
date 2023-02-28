using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    //public Text timeText;
    public float time;
    public bool isOver = false;

    private void Awake()
    {
        time = 10f;
    }

    private void Start()
    {
        isOver = false;
    }

    private void Update()
    {
        if (!isOver)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                Debug.Log($"TIME ::: {Mathf.Ceil(time).ToString()}");
                //timeText.text = Mathf.Ceil(time).ToString();
            }
            else if (Mathf.Ceil(time) == 0)
            {
                Debug.Log("TIME OVER ::: TURN CHANGED");
                isOver = true;
            }
            
        }


    }
}
