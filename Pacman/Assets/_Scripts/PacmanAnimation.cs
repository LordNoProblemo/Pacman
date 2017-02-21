using UnityEngine;
using System.Collections;

public class PacmanAnimation : Photon.MonoBehaviour {
    private bool close = true;
    public GameObject upper, lower;
    float limX;
    public float speed = 1.0f;
	// Use this for initialization
	void Start () {
        limX = upper.transform.localRotation.x;
	}
	
	// Update is called once per frame
	void Update () {
        if (close && upper.transform.localRotation.x < 0)
        {
            upper.transform.Rotate(new Vector3(0, -speed, 0));
            lower.transform.Rotate(new Vector3(0, -speed, 0));
        }
        else if (close)
            close = false;
        else if (!close && upper.transform.localRotation.x > limX)
        {
            upper.transform.Rotate(new Vector3(0, speed, 0));
            lower.transform.Rotate(new Vector3(0, speed, 0));
        }
        else
            close = true;
        
	}
}
