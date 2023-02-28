using UnityEngine;
using System.Collections;

public class TextLooker : MonoBehaviour 
{
    public Transform FollowObject;
    public int number = -1;
    public bool isShowing = false;

    void Start()
    {
        FollowObject = transform.parent.GetChild(0);
        transform.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = false;
    }

	// Update is called once per frame
	void Update () 
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
        transform.position = FollowObject.position + Vector3.forward * 0.4f;

        if (number != -1)
            transform.GetComponent<TextMesh>().text = number.ToString();

        if (!isShowing && number != -1)
        {
            isShowing = true;
            transform.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = true;
        }
        else if (isShowing && number == -1)
        {
            isShowing = false;
            transform.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = false;
        }
	}
}
