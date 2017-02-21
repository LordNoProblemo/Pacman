using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject Maze;
    public List<GameObject> PlayersPrefs;
    public static string Won;
    public Text Finish;
    // Use this for initialization
    void Start()
    {
        Debug.Log(PlayersPrefs.Count + "," + PhotonNetwork.player.GetScore());


        GameObject.Instantiate(Maze, new Vector3(0, 0, 0), Quaternion.identity);
        GameObject obj = PlayersPrefs[PhotonNetwork.player.GetScore() % 5];
        GameObject Player = PhotonNetwork.Instantiate(obj.name, obj.GetComponent<Movement>().Spawn, obj.transform.rotation, 0);
        Player.name = obj.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || PhotonNetwork.player.GetScore() == -1)
        {
            PhotonNetwork.room.IsVisible = true;
            PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);

            for (int i = 0; i < PhotonNetwork.room.PlayerCount; i++)
            {
                PhotonNetwork.playerList[i].SetScore(-1);
            }

            SceneManager.LoadScene("Menu");

        }
        if(PhotonNetwork.player.GetScore() == -5)
        {
            Finish.text = Won;
        }
    }
}
