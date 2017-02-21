using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : Photon.MonoBehaviour
{
    public float speed;
    public float Bspeed;
    public Text you;
    public GameObject cam1, cam2;
    public Vector3 Spawn;
    public PhotonPlayer p;
    // Use this for initialization
    public void Start()
    {
        you.text = "You're " + gameObject.name + "!";

        if (Menu.Connected() && !photonView.isMine)
        {
            cam1.SetActive(false);
            you.gameObject.SetActive(false);
        }
        if (Menu.Connected())
            p = PhotonNetwork.player;
        transform.position = Spawn;
    }
    public void Respawn()
    {
        transform.position = Spawn;
        speed = 0;
    }
    void switchCam()
    {
        if (cam1.activeSelf)
        {
            cam2.SetActive(true);
            cam1.SetActive(false);
            return;
        }
        cam2.SetActive(false);
        cam1.SetActive(true);
        return;

    }
    // Update is called once per frame
    public void Update()
    {
        if (Menu.Connected() && !photonView.isMine)
            return;
        if (Menu.Connected() && PhotonNetwork.player.GetScore() == -5)
        {
            
            enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.Rotate(new Vector3(0, 180, 0));
            speed = Bspeed;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            speed = Bspeed;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.Rotate(new Vector3(0, 90, 0));
            speed = Bspeed;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            speed = Bspeed;
            transform.Rotate(new Vector3(0, -90, 0));
        }
        if (Input.GetKeyDown(KeyCode.C))
            switchCam();
        transform.position = transform.position + speed * transform.forward;
    }

    public void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == "Wall")
        {
            speed = 0;
            if (gameObject.tag == "Pacman")
                transform.position = transform.position - 0.1f * transform.forward;
            else
                transform.position = transform.position - 0.05f * transform.forward;
        }
    }
    protected void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }

        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
