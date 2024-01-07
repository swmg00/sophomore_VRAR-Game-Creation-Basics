using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControllerLvBOSS : MonoBehaviour
{
    // 플레이어 객체
    public GameObject player;
    public static PlayerControllerLvBOSS iNstance;
    
    // 조이스틱
    public FixedJoystick joystick;

    public int bossCnt;

    // 벽과 접촉 여부
    private bool _contactWall;
    // 공격 입력 여부
    public bool fDown;
    // 공격 가능 여부
    bool isFireReady = true;
    // 벽에 닿았는지 여부
    bool isBorder;

    // 획득한 보물 여부
    public bool hasCollectedTreasure = false;

    public bool ContactWall
    {
        get { return _contactWall; }
        set { _contactWall = value; }
    }

    // 플레이어 속성
    public int health = 150;
    public int maxHealth = 150;
    public int score;
    public int bossDamageAmount = 30;


    // 이동 및 회전 관련 변수
    public float speed;
    float hAxis;
    float vAxis;
    float backwardForce = 0.5f;
    float fireDelay;

    // 이동 벡터
    Vector3 moveVec;

    Animator anim;
    AudioSource attackAudio;
    Rigidbody myRigid;

    public AudioClip attack;
    public AudioClip treasurePickup;

    public List<AudioSource> playerAudio = new List<AudioSource>();
    
    // 현재 장착한 무기
    public Weapon equipWeapon;
    
    // 공격 플래그
    private bool hasAttacked = false;


    void Awake()
    {
        // 인스턴스가 null이면 현재 인스턴스를 할당
        if (iNstance == null)
            iNstance = this;

        fDown = false;

        // 컴포넌트 초기화
        anim = GetComponentInChildren<Animator>();
        myRigid = GetComponent<Rigidbody>();
        equipWeapon = GetComponentInChildren<Weapon>();

        // AudioSource 추가 및 AudioClip 할당
        playerAudio.Add(gameObject.AddComponent<AudioSource>());
        playerAudio.Add(gameObject.AddComponent<AudioSource>());
        playerAudio[0].clip = attack;
        playerAudio[1].clip = treasurePickup;

    }

    void StopToWall()
    {
        // 벽에 닿았는지 검사
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }

    void Update()
    {
        // 입력 받기 및 벽에 닿았는지 여부 확인
        GetInput();
        StopToWall();

        // 이동, 회전, 공격 처리
        Move();
        Turn();
        playerAttack();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        fDown = Input.GetMouseButtonDown(0);
    }

    void Move()
    {
        // 조이스틱으로 이동 처리
        float moveHorizontal = joystick.Horizontal;
        float moveVertical = joystick.Vertical;

        if (!_contactWall)
        {
            moveVec = new Vector3(moveHorizontal, 0, moveVertical).normalized;
           
            // 벽에 닿지 않았을 때 이동
            if (!isBorder)
            {
                transform.position += moveVec * speed * Time.deltaTime;
            }
            else
            {
                // 벽에 닿았을 때는 후진 힘을 가함
                myRigid.AddForce(-transform.forward * backwardForce, ForceMode.Impulse);
            }
            // 이동 애니메이션 재생
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

    void FixedUpdate()
    {
        FreezeRotation();
    }

    public void playerAttack()
    {
        // 공격 딜레이 계산
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        // 마우스 클릭 시 공격 및 공격 애니메이션 재생
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
        // 공격 플래그 초기화
        fDown = false;
    }

    public void bulletDamage()
    {
        // 총알에 의한 피해 처리
        health -= 8;
        GameManagerLvBOSS.Instance.updatePlayerHealth(health);

        // 플레이어 체력이 0 이하일 경우 사망 처리
        if (health <= 0)
            Die();
    }



    public void ReduceHealth(int amount)
    {
        // 체력 감소
        health -= amount;

        // 체력이 0 이하일 경우 사망 처리
        if (health <= 0)
        {
            health = 0;
            Die(); 
        }
    }

    public void Die()
    {
        // 게임 오브젝트 비활성화 및 보스 스테이지 종료 처리
        gameObject.SetActive(false);
        GameManagerLvBOSS.Instance.LevelBOSSEnd();
    }

}  