using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform targetTransform; //카메라가 따라다닐 대상의 Transform
    public Vector3 Offset; //대상으로부터 떨어진 상대적인 위치


    void Update()
    {
        //카메라의 위치를 대상의 위치에 오프셋을 더한 값으로 설정
        transform.position = targetTransform.position + Offset;
    }
}
