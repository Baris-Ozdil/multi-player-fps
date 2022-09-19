using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviourPun
{

    public Image healtbar;
    PhotonView PV;
    GameObject gm;
    float health = 100f;
    public Text lifeUI;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("manager");
        PV = GetComponent<PhotonView>();
        healtbar.fillAmount = 1;
        lifeUI.text ="Life : " + gm.GetComponent<gameManager>().die_count.ToString() ;
    }

    public void takeDamage()
    {
        PV.RPC("Rpc_TakeDAmage", RpcTarget.All, 20f);
    }

    [PunRPC]
    void Rpc_TakeDAmage(float damage)
    {
        if (!photonView.IsMine)
            return;
        health -= damage;
        healtbar.fillAmount = health / 100;
        if (health <= 0)
            gm.GetComponent<gameManager>().playerDie();
    }

    
}
