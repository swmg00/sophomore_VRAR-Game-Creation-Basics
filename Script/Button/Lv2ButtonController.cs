using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lv2ButtonController : MonoBehaviour
{
    public Button PlayerUp;
    public Button PlayerHeartUp; //플레이어 체력 업그레이드 버튼 
    public Button Quit;
    public Button Next;
    public Button Attack; 

    public Text PlayerUpgradeText; //플레이어 업그레이드 텍스트 
    public Text PlayerHeartUpgradeText; //플레이어 체력 업그레이드 텍스트 

    public Color disabledColor = Color.gray;  //비활성화 상태일 때의 버튼 색상 

    public AudioClip Button; 
    private AudioSource playerAudio; 
    float displayTime = 1f; //텍스트가 표시되는 시간 

    private bool playerUpClicked = false;  //플레이어 업그레이드 버튼이 클리되었는지를 나타내는 플래그  
    private bool playerHeartUpClicked = false; //플레이어 체력 업그레이드 버튼이 클릭되었는지를 나타내는 플래그
    private bool isAttackExecuting = false; //공격이 실행 중인지를 나타내는 플래그 



    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        playerUpClicked = false;
        playerHeartUpClicked = false;

        Next.interactable = false; //시작할 때는 다음 레벨로 이동 버튼을 비활성화 상태로 설정
    }
   
    //다음 레벨로 이동 버튼 동작
    public void NextLevel()
    {
        playerAudio.Play();
        SceneSwitcher.instance.SwitchScene("Level BOSS");//"Level BOSS"씬으로 이동
    }

    //공격 버튼 동작
    public void attack()
    {
        if (!isAttackExecuting)
        {
            isAttackExecuting = true;

            PlayerControllerLv2.instance.fDown = true; //플레이어 컨트롤러 공격 플래그 설정
            PlayerControllerLv2.instance.playerAttack(); //플레이어 공격 실행

            StartCoroutine(WaitForAttackCompletion()); //공격 완료를 기다리는 코루틴 시작
        }
    }

    //공격 버튼에서 손을 떼었을 때 호출되는 메서드
    public void OnAttackButtonUp()
    {
        PlayerControllerLv2.instance.fDown = false; //플레이어 컨트롤러에게 공격 플래그 해제 
    }

    //공격이 완료될 때까지 기다리는 코루틴
    IEnumerator WaitForAttackCompletion()
    {
        //일정 시간 대기 후 다시 공격을 허용 
        yield return new WaitForSeconds(0.1f);

        //플래그를 초기화하여 다시 공격할 수 있게 함 
        isAttackExecuting = false;
        PlayerControllerLv2.instance.ResetAttackFlag();
    }

   //종료 버튼 동작
    public void quit()
    {
        playerAudio.Play();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    //플레이어 업그레이드 버튼 클릭 시 호출되는 메서드
    public void OnPlayerUpButtonClick()
    {
        if (ScoreManagerLv2.instance.coin >= 25)
        {
            //코인 감소 및 텍스트 업데이트 
            ScoreManagerLv2.instance.coin = ScoreManagerLv2.instance.coin - 25;
            ScoreManagerLv2.instance.UpdateCoinText();
            GameManagerLv2.iNstance.playerCoins = ScoreManagerLv2.instance.coin - 25;
            playerAudio.Play();

            // PlayerUpgradeText를 보이게 설정
            PlayerUpgradeText.gameObject.SetActive(true);

            // PlayerUp 버튼을 비활성화
            PlayerUp.interactable = false;


            // 버튼 색상을 회색으로 변경
            ColorBlock colors = PlayerUp.colors;
            colors.normalColor = disabledColor;
            PlayerUp.colors = colors;

            // PlayerUp 버튼이 클릭되었음을 나타내는 플래그 설정
            playerUpClicked = true;
            StartCoroutine(DisappearDelay());

        }
        else
        {
            PlayerUpgradeText.text = "보석이 부족합니다.";
            PlayerUpgradeText.gameObject.SetActive(true);
        }
    }

    //플레이어 체력 업그레이드 버튼 클릭 시 호출되는 메서드 
    public void OnPlayerHeartUpButtonClick()
    {

        if (ScoreManagerLv2.instance.coin >= 30)
        {
            //코인 감소 및 텍스트 엄데이트 
            ScoreManagerLv2.instance.coin = ScoreManagerLv2.instance.coin - 40;
            ScoreManagerLv2.instance.UpdateCoinText();
            GameManagerLv2.iNstance.playerCoins = ScoreManagerLv2.instance.coin - 40;
           
            playerAudio.Play(); 
            
            // PlayerUpgradeText를 보이게 설정
            PlayerHeartUpgradeText.gameObject.SetActive(true);

            // PlayerUp 버튼을 비활성화
            PlayerHeartUp.interactable = false;

            // 버튼 색상을 회색으로 변경
            ColorBlock colors = PlayerHeartUp.colors;
            colors.normalColor = disabledColor;
            PlayerHeartUp.colors = colors;

            // PlayerUp 버튼이 클릭되었음을 나타내는 플래그 설정
            playerHeartUpClicked = true;
            StartCoroutine(DisappearDelay());

        }
        else
        {
            PlayerHeartUpgradeText.text = "보석이 부족합니다.";
            PlayerHeartUpgradeText.gameObject.SetActive(true);
        }

    }



    IEnumerator DisappearDelay()
    {
        yield return new WaitForSeconds(displayTime);

        if (playerHeartUpClicked == true)
        {
            PlayerHeartUpgradeText.enabled = false;
        }
        else if (playerUpClicked == true)
        {
            PlayerUpgradeText.enabled = false;
        }

        next();

    }

    public void next()
    {

        if ((playerHeartUpClicked == true) && (playerUpClicked == true))
            Next.interactable = true;
        else
            Next.interactable = false;
    }

}


