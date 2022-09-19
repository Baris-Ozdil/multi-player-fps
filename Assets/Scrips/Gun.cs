using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Gun : MonoBehaviourPun
{
    Rigidbody rb;
    GameObject gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("manager");
        rb = GetComponent<Rigidbody>();
        rb.velocity += transform.forward * 15;

        StartCoroutine(DestroyTime());
    }
    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(4.5f);

        gm.GetComponent<gameManager>().DestroyObject(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            gm.GetComponent<gameManager>().DestroyObject(this.gameObject);
            return;
        }
        other.GetComponent<PlayerHealth>().takeDamage();

        gm.GetComponent<gameManager>().DestroyObject(this.gameObject);

    }
}
