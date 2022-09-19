using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GAmeOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        PhotonNetwork.LeaveRoom();
    }

    public void OnMainMenuButtonClick()
    {
        
        SceneManager.LoadScene("MainMenu");
    }
}
