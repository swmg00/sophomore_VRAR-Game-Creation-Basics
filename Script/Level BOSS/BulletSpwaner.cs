using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpwaner : MonoBehaviour
{
    //프리팹
    public GameObject bulletPrefab;
    
    public float spawnRateMin = 0.5f;
    public float spawnRateMax = 3f;
    float spawnRate;
    float timeAfterSpawn;


    public AudioClip bullet;
    AudioSource bulletAudio;
    GameObject player;
  

    void Start()
    {
        timeAfterSpawn = 0f;
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        // "Sword" 태그를 가진 게임 오브젝트 찾기
        player = GameObject.FindGameObjectWithTag("Sword");
        bulletAudio = GetComponent<AudioSource>();

        bulletAudio.clip = bullet;

    }

    // Update is called once per frame
    void Update()
    {

        if (player == null)
            return;

        timeAfterSpawn += Time.deltaTime;
        if (timeAfterSpawn >= spawnRate)
        {
            timeAfterSpawn = 0f;
            bulletAudio.Play();

            // 고정된 높이에 위치에 총알 생성
            Vector3 spawnPosition = new Vector3(transform.position.x, 3f, transform.position.z);

            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, transform.rotation);
            // 플레이어를 향해 총알이 바라보도록 설정
            bullet.transform.LookAt(player.transform);
            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        }
    }
}
