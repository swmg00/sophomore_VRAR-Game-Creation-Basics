using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // 벽과 접촉한 여부를 저장하는 변수
    private bool _contactWall;

    // 외부에서 벽과 접촉한 여부를 읽고 쓸 수 있는 속성
    public bool ContactWall
    {
        get { return _contactWall; }
        set { _contactWall = value; }
    }

    // 후진 힘
    float backwardForce = 0.5f;

    // 플레이어 속성
    public Player instance;
    float PlayerSpeed = 8f;

    // 코인을 표시하는 텍스트 UI
    public Text coinText;
    // 게임 패널 UI
    public GameObject GamePanel;

    // 코인 획득 시 재생되는 사운드
    public AudioClip pickUp;


    // 현재 코인 갯수
    public int count;
    // 벽과 접촉한지 여부
    bool isBorder;

    // 애니메이터, 리지드바디, 코인 획득 사운드 관련 변수
    Animator anim;
    Rigidbody myRigid;
    AudioSource pickUpAudio;

    void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (instance == null)
            instance = this;

        // 컴포넌트 초기화
        anim = GetComponentInChildren<Animator>();
        myRigid = GetComponent<Rigidbody>();
        pickUpAudio = GetComponent<AudioSource>();
        pickUpAudio.clip = pickUp;

        // 초기 코인 갯수 설정
        count = 0;
        SetCoinText();

    }

    // 벽에 닿았는지 여부 검사
    void StopToWall()
    {
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }

    void Update()
    {
        // 입력 받기
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");
        float xspeed = xInput * PlayerSpeed;
        float zspeed = zInput * PlayerSpeed;

        // 이동 벡터 설정
        Vector3 newVelocity = new Vector3(xspeed, 0f, zspeed);
        myRigid.velocity = newVelocity;

        // 마우스 클릭 시 이동 및 회전 처리
        if (Input.GetMouseButton(0))
        {
            // 마우스 위치를 월드 좌표로 변환
            Vector3 screePos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screePos.z);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.y = 1.0f;
            transform.position = Vector3.MoveTowards(transform.position, worldPos, PlayerSpeed * Time.deltaTime);

            // 방향 벡터 계산
            Vector3 moveDirection = worldPos - transform.position;
            moveDirection.y = 0f;

            // 새로 계산된 방향 벡터를 기준으로 플레이어가 바라보도록 회전
            if (moveDirection != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
            }
            // 이동 중 애니메이션 재생
            anim.SetBool("isRun", true);
        }
        else
            // 정지 시 애니메이션 설정
            anim.SetBool("isRun", false);
    }

    // 트리거 충돌 시 호출되는 메서드
    void OnTriggerEnter(Collider other)
    {
        // 트레저와 충돌한 경우
        if (other.gameObject.CompareTag("Treasure"))
        {
            // 코인 획득 사운드 재생 및 트레저 비활성화
            pickUpAudio.Play();
            other.gameObject.SetActive(false);
            count++;

            // 모든 트레저를 획득한 경우 Bonus 레벨 종료 처리
            if (count == 16)
            {
                pickUpAudio.Play();
                SetCoinText();
                GameManagerLvBonus.Instance.LevelBonusEnd();
            }
            else
                // 트레저를 먹을 때마다 텍스트 업데이트
                SetCoinText();
        }
    }

    // 코인 텍스트 업데이트 메서드
    void SetCoinText()
    {
        coinText.text = "x " + count.ToString();
    }
}