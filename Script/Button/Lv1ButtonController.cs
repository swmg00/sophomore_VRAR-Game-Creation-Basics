using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lv1ButtonController : MonoBehaviour
{
    public Button PlayerUp; //플레이어 업그레이드 버튼
    public Button Quit; //종료 버튼
    public Button Next; //다음 레벨로 이동 버튼
    public Button Attack; //공격 버튼

    public Text PlayerUpgradeText; //플레이어 업그레이드 텍스트
    public Color disabledColor = Color.gray; //비활성화 상태일 때의 버튼 색상

    public AudioClip Button;
    private AudioSource playerAudio;
    private bool isAttackExecuting = false; //공격이 실행 중인지를 나타내는 플래그


    private bool playerUpClicked = false; //플레이어 업그레이드 버튼이 클릭되었는지를 나타내는 플래그 


    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        playerUpClicked = true;

        Next.interactable = false; //시작할때는 Next 버튼 비활성화 상태로 설정 
    }

    //종료 버튼 동작
    public void Exit()
    {
        playerAudio.Play();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    //다음 레벨로 이동 버튼 동작
    public void NextLevel()
    {
        if (playerUpClicked)
        {
            playerAudio.Play();
            SceneManager.LoadScene("Level 2"); //"Level 2"씬으로 이동
        }
    }


    //공격 버튼 동작 
    public void attack()
    {
        if (!isAttackExecuting)
        {
            isAttackExecuting = true;

            PlayerController.instance.fDown = true;  //플레이어 컨트롤러에게 공격 플래그 설정
            PlayerController.instance.playerAttack(); //플레이어 공격 실행 

            StartCoroutine(WaitForAttackCompletion()); //공격 완료를 기다리는 코루틴 시작 
        }
    }

    //공격 버튼에서 손을 떼었을 때 호출되는 메서드 
    public void OnAttackButtonUp()
    {
        PlayerController.instance.fDown = false;
    }


    //공격이 완료될 때까지 기다리는 코루틴   
    IEnumerator WaitForAttackCompletion()
    {
        //일정 시간 대기 후 다시 공격할 수 있게 함 
        yield return new WaitForSeconds(0.1f);

        // 플래그를 초기화하여 다시 공격할 수 있게 함 
        isAttackExecuting = false;
        PlayerController.instance.ResetAttackFlag();
    }

    //플레이어 업그레이드 버튼 클릭 시 호출되는 메서드 
    public void OnPlayerUpButtonClick()
    {
        if (ScoreManager.instance.coin >= 13)
        {
            //코인 감소 및 텍스트 업데이트
            ScoreManager.instance.coin = ScoreManager.instance.coin - 10;
            ScoreManager.instance.UpdateCoinText();
            GameManager.Instance.playerCoins = ScoreManager.instance.coin - 10;
            
            playerAudio.Play();
            
            // PlayerUpgradeText를 보이게 설정
            PlayerUpgradeText.gameObject.SetActive(true);

            // PlayerUp 버튼을 비활성화
            PlayerUp.interactable = false;

            // 버튼 색상을 회색으로 변경
            ColorBlock colors = PlayerUp.colors;
            colors.normalColor = disabledColor;
            PlayerUp.colors = colors;

            // Next 버튼을 활성화합니다.
            Next.interactable = true;

            // PlayerUp 버튼이 클릭되었음을 나타내는 플래그 설정
            playerUpClicked = true;
        }
        else
        {
            PlayerUp.interactable = false; //코인이 부족할 경우 버튼 비활성화
        }


    }


}