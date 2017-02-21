using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Photon.MonoBehaviour
{

    public GameObject target;
    public bool GhostOnly;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if (!GhostOnly || obj.tag == "Ghost")
        {
            obj.transform.position = new Vector3(target.transform.position.x, obj.transform.position.y, target.transform.position.z);
            obj.GetComponent<Movement>().speed = obj.GetComponent<Movement>().Bspeed;
        }
        if(GhostOnly && obj.tag != "Ghost")
        {
            obj.transform.position -= 0.05f * obj.transform.forward;
            obj.GetComponent<Movement>().speed = 0;
        }
    }
}
