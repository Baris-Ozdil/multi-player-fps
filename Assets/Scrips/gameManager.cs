using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviourPun
{

    public GameObject playerPref;
    public GameObject bullet;
    GameObject player;
    PhotonView PV;
    public int die_count = 3;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    void Start()
    {
            CreatePlayer();
    }

    void CreatePlayer()
    {
        player = PhotonNetwork.Instantiate(playerPref.name, new Vector3(Random.Range(-10f, 10f), 1f, Random.Range(-10f, 10f)), Quaternion.identity , 0 , new object[] { PV.ViewID });
    }


    public void playerDie()
    {
        die_count -= 1;
        if (die_count == 0)
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            PV.RPC("gameOver", RpcTarget.Others);
            SceneManager.LoadScene("Lose");
            
            return;
        }

        PhotonNetwork.Destroy(player);
        CreatePlayer();
    }

    [PunRPC]


    public void DestroyObject(GameObject objee)
    {
        PhotonNetwork.Destroy(objee);
    }


    public void bulletSpawn(GameObject objee)
    {
        PhotonNetwork.Instantiate(bullet.name, objee.transform.position, objee.transform.rotation);
    }
}
