using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowScript : MonoBehaviour
{
    public string dataPath;
    public List<Dictionary<string, object>> data;
    public string curStageNum;

    public Text talkerText;
    public Text scriptText;

    public int i = 0;
    public int range;
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LoadDataFromCSV(i);
            i++;
        }
    }

    public void LoadDataFromCSV(int i)
    {
        data = CSVReader.Read(dataPath);
        
        if (i < data.Count)
        {
            if (data[i]["StageNum"].Equals(curStageNum))
            {
                talkerText.text = data[i]["Talker"].ToString();
                scriptText.text = data[i]["Script"].ToString();
                Debug.Log($"{data[i]["Talker"]} : {data[i]["Script"]}\n");
            }
        }
        else
        {
            Debug.Log("게임으로 넘어가기");
            //GameManager2.move
        }
    }
}
