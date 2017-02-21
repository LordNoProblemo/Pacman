using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public RawImage Load;
    public Image menu, waitScreen;
    public InputField Pname, Rname;
    public Dropdown Rooms;
    int Rnum = 0;
    RoomOptions roomOptions;
    public static string PlayerName;

    public static bool Connected()
    {
        return PhotonNetwork.connectionState == ConnectionState.Connected || PhotonNetwork.connected;
    }
    // Use this for initialization
    void Start () {
        try
        {
            Load.gameObject.SetActive(true);
            if (!Connected())
                PhotonNetwork.ConnectUsingSettings("MultiPac1");
        }
        catch(Exception e)
        {
            Application.Quit();
        }

    }
    bool checkExist()
    {
        foreach (RoomInfo RI in PhotonNetwork.GetRoomList())
            if (RI.Name == Rname.text)
                return true;
        return false;
    }
    bool checkExist2()
    {
        foreach (RoomInfo RI in PhotonNetwork.GetRoomList())
            if (RI.Name == Rooms.captionText.text)
                return true;
        return false;
    }
    void Awake()
    {
        PhotonNetwork.autoJoinLobby = true;
    }
    // Update is called once per frame
    void Update () {
        try
        {
            if (Connected())
                Load.gameObject.SetActive(false);
            else
                return;
            if (PhotonNetwork.inRoom)
            {
                PhotonNetwork.player.SetScore(-1);
                waitScreen.gameObject.SetActive(true);
                menu.gameObject.SetActive(false);
            }
            else
            {
                waitScreen.gameObject.SetActive(false);
                menu.gameObject.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Escape) )
            {
                Debug.Log("Meow");
                if (!PhotonNetwork.inRoom)
                    Application.Quit();
                else
                {
                    PhotonNetwork.LeaveRoom();
                    LeaveRoom();
                }
            }
            roomOptions = new RoomOptions();
            roomOptions.maxPlayers = 5;
            roomOptions.IsVisible = true;
            RoomInfo[] rooms = PhotonNetwork.GetRoomList();
            List<string> ops = new List<string>();

            foreach (RoomInfo r in rooms)
            {
                if (r.PlayerCount < r.MaxPlayers && r.IsVisible)
                    ops.Add(r.Name);
            }

            if (Rnum != ops.Count)
            {
                Rooms.ClearOptions();
                Rooms.AddOptions(ops);
                Rnum = ops.Count;
            }
        }
        catch (Exception e)
        {
            Application.Quit();
        }

    }
    public void CreateRoom()
    {
        if (checkExist() || Rname.text == "")
            return;

        PhotonNetwork.CreateRoom(Rname.text,roomOptions, null);
        PlayerName = Pname.text;
        menu.gameObject.SetActive(false);
        waitScreen.gameObject.SetActive(true);
        PhotonNetwork.player.NickName = PlayerName;
        PhotonNetwork.player.SetScore(-1);
    }
    public void JoinRoom()
    {
        if (!checkExist2())
            return;
        PhotonNetwork.JoinRoom(Rooms.captionText.text);
        PlayerName = Pname.text;
        menu.gameObject.SetActive(false);
        waitScreen.gameObject.SetActive(true);
        PhotonNetwork.player.NickName = PlayerName;
        PhotonNetwork.player.SetScore(-1);
        

    }
    void LeaveRoom()
    {
        menu.gameObject.SetActive(true);
        waitScreen.gameObject.SetActive(false);
        Load.gameObject.SetActive(true);
        Pname.text = PlayerName;
    }
    
}
