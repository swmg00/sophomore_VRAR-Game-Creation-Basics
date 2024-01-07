using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int coin;
    public int monster1;
    public int monster2 = 0;

    public Text coinText;
    public Text monster1Text;
    public Text monster2Text;
    public Text MissionText;
    public Text MissionClearText;
    public Text scoreText;

    private int score;
    private int resultScore;
    private int mon1Score;
    private int mon2Score;

    private void Awake()
    {
        // Singleton 패턴 적용
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
        score = 0;
        monster1 = 0;
        monster2 = 0;
        resultScore = 0;
        mon1Score = 0;
        mon2Score = 0;
        coin = 0;

        // 스테이지에 따라 미션 텍스트 설정
        if (GameManager.Instance != null && GameManager.Instance.stage == 1)
            MissionText.text = "미션: 슬라임 13마리  보석 13개";


        MissionText.gameObject.SetActive(true);
        MissionClearText.gameObject.SetActive(false);



        UpdateCoinText();
    }

    public void AddMonster(int mon1, int mon2)
    {
        // 스테이지에 따라 몬스터 추가 및 갱신
        if (GameManager.Instance != null && GameManager.Instance.stage == 1)
        {
            monster1 += mon1;
            monster2 = 0;

            UpdateMonster1Text();

        }
        // 스코어 갱신
        AddScore(monster1, monster2);
    }


    public void AddScore(int monster1, int monster2)
    {
        int scoreForMonster1 = monster1 * 100;
        int scoreForMonster2 = monster2 * 300;

        resultScore = scoreForMonster1 + scoreForMonster2;
        UpdateScoreText();
    }

    public void AddCoin(int amount)
    {
        coin += amount;
        UpdateCoinText();
    }

    public void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = coin.ToString();
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = resultScore.ToString();
        }
    }

    private void UpdateMonster1Text()
    {
        if (monster1Text != null)
        {
            monster1Text.text = "x " + monster1.ToString();

            // 몬스터와 코인 미션 클리어 조건 확인
            if (monster1 >= 13 && coin >= 13)
            {
                MissionText.gameObject.SetActive(false);
                MissionClearText.gameObject.SetActive(true);

            }
        }

    }
}
