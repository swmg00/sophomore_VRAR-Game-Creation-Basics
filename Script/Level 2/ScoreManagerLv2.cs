using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManagerLv2 : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static ScoreManagerLv2 instance; // Singleton pattern

    // 게임 스코어 및 미션 관련 변수들
    public int coin;
    public int monster1;
    public int monster2;
    private int score;
    private int resultScore;
    private int mon1Score;
    private int mon2Score;

    // UI 텍스트 요소들
    public Text coinText;
    public Text monster1Text;
    public Text monster2Text;
    public Text MissionText;
    public Text MissionClearText;
    public Text scoreText;


    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 초기화
        score = 0;
        monster1 = 0;
        monster2 = 0;
        resultScore = 0;
        mon1Score = 0;
        mon2Score = 0;
        coin = 0;

        // 미션 텍스트 초기화
        MissionText.text = "미션: 슬라임 5마리 뿔슬라임 10마리 보석 65개";
        MissionText.gameObject.SetActive(true);
        MissionClearText.gameObject.SetActive(false);
        // 코인 텍스트 업데이트
        UpdateCoinText();
    }

    // 몬스터 스코어 및 미션 처리 메서드
    public void AddMonster(int mon1, int mon2)
    {
        if (GameManagerLv2.iNstance != null && GameManagerLv2.iNstance.stage2 == 2)
        {
            if (mon1 == 1 && mon2 == 0)
            {
                monster1 += mon1;
                UpdateMonster1Text();
            }
            else
            {
                monster2 += mon2;
                UpdateMonster2Text();
            }

            AddScore(monster1, monster2);
        }
    }

    // 스코어 누적 및 업데이트 메서드
    public void AddScore(int monster1, int monster2)
    {
        int scoreForMonster1 = monster1 * 100;
        int scoreForMonster2 = monster2 * 300;

        resultScore = scoreForMonster1 + scoreForMonster2; // score 누적이 아닌 새로운 값으로 설정

        UpdateScoreText();
    }

    // 코인 획득 메서드
    public void AddCoin(int amount)
    {
        coin += amount;
        UpdateCoinText();
    }

    // 코인 텍스트 업데이트 메서드
    public void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = coin.ToString();
        }
    }

    // 스코어 텍스트 업데이트 메서드
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = resultScore.ToString();
        }
    }

    // 몬스터1 텍스트 업데이트 메서드
    private void UpdateMonster1Text()
    {
        if (monster1Text != null)
        {
            monster1Text.text = "x " + monster1.ToString();
            Level2Clear();
        }

    }

    // 몬스터2 텍스트 업데이트 메서드
    private void UpdateMonster2Text()
    {
        if (monster2Text != null)
        {
            monster2Text.text = "x " + monster2.ToString();
            Level2Clear();
        }
    }

    // 레벨 2 클리어 여부 확인 및 미션 텍스트 업데이트 메서드
    void Level2Clear()
    {
        if (monster1 >= 5 && monster2 >= 10 && coin >= 65)
        {
            MissionText.gameObject.SetActive(false);
            MissionClearText.gameObject.SetActive(true);
        }
        else
            MissionText.gameObject.SetActive(true);
    }

}