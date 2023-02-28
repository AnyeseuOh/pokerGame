using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour 
{
    public bool curState = false;
    public int number = 0;

    public float[] ColliderDistances;

	// Use this for initialization
	void Start () 
    {
        ColliderDistances = new float[21];
        for (int i = 0; i < ColliderDistances.Length; ++i)
        {
            ColliderDistances[i] = 100;
        }
	}

    public void SetNumber(float distance, int newNumber)
    {
        ColliderDistances[newNumber] = distance;

        if (Mathf.Min(ColliderDistances) == distance)
        {
            number = newNumber;
            transform.parent.Find("Status").GetComponent<TextLooker>().number = number;
        }
    }

    public void SetNumber(int newNumber)
    {
        number = newNumber;
        transform.parent.Find("Status").GetComponent<TextLooker>().number = number;
    }
}
