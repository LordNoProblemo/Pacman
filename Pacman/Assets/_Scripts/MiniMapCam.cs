using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCam : Photon.MonoBehaviour {

    public bool saveLoc;
    private Vector3  loc;
    Quaternion rot;
    public GameObject cam;
	// Use this for initialization
	void Start () {
        if (Menu.Connected() && !photonView.isMine)
            cam.SetActive(false);
        loc = cam.transform.position;
        rot = cam.transform.rotation;		
	}
	
	// Update is called once per frame
	void Update () {
        if (Menu.Connected() && !photonView.isMine)
            cam.SetActive(false);
        cam.transform.rotation = rot;
        if (saveLoc)
            cam.transform.position = loc;
	}
}
