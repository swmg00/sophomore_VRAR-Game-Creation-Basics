//몬스터 제네레이터
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterGenerator : MonoBehaviour
{
    public GameObject enemyPrefab; // 몬스터 프리팹에 대한 참조 
    public float spawnRadius = 10.0f; // 몬스터가 생성될 반경
    public int numberOfEnemies = 20; // 생성할 몬스터의 수
    public float minDistanceBetweenMonsters = 2.0f; // 몬스터 간 최소 간격

   //활성 상태의 몬스터를 추적하기 위한 리스트
    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        //몬스터 생성 메서드 호출
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        //지정된 수만큼 몬스터 생성
        for (int i = 0; i < numberOfEnemies; i++)
        {
            //유효한 생성 위치를 얻음
            Vector3 spawnPosition = GetValidSpawnPosition();

            //몬스터의 원하는 회전값 설정
            Quaternion desiredRotation = Quaternion.Euler(0f, 0f, 0f);

            //몬스터 생성 및 위치 설정
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, desiredRotation);

            //생성된 몬스터를 활성 상태의 몬스터 리스트에 추가 
            activeEnemies.Add(enemy);
        }
    }
    
    //유효한 생성 위치를 얻는 메서드
    Vector3 GetValidSpawnPosition()
    {
        Vector3 newSpawnPosition;

        //유효한 위치를 얻을 때까지 반복
        do
        {
            newSpawnPosition = RandomSphereInPoint(spawnRadius);
        } while (IsTooCloseToExistingMonster(newSpawnPosition));

        return newSpawnPosition;
    }

    //기존 몬스터와의 거리를 확인하여 너무 가까우면 true 반환
    bool IsTooCloseToExistingMonster(Vector3 position)
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (Vector3.Distance(position, enemy.transform.position) < minDistanceBetweenMonsters)
                return true; //너무 가까우면 다시 시도 
         
        }

        return false; //유효한 위치
    }

    //주어진 반경 내에서 무작위 위치를 반환하는 메서드
    public Vector3 RandomSphereInPoint(float radius)
    {
        Vector3 getPoint = Random.onUnitSphere;
        getPoint.y = 0.0f;

        float r = Random.Range(0.0f, radius);

        return (getPoint * r) + transform.position;
    }
}