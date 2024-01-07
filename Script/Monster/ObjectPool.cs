using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance; //싱글톤 인스턴스

    private Queue<GameObject> objectPool = new Queue<GameObject>(); //게임 오브젝트 풀

    void Awake()
    {
        //싱글톤 패턴 적용 
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //게임 오브젝트를 풀에 반환하고 비활성화하는 메서드
    public static void Destroy(GameObject obj)
    {
        //게임 오브젝트 비활성화
        obj.SetActive(false);

        //게임 오브젝트의 상태를 초기화하고 풀에 반환
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;

        //풀에 게임 오브젝트 추가 
        Instance.objectPool.Enqueue(obj);
    }
}