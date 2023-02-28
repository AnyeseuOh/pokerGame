using UnityEngine;
using System.Collections;

public class DiceNumberRegister : MonoBehaviour 
{
    [SerializeField]
    int Number = -1;

    [SerializeField]//Left here in case you want to determine which dice is determining the value for debugging purposes
    string diceName = "";

    float distance = 10;
    RaycastHit hit;

    int layerMask = -1;

    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Ground");
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Ground")
        {
            Debug.DrawRay(transform.position, Vector3.down);

            if (Physics.SphereCast(transform.position, 0.5f, Vector3.down, out hit, 1, layerMask))
            {
                distance = hit.distance;
                //Debug.Log("distance: " + distance);
            }

            transform.parent.transform.parent.Find("Dice").GetComponent<Dice>().SetNumber(distance, Number);
        }
    }
}
