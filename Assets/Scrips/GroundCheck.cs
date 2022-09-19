using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    CharacterContoller charterCont;


    private void Awake()
    {
        charterCont = GetComponentInParent<CharacterContoller>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == charterCont.gameObject)
            return;

        charterCont.setGrounded(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == charterCont.gameObject)
            return;

        charterCont.setGrounded(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == charterCont.gameObject)
            return;

        charterCont.setGrounded(true);
    }

}
