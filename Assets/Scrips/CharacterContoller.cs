using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class CharacterContoller : MonoBehaviour
{
    public float verticalLookRotation, mouseSensivty , walkSpeed , runSpeed,  jumpForce , smoothTime;
    public GameObject Cameraholder;
    Rigidbody rb;
    Vector3 moveAmount;
    Vector3 smoothvelocity;
    private bool ground;
    private bool canAttack = true;
    public float attackTime;
    PhotonView pv;
    public GameObject bullet;
    GameObject muzzel;
    public GameObject ui;
    gameManager gm;
    public Text AmmoUi;
    int ammo ;
    bool reloadCheck = false;
    bool stopAnim = false;
    Animator anim;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {

        if (!pv.IsMine)
            return;
        Look();
        move();
        jump();
        
    }

    private void Start()
    {
         Cursor.lockState = CursorLockMode.Locked;
        
        if (!pv.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(ui);
        }
        ammo = 30;

        AmmoUi.text = ammo.ToString() + "/30"; 
        muzzel = this.gameObject.transform.GetChild(0).transform.GetChild(0).gameObject;
    }
    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensivty);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensivty;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        Cameraholder.transform.localEulerAngles = Vector3.left * verticalLookRotation;

    }

    void move()
    {
       
        Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDirection * (Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed), ref smoothvelocity, smoothTime, runSpeed);

        if (moveDirection == Vector3.zero && ground)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Jump", false);
            anim.SetBool("Stop", true);
            anim.SetBool("Run", false);
        }
        else if (Input.GetKey(KeyCode.LeftShift) && ground)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Jump", false);
            anim.SetBool("Stop", false);
            anim.SetBool("Run", true);
        }
        else if (ground)
        {
            anim.SetBool("Walk", true);
            anim.SetBool("Jump", false);
            anim.SetBool("Stop", false);
            anim.SetBool("Run", false);
        }

    }

    void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && ground)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    public void setGrounded(bool _ground)
    {
        ground = _ground;
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine)
            return;

        if (!ground && !stopAnim)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Jump", true);
            anim.SetBool("Stop", false);
            anim.SetBool("Run", false);

            stopAnim = true;
        }
        else if (stopAnim && ground)
        {
            stopAnim = false;
        }

        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.deltaTime);
        if((ammo <= 0 || Input.GetKey(KeyCode.R)) && !reloadCheck && ammo!=30)
        {
            StartCoroutine(reload());
            
        }
        if (Input.GetKey(KeyCode.Mouse0) && ammo > 0 && !reloadCheck)
        {
            if (canAttack)
            {
                StartCoroutine(AttackTime());
            }
            
        }
    }

    IEnumerator reload()
    {
        reloadCheck = true;
        yield return new WaitForSeconds(1);
        //reload anmasyonu
        ammo = 30;
        AmmoUi.text = ammo.ToString() + "/30";
        reloadCheck = false;
    }

        IEnumerator AttackTime()
    {
        canAttack = false;
        bulletSpawn();
        yield return new WaitForSeconds(attackTime);

        canAttack = true;
    }

    private void bulletSpawn()
    {
        ammo -= 1;
        AmmoUi.text = ammo.ToString() + "/30";
        GameObject.Find("manager").GetComponent<gameManager>().bulletSpawn(muzzel);
    }


}
