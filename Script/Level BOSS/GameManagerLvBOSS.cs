using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManagerLvBOSS : MonoBehaviour
{
    // GameManagerLvBOSS의 단일 인스턴스
    public static GameManagerLvBOSS Instance;

    // 게임 오브젝트 및 캐릭터 변수
    public GameObject gameCam;
    public PlayerControllerLvBOSS player;
    public MonsterLv3 boss;

    // 플레이어와 보스의 코인 변수
    public int playerCoins;
    public int bossCoin = 1;
    public int coin;

    // 전투 관련 변수
    public float setTime = 60;
    public bool isBattle = true;
    public bool gameoverSoundPlayed = false;

    // UI 패널 변수
    public GameObject GamePanel;
    public GameObject GameoverPanel;
    public GameObject Upgrade3Panel;

    // UI Text 변수
    public Text stageText;
    public Text playTimeText;
    public Text playerHealthText;
    public Text bossHealthText;
    public Text BossAttackText;

    // 오디오 및 사운드 변수
    public AudioClip Clear;
    public AudioClip Gameover;

    // 오디오 소스 목록
    public List<AudioSource> audioSources = new List<AudioSource>();

    // SceneSwitcher 참조 변수
    private SceneSwitcher sceneSwitcher;


    private void Awake()
    {

        BossAttackText.gameObject.SetActive(true);
        sceneSwitcher = GetComponent<SceneSwitcher>();

        // 전투 중인지 확인 후 초기화
        if (isBattle != true)
            isBattle = true;

        // 싱글톤 패턴 적용
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // AudioSource 추가 및 AudioClip 할당
        audioSources.Add(gameObject.AddComponent<AudioSource>());
        audioSources.Add(gameObject.AddComponent<AudioSource>());
        audioSources[0].clip = Clear;
        audioSources[1].clip = Gameover;

        // 플레이어가 존재하면 활성화
        if (player != null)
        {
            player.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (isBattle)
        {
            // 남은 시간 감소
            setTime -= Time.deltaTime;

            // 보스 체력이 0 이하거나 (시간 종료와 함께 보스 체력이 0 이하)일 때
            if (boss.health <= 0 || (setTime <= 0 && boss.health <= 0))
            {
                LevelBOSSEnd();
            }
            // 시간이 0 이하이고 보스 체력이 0 이상이거나, 플레이어 체력이 0 이하일 때
            if ((setTime <= 0 && boss.health >= 0) || player.health == 0)
            {
                GameEnd();
            }

        }
    }

    // 게임 종료 처리
    void GameEnd()
    {
        if (!gameoverSoundPlayed)
        {
            // 플레이어 비활성화 및 UI 설정
            player.gameObject.SetActive(false);
            GamePanel.gameObject.SetActive(false);
            audioSources[1].Play();
            GameoverPanel.gameObject.SetActive(true);
            gameoverSoundPlayed = true;
        }
    }

    // 게임 시작 처리
    public void GameStart()
    {
        // 플레이어 활성화 및 전투 상태 설정
        player.gameObject.SetActive(true);
        isBattle = true;

        // 게임 카메라 활성화
        if (gameCam != null)
            gameCam.SetActive(true);

        // 플레이어가 null이 아니면 활성화
        if (player != null)
        {
            player.gameObject.SetActive(true);
        }
    }

    // 스테이지 시작 처리
    public void StageStart()
    {
        // 전투 상태 설정
        isBattle = true;

        // 게임 패널 활성화
        if (GamePanel != null)
            GamePanel.SetActive(true);

        // InBattle 코루틴 시작
        StartCoroutine(InBattle());
    }

    // 스테이지 보스 종료 처리
    public void StageBOSSEnd()
    {
        // 플레이어 비활성화
        player.gameObject.SetActive(false);

        // 보스 체력이 0 이하이거나 (시간 종료와 함께 보스 체력이 0 이하)일 때
        if (boss.health <= 0 || (setTime <= 0 && boss.health <= 0))
        {
            // 클리어 사운드 재생 및 업그레이드 패널 활성화
            audioSources[0].Play();
            Upgrade3Panel.SetActive(true);
        }
        else
        {
            // 게임 종료 처리
            GameEnd();
        }
    }

    // 레벨 보스 종료 처리
    public void LevelBOSSEnd()
    {
        // 전투 상태 해제
        isBattle = false;
        if (!gameoverSoundPlayed)
        {
            // 스테이지 보스 종료 함수 호출 및 게임 오버 사운드 플래그 설정
            StageBOSSEnd();  
            gameoverSoundPlayed = true; 
        }
    }

    // 전투 중 코루틴
    IEnumerator InBattle()
    {
        yield return new WaitForSeconds(0f);
    }

    // 플레이어 체력 업데이트
    public void updatePlayerHealth(int playerHealth)
    {
        if (player.health <= 0)
            player.health = 0;
        else
            player.health = playerHealth;
    }

    // 보스 체력 업데이트
    public void updateBossHealth(int bossHealth)
    {
        if (bossHealth <= 0)
            boss.health = 0;
        else
            boss.health = bossHealth;
    }

    // 보스가 죽었을 때 처리
    public void BossDied()
    {
        if (isBattle)
        {
            // 전투 종료 및 게임 오버 사운드 플래그 설정
            isBattle = false;

            if (!gameoverSoundPlayed)
            {
                StageBOSSEnd();
                gameoverSoundPlayed = true;
            }
        }
    }

    /*
    // 보스 공격 처리
    public void bossAttack()
    {
        if ((int)setTime == 30)
        {
            BossAttackText.gameObject.SetActive(true);
            audioSources[2].Play();
            int result = player.health - 75;
          // updatePlayerHealth(result);

        }
    }
    */


    void LateUpdate()
    {
        //상단UI 
        int min = (int)setTime / 60;
        int sec = (int)setTime % 60;

        // 플레이어 UI
        playTimeText.text = string.Format("00:{0:00}:{1:00}", min, sec);
        playerHealthText.text = player.health + " /  " + player.maxHealth;



        //몬스터 숫자UI 
        bossHealthText.text = boss.health + " /  " + boss.maxHealth;
        bossHealthText.gameObject.SetActive(true);

    }
}