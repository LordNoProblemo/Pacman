using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Movement
{

    public bool eat = false;
    public GameObject EatSkin;
    public int EatTime;
    int eatTimer = 0;
    public int score;
    void Start()
    {
        base.Start();
        
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.isWriting)
        {
            stream.SendNext(eat);
            stream.SendNext(EatSkin.activeSelf);
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }

        else
        {
            eat = (bool)stream.ReceiveNext();
            EatSkin.SetActive((bool)stream.ReceiveNext());
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
    // Update is called once per frame
    void Update()
    {
        base.Update();
        EatSkin.SetActive(eat);
        if (Menu.Connected())
            if (PhotonNetwork.player.GetScore() == -2)
            {
                eat = true;
                PhotonNetwork.player.SetScore(score);
            }
        if (eat)
        {
            if (eatTimer == EatTime)
            {
                eat = false;
                eatTimer = 0;
            }
            else
                eatTimer++;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        GameObject obj = collision.gameObject;
        if (obj.tag == "Pacman" && eat)
        {
            Respawn();
            eat = false;
            obj.GetPhotonView().owner.SetScore(-400);
            PhotonNetwork.player.SetScore(score);
        }
    }
}
