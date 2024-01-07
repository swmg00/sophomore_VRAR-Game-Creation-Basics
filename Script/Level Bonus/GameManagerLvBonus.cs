using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerLvBonus : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static GameManagerLvBonus Instance;

    // 게임 카메라와 플레이어
    public GameObject gameCam;
    public Player player;

    // 게임 패널과 클리어 패널
    public GameObject GamePanel;
    public GameObject ClearPanel;

    // 게임오버 사운드 재생 여부
    public bool gameoverSoundPlayed = false;

    // 채널 스위처
    private SceneSwitcher sceneSwitcher;

    // 클리어 사운드
    public AudioClip clear;
    AudioSource clearAudio;

    private void Awake()
    {
        // 채널 스위처 초기화
        sceneSwitcher = GetComponent<SceneSwitcher>();

        // 싱글톤 인스턴스 설정
        if (Instance == null)
            Instance = this;

        // 클리어 오디오 설정
        clearAudio = GetComponent<AudioSource>();
        clearAudio.clip = clear;

        // 플레이어가 존재하면 활성화
        if (player != null)
        {
            player.gameObject.SetActive(true);
        }

        // 게임 패널 활성화
        GamePanel.SetActive(true);
        // 클리어 패널 시작 시 비활성화
        ClearPanel.SetActive(false);
    }

    // 보너스 레벨 종료 처리 메서드
    public void LevelBonusEnd()
    {
        if (!gameoverSoundPlayed)
        {
            StageBonusEnd();
            StartCoroutine(EndBonusLevel());
            gameoverSoundPlayed = true;
        }
    }

    // 스테이지 종료 처리 메서드
    void StageBonusEnd()
    {
        clearAudio.Play();

        // 플레이어 비활성화 및 패널 전환
        player.gameObject.SetActive(false);
        GamePanel.SetActive(false);
        ClearPanel.SetActive(true);
    }

    // 보너스 레벨 종료 시 대기 및 어플리케이션 종료 메서드
    IEnumerator EndBonusLevel()
    {
        // 5초 대기
        yield return new WaitForSeconds(5f); // 5초 대기


#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
}