using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossBulletSpawner : MonoBehaviour
{
    //총알 프리팹
    public GameObject bulletPrefab;

    //총알 생성 간격 최소값
    public float spawnRateMin = 0.5f;
    //총알 생성 간격 최대값
    public float spawnRateMax = 3f;
    // 현재 총알 생성 간격
    float spawnRate;
    // 총알 생성 후 경과 시간
    float timeAfterSpawn;

    // 총알 사운드
    public AudioClip bullet;
    // 총알 사운드를 재생하는 오디오 소스
    AudioSource bulletAudio;

    // 플레이어를 추적하기 위한 게임 오브젝트
    GameObject player;


    void Start()
    {
        // 초기화
        timeAfterSpawn = 0f;
        // 초기 총알 생성 간격 설정
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        // "Player" 태그를 가진 게임 오브젝트 찾기
        player = GameObject.FindGameObjectWithTag("Player");
        // 오디오 소스 컴포넌트 참조
        bulletAudio = GetComponent<AudioSource>();
        // 총알 사운드 클립 설정
        bulletAudio.clip = bullet;

    }

    void Update()
    {
        // 플레이어가 없으면 리턴
        if (player == null)
            return;

        // 경과 시간 업데이트
        timeAfterSpawn += Time.deltaTime;

        if (timeAfterSpawn >= spawnRate)
        {
            // 경과 시간 초기화
            timeAfterSpawn = 0f;
            bulletAudio.Play();

            // 플레이어의 높이에 추가로 위치를 더합니다.
            float playerHeight = player.transform.position.y;
            Vector3 spawnPosition = new Vector3(transform.position.x, playerHeight + 2f, transform.position.z);

            // 총알 생성
            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, transform.rotation);
            // 플레이어를 향해 총알이 바라보도록 설정
            bullet.transform.LookAt(player.transform);
            // 다음 총알 생성 간격 설정
            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        }
    }
}
