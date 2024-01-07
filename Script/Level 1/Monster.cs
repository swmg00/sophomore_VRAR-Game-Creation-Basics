using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Monster : MonoBehaviour
{
    public int mon1Score;
    // 몬스터 최대 체력
    public int maxHealth;
    // 현재 체력
    public int curHealth;

    // 회전 속도
    public float rotationSpeed = 30f;

    // Treasure 프리팹
    public GameObject treasurelv1Prefab;


    // Rigidbody와 Sphere Collider
    Rigidbody rigid;
    SphereCollider sphereCollider;

    // 몬스터가 죽었는지 여부를 나타내는 플래그
    private bool isDead = false;

    private Transform playerTransform;

    // 피해 사운드
    public AudioClip damage;
    AudioSource damageAudio;

    private void Awake()
    {
        // 컴포넌트 초기화
        rigid = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        damageAudio = GetComponent<AudioSource>();

        damageAudio.clip = damage;

        // 게임이 시작될 때 플레이어의 트랜스폼을 찾아서 할당
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // 코루틴 시작
        StartCoroutine(FollowPlayerRotation());
    }

    // 플레이어에게 닿았을 때 호출되는 메서드
    void OnTriggerEnter(Collider other)
    {
        // 몬스터가 이미 죽지 않았고, 검에 맞았다면
        if (!isDead && other.tag == "Sword")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));
        }
    }

    // 데미지를 받았을 때 처리하는 코루틴
    public IEnumerator OnDamage(Vector3 reactVec)
    {
        damageAudio.PlayOneShot(damageAudio.clip);
        yield return new WaitForSeconds(0.1f);

        // 몬스터의 체력이 0 이하이고 아직 죽지 않았다면
        if (curHealth <= 0 && !isDead)
        {
            isDead = true;
            gameObject.layer = 19;

            // 사망 시 튀어오르는 효과
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 10, ForceMode.Impulse);

            // 보물 생성 및 몬스터 제거
            Destroy(gameObject, 0.5f);
            SpawnTreasure(transform.position);

            if (GameManager.Instance.stage != 1)
            {
                if (GameManagerLv2.iNstance.stage2 == 2)
                    AddLv2();
            }
            else AddLv1();
        }
    }

    void AddLv1()
    {

        if (gameObject.tag == "Monster")
            ScoreManager.instance.AddMonster(1, 0);
    }

    void AddLv2()
    {
        if (gameObject.tag == "Monster")
            ScoreManagerLv2.instance.AddMonster(1, 0);
        else if (gameObject.tag == "Monster2")
            ScoreManagerLv2.instance.AddMonster(0, 1);

    }

    // 보물 생성 메서드
    void SpawnTreasure(Vector3 spawnPosition)
    {
        if (treasurelv1Prefab != null)
        {
            Vector3 spawnOffset = new Vector3(0f, 1f, 0f);
            Vector3 adjustedSpawnPosition = spawnPosition + spawnOffset;

            Quaternion treasureRotation = Quaternion.Euler(-90f, 0f, 0f);
            GameObject treasure = Instantiate(treasurelv1Prefab, adjustedSpawnPosition, treasureRotation);
        }
    }

    // 플레이어를 따라 회전하는 코루틴
    IEnumerator FollowPlayerRotation()
    {
        while (true)
        {
            if (playerTransform != null)
            {
                // 플레이어를 향하는 방향 벡터 계산
                Vector3 directionToPlayer = playerTransform.position - transform.position;

                // 몬스터를 플레이어 쪽으로 회전
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            }

            yield return null;
        }
    }
}