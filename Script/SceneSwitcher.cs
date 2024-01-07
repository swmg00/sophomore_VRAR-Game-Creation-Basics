using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // SceneSwitcher의 단일 인스턴스를 관리하는 정적 변수
    public static SceneSwitcher instance;

    // 객체가 생성될 때 호출되는 메서드
    private void Awake()
    {
        // 만약 SceneSwitcher 인스턴스가 없으면 현재 인스턴스를 할당
        if (instance == null)
            instance = this;
      }

    //씬 전환 메서드 
    public void SwitchScene(string sceneName)
    {
        //주어진 씬으로 전환
        SceneManager.LoadScene(sceneName);

        //전환된 씬에 따라 활성화 여부 설정
        if (sceneName == "Level 1")
        {
            GameManager.Instance.gameObject.SetActive(true);
            GameManagerLv2.iNstance.gameObject.SetActive(false);
        }
        else if (sceneName == "Level 2")
        {
            GameManager.Instance.gameObject.SetActive(false);
            GameManagerLv2.iNstance.gameObject.SetActive(true);
        }
        else if (sceneName == "Level BOSS")
        {
            GameManagerLv2.iNstance.gameObject.SetActive(false);
            GameManagerLvBOSS.Instance.gameObject.SetActive(true);
        }
        else if (sceneName == "Level Bonus")
        {
            GameManagerLvBOSS.Instance.gameObject.SetActive(false);
            GameManagerLvBonus.Instance.gameObject.SetActive(true);    
        }

    }
}
