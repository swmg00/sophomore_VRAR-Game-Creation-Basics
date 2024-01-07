using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    //무기 유형 정의
    public enum Type { Sword, Range };
    public Type type;

    public int damage;  // 데미지
    public float rate;  // 공격 속도
    public BoxCollider SwordArea;  //공격 범위
    public TrailRenderer trailEffect; //공격 이펙트


    //무기 사용 메서드 
    public void Use()
    {
        if (type == Type.Sword)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
    }

    //휘두르기 애니메이션을 위한 코루틴 
    IEnumerator Swing()
    {
        // 대기 시간
        yield return new WaitForSeconds(0.1f);

        // 휘두르기 실행
        SwordArea.enabled = true;
        trailEffect.enabled = true;

        // 대기 시간
        yield return new WaitForSeconds(0.3f);
        SwordArea.enabled = false;

        // 휘두르기 종료
        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
    }

    //충돌 발생 시 호출되는 메서드
    void OnCollisionEnter(Collision collision)
    {
        //충돌 대상이 " world" 태그를 가지고 있으면 트리거로 설정
        if (collision.gameObject.tag == "world")
            SwordArea.isTrigger = true;
    }

    //트리거에 진입 시 호출되는 메서드
    void OnTriggerEnter(Collider other)
    {
        //충돌 대상이 "Monster"태그를 가지고 있으면 몬스터에게 데미지를 가하는 메서드 호출
        if (other.tag == "Monster")
        {
            Monster monster = other.GetComponent<Monster>();
            if (monster != null)
            {
               //반응 벡터 계산
                Vector3 reactVec = monster.transform.position - transform.position;

               //몬스터 스크립트의 OnDamage 코루틴 호출
                monster.StartCoroutine(monster.OnDamage(reactVec));
            }
        }
    }
}