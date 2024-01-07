using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 총알 속도
    public float speed = 8f;

    // 총알 Rigidbody
    Rigidbody bulletRigidbody;



    void Start()
    {
        // 초기화
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletRigidbody.velocity = transform.forward * speed;

    }

    void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 "Sword" 또는 "Player" 태그인 경우
        if (other.tag == "Sword" || other.tag == "Player")
        {
            // 총알 파괴
            Destroy(gameObject);

            // 플레이어 컨트롤러 스크립트 가져오기
            PlayerControllerLvBOSS playerController = other.GetComponent<PlayerControllerLvBOSS>();

            // 플레이어 컨트롤러가 존재하면 피해 입히기 메서드 호출
            if (playerController != null)
            {
                playerController.bulletDamage();
            }
        }
        // 충돌한 오브젝트가 "Wall" 태그인 경우
        else if (other.tag == "Wall")
            Destroy(gameObject);
    }
}
