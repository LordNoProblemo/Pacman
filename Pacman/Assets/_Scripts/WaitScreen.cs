using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;

public class WaitScreen : MonoBehaviour
{

    int index = -1;
    public Text Title, PlayersNum, PlayersList;
    public Button StartGame;
    public RawImage Loading;
    // Use this for initialization
    void Start()
    {
        StartGame.gameObject.active = false;
        PhotonNetwork.player.SetScore(-1);
        Debug.Log("MIP");
        if (PhotonNetwork.inRoom)
            PhotonNetwork.room.IsVisible = true;
        

    }
    bool checkPname()
    {
        foreach(PhotonPlayer p in PhotonNetwork.otherPlayers)
        {
            if (p.NickName == PhotonNetwork.player.NickName)
                return false;
        }
        return true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.inRoom)
        {
            Loading.gameObject.SetActive(true);
            return;
        }
        if (PhotonNetwork.player.IsMasterClient)
            PhotonNetwork.player.SetScore(index);
        Loading.gameObject.SetActive(false);
       
        checkStart();
        if (PhotonNetwork.player.GetScore() > -1)
            SceneManager.LoadScene("Game");
       
        Title.text = "Room Name: " + PhotonNetwork.room.Name;
        PlayersNum.text = "Number Of Players:" + PhotonNetwork.room.PlayerCount.ToString() + "/5";
        PlayersList.text = "";
        for (int i = 0; i < PhotonNetwork.room.PlayerCount; i++)
        {
            PlayersList.text += i + ") " + PhotonNetwork.playerList[i].NickName + "\n";
        }
        if (PhotonNetwork.player.IsMasterClient && PhotonNetwork.room.PlayerCount > 1)
            StartGame.gameObject.active = true;
        else
            StartGame.gameObject.active = false;
    }

    List<int> Shuffle(List<int> lst)
    {
        List<int> ret = new List<int>();
        int n = lst.Count;
        for(int i = 0;i<n;i++)
        {
            int ind = Random.Range(0, lst.Count);
            ret.Add(lst[ind]);

            lst.Remove(lst[ind]);
        }
        return ret;
    }

    public void GameStart()
    {
        PhotonNetwork.room.IsVisible = false;
        int n = PhotonNetwork.room.PlayerCount;
        List<int> indexes = new List<int>();
        for (int i = 0; i < n; i++)
        {
            indexes.Add(i);
        }
        indexes = Shuffle(indexes);
        
        int id = 0;
        for (int i = 0; i < n; i++)
        {
            if (PhotonNetwork.player == PhotonNetwork.playerList[i])
                id = i;
            PhotonNetwork.playerList[i].SetScore(indexes[i]);
        }
        PhotonNetwork.player.SetScore(indexes[id]);
        index = indexes[id];
        Debug.Log(PhotonNetwork.player.ID + ", " + PhotonNetwork.player.GetScore());
        

    }
    void checkStart()
    {
        
        
        int n = PhotonNetwork.room.PlayerCount;
        bool started = false;
        for (int i = 0;i<n;i++)
        {
            if(PhotonNetwork.playerList[i].GetScore()>-1)
            {
                started = true;
                break;
            }
        }
        if (!started)
            return;

        if (PhotonNetwork.player.GetScore() != -1)
            return;
        List<int> indexes = new List<int>();
        List<int> chosen = new List<int>();
        for (int i = 0; i < n; i++)
        {
            indexes.Add(i);
            if (PhotonNetwork.playerList[i].GetScore() != -1)
                chosen.Add(PhotonNetwork.playerList[i].GetScore());
        }
        foreach(int i in chosen)
        {
            indexes.Remove(i);
        }
        indexes = Shuffle(indexes);
        PhotonNetwork.player.SetScore(indexes[0]);


    }
}
