using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pacman : Movement {

    public float lifes = 3;
    public float points = 0;
    public Text Lifes, Points;
    GameObject PointsObjs;
	// Use this for initialization
	void Start () {
        base.Start();
        if (Menu.Connected())
            PointsObjs = PhotonNetwork.Instantiate("Points", new Vector3(0, 0, 0), Quaternion.identity, 0);
        
	}
	public void Respawn()
    {
        base.Respawn();
        if(PhotonNetwork.player.GetScore()  !=  -3)
            lifes--;
        PhotonNetwork.player.SetScore(-3);
    }
	// Update is called once per frame
	void Update () {
        base.Update();
        if (checkFinish())
            FinishGame("Pacman");
        if (lifes == 0)
            FinishGame("Ghosts");
        if(PhotonNetwork.player.GetScore() < -3 && PhotonNetwork.player.GetScore() % 4 == 0)
        {
            points -= PhotonNetwork.player.GetScore()/4;
            PhotonNetwork.player.SetScore(0);
        }
        if (PhotonNetwork.player.GetScore() == -3)
            PhotonNetwork.player.SetScore(0);
        Points.text = "Points: " + points;
        Lifes.text = "Lifes: " + lifes;
	}
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);
        if(stream.isWriting)
        {
            stream.SendNext(lifes);
            stream.SendNext(points);
            
        }
        else
        {
            lifes = (float)stream.ReceiveNext();
            points = (float)stream.ReceiveNext();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        GameObject obj = collision.gameObject;
        if(obj.tag == "Ghost")
        {
            if (obj.GetComponent<Ghost>().p.GetScore() != -2 && !obj.GetComponent<Ghost>().EatSkin.GetActive() && !obj.GetComponent<Ghost>().eat)
            {
                
                Respawn();
            }
            
        }
        if(obj.tag == "Point")
        {
            obj.SetActive(false);
            PhotonNetwork.player.SetScore(-4);
        }
        if (obj.tag == "EatPoint")
        {
            obj.SetActive(false);
            PhotonNetwork.player.SetScore(-40);
            if (Menu.Connected())
            {
                UpdateGhosts();
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        GameObject obj = collision.gameObject;
        
    }

    void UpdateGhosts()
    {
        for(int i = 0;i < PhotonNetwork.room.PlayerCount;i++)
        {
            if (PhotonNetwork.playerList[i] != PhotonNetwork.player)
                PhotonNetwork.playerList[i].SetScore(-2);
        }
    }

    bool checkFinish()
    {
        
        for(int i = 0;i<PointsObjs.transform.childCount;i++)
            if (PointsObjs.transform.GetChild(i).gameObject.activeSelf)
                return false;
        return true;
    }
    void FinishGame(string Won)
    {
        GameManager.Won = Won + " Won!!!";
        foreach (PhotonPlayer p in PhotonNetwork.playerList)
            p.SetScore(-5);
    }
}
