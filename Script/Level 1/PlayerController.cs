using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    private bool _contactWall;
    public bool fDown;
    bool isFireReady = true;
    bool isBorder;

    public FixedJoystick joystick;

    public bool ContactWall
    {
        get { return _contactWall; }
        set { _contactWall = value; }
    }

    public int coin;
    public int health = 100;
    public int score;


    public int maxHealth = 100;

    public float speed;

    float backwardForce = 0.5f;
    float fireDelay;


    Vector3 moveVec;

    Animator anim;
    AudioSource attackAudio;
    Rigidbody myRigid;

    public AudioClip attack;
    public AudioClip treasurePickup;

    public List<AudioSource> playerAudio = new List<AudioSource>();
    public Weapon equipWeapon;

    private bool hasAttacked = false;

    void Awake()
    {
        if (instance == null)
            instance = this;

        fDown = false;
        anim = GetComponentInChildren<Animator>();
        myRigid = GetComponent<Rigidbody>();
        equipWeapon = GetComponentInChildren<Weapon>();

        playerAudio.Add(gameObject.AddComponent<AudioSource>());
        playerAudio.Add(gameObject.AddComponent<AudioSource>());

        playerAudio[0].clip = attack;
        playerAudio[1].clip = treasurePickup;



    }

    void StopToWall()
    {
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }

    void Update()
    {
        StopToWall();
        Move();
        Turn();
        playerAttack();
    }



    void Move()
    {
        float moveHorizontal = joystick.Horizontal;
        float moveVertical = joystick.Vertical;

        if (!_contactWall)
        {
            moveVec = new Vector3(moveHorizontal, 0, moveVertical).normalized;
            if (!isBorder)
            {
                transform.position += moveVec * speed * Time.deltaTime;
            }
            else
            {
                myRigid.AddForce(-transform.forward * backwardForce, ForceMode.Impulse);
            }

            anim.SetBool("isRun", moveVec != Vector3.zero);
        }
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }



    void FreezeRotation()
    {
        myRigid.angularVelocity = Vector3.zero;
    }






    public void playerAttack()
    {


        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if (fDown && isFireReady)
        {

            equipWeapon.Use();
            playerAudio[0].Play();
            anim.SetTrigger("doSwing");
            fireDelay = 0f;

        }
    }

    public void ResetAttackFlag()
    {
        fDown = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Treasure"))
        {
            ScoreManager.instance.AddCoin(2);

            other.gameObject.SetActive(false);


            if (treasurePickup != null)
            {
                playerAudio[1].clip = treasurePickup;
                playerAudio[1].Play();
            }
            else
            {
                ScoreManager.instance.AddMonster(1, 0);
            }
        }
        else if (other.gameObject.CompareTag("Treasure2"))
        {

            ScoreManager.instance.AddCoin(4);

            other.gameObject.SetActive(false);
            if (treasurePickup != null)
            {
                playerAudio[1].clip = treasurePickup;
                playerAudio[1].Play();
            }

        }
    }


}