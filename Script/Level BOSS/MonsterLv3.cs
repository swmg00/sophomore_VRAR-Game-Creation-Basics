using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MonsterLv3 : MonoBehaviour
{
    // 몬스터 최대 체력
    public int maxHealth = 3000;
    // 현재 체력
    public int health = 3000;

    // 회전 속도
    public float rotationSpeed = 30f;


    // Rigidbody와 Sphere Collider
    Rigidbody rigid;
    SphereCollider sphereCollider;

    // 몬스터가 죽었는지 여부를 나타내는 플래그
    private bool isDead = false;

    private Transform playerTransform;

    // 데미지 사운드
    public AudioClip damage;
    AudioSource damageAudio;

    private Weapon weapon;

    
    private void Awake()
    {
        // 컴포넌트 초기화
        rigid = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        damageAudio = GetComponent<AudioSource>();

        damageAudio.clip = damage;


        // 게임 시작 시 플레이어의 Transform 찾아 할당
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;


        // 플레이어를 따라 무작위로 회전하는 코루틴 시작
        StartCoroutine(FollowPlayerRotation());


    }

    // 플레이어에게 닿았을 때 호출되는 메서드
    void OnTriggerEnter(Collider other)
    {
        // 몬스터가 이미 죽지 않았고 검에 맞았을 경우
        if (!isDead && other.tag == "Sword")
        {
            // 무기의 데미지를 가져오고 몬스터의 체력 감소
            weapon = other.GetComponent<Weapon>();
            health -= weapon.damage;
            GameManagerLvBOSS.Instance.updateBossHealth(health);

            // 플레이어에게 받은 반작용 벡터 계산 및 데미지 처리 코루틴 시작
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));
        }
    }

    // 데미지를 받았을 때 처리하는 코루틴
    public IEnumerator OnDamage(Vector3 reactVec)
    {
        // 데미지 사운드 재생 후 잠시 대기
        damageAudio.Play();
        yield return new WaitForSeconds(0.1f);

        // 이미 몬스터가 죽었다면 코루틴 종료
        if (isDead)
        {
            yield break;
        }

        // 몬스터에게 데미지 적용 및 체력 갱신
        health -= weapon.damage;
        GameManagerLvBOSS.Instance.updateBossHealth(health);

        // 몬스터의 체력이 0 이하일 경우
        if (health <= 0)
        {
            // 몬스터를 죽은 상태로 설정하고 레이어 변경
            isDead = true;
            gameObject.layer = 19;
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 10, ForceMode.Impulse);

            // 0.2초 후 몬스터 제거
            Destroy(gameObject, 0.2f);

            // GameManagerLvBOSS에게 보스가 죽었음을 알림
            GameManagerLvBOSS.Instance.BossDied();
        }

    }


    // 플레이어를 따라 회전하는 코루틴
    IEnumerator FollowPlayerRotation()
    {
        while (true)
        {
            if (playerTransform != null)
            {
                // 플레이어의 방향 벡터 계산
                Vector3 directionToPlayer = playerTransform.position - transform.position;

                // 몬스터를 플레이어 쪽으로 회전
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            }

            yield return null;
        }
    }
}