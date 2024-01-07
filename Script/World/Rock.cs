using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    // PlayerController 인스턴스에 대한 참조
    public PlayerController playerController;

    void Update()
    {
        // Ray를 정의하여 Ray를 시각적으로 확인하기 위해 DrawRay를 사용
        Vector3 look = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position + Vector3.up, look * 0.3f, Color.red);
        
        RaycastHit hit;

        //Raycast를 사용하여 앞쪽으로 레이를 쏘고 충돌이 감지되면 처리를 수행
        if (Physics.Raycast(transform.position, look, out hit, 0.3f))
        {
            if (hit.collider.gameObject.name == "Cube" && playerController != null)
            {
                // 플레이어가 "Cube"와 충돌하면 ContactWall 속성을 true로 설정
                playerController.ContactWall = true;
            }
        }
        else
        {
            if (playerController != null)
            {
               //충돌이 감지되지 않으면 ContactWall 속성을 false로 설정 
                playerController.ContactWall = false;
            }
        }
    }
}
