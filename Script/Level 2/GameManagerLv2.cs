using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;


public class GameManagerLv2 : MonoBehaviour
{

    public static GameManagerLv2 iNstance;
    public GameObject gameCam;
    public PlayerControllerLv2 player;
    public ScoreManagerLv2 scoreManager2;

    public int playerCoins;
    public int stage2;
    public int coin;

    public float setTime = 90;
    public bool isBattle = true;
    public bool gameoverSoundPlayed = false;

    public GameObject GamePanel;

    public GameObject GameoverPanel;
    public GameObject Upgrade2Panel;

    public Text stageText;
    public Text playTimeText;
    public Text playerHealthText;

    public AudioClip Clear;
    public AudioClip Gameover;
    public List<AudioSource> audioSources = new List<AudioSource>();

    private int _player2Coins = 0;
    private SceneSwitcher sceneSwitcher;






    private void Awake()
    {
        stage2 = 2;

        if (isBattle != true)
            isBattle = true;

        if (iNstance == null)
        {
            iNstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // AudioSource를 추가합니다.
        audioSources.Add(gameObject.AddComponent<AudioSource>());
        audioSources.Add(gameObject.AddComponent<AudioSource>());

        // AudioClip을 할당합니다.
        audioSources[0].clip = Clear;
        audioSources[1].clip = Gameover;

        if (player != null)
        {
            player.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (isBattle)
        {
            setTime -= Time.deltaTime;

            if (stage2 == 2 && setTime <= 0)
            {
                Level2End();
            }
        }

    }


    public void GameStart()
    {
        player.gameObject.SetActive(true);
        isBattle = true;
        if (gameCam != null)
            gameCam.SetActive(true);

        if (player != null)
        {
            player.gameObject.SetActive(true);

        }


    }

    public void StageStart()
    {

        isBattle = true;

        if (GamePanel != null)
            GamePanel.SetActive(true);

        StartCoroutine(InBattle());
    }

    public void Stage2End()
    {
        player.gameObject.SetActive(false);

        if (ScoreManagerLv2.instance != null &&
            (ScoreManagerLv2.instance.monster1 >= 5 &&
            ScoreManagerLv2.instance.monster2 >= 10 &&
            ScoreManagerLv2.instance.coin >= 65))
        {
            audioSources[0].Play();
            Upgrade2Panel.SetActive(true);
        }
        else
        {
            audioSources[1].Play();
            GameoverPanel.SetActive(true);
        }
    }




    void Level2End()
    {
        isBattle = false;
        if (!gameoverSoundPlayed)
        {
            Stage2End();
            gameoverSoundPlayed = true;

        }
    }

    IEnumerator InBattle()
    {
        yield return new WaitForSeconds(0.2f);

    }

    void LateUpdate()
    {
        //상단UI 
        stageText.text = "STAGE " + stage2;
        int min = (int)setTime / 60;
        int sec = (int)setTime % 60;

        // 플레이어 UI
        playTimeText.text = string.Format("00:{0:00}:{1:00}", min, sec);
        playerHealthText.text = player.health + " /  " + player.maxHealth;


    }
}