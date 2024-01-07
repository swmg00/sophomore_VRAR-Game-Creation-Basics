using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameObject gameCam;
    public PlayerController player;
    public ScoreManager scoreManager;

    public int stage;
    public int playerCoins;
    public int monsterCnt1;


    public float playTime;
    public bool isBattle = true;
    public bool gameoverSoundPlayed = false;

    public GameObject GamePanel;
    public GameObject UpgradePanel;
    public GameObject GameoverPanel;


    public Text stageText;
    public Text playTimeText;
    public Text playerHealthText;



    public AudioClip Clear;
    public AudioClip Gameover;

    public List<AudioSource> audioSources = new List<AudioSource>();

    private int _stage = 1;
    private int _playerCoins = 0;
    private int _playerScore = 0;
    private SceneSwitcher sceneSwitcher;

    public int Stage
    {
        get { return _stage; }
        set { _stage = value; }
    }

    public int PlayerCoins
    {
        get { return _playerCoins; }
        set { _playerCoins = value; }
    }

    public int PlayerScore
    {
        get { return _playerScore; }
        set { _playerScore = value; }
    }

    private void Awake()
    {
        stage = 1;
        sceneSwitcher = GetComponent<SceneSwitcher>();
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
            playTime += Time.deltaTime;

            if (stage == 1 && playTime >= 60.0f)
            {
                Level1End();
            }

            playerCoins = ScoreManager.instance.coin;
        }
    }

    public void GameStart()
    {
        player.gameObject.SetActive(true);
        if (gameCam != null)
            gameCam.SetActive(true);

        if (player != null)
        {
            player.gameObject.SetActive(true);
            GameManager.Instance.playerCoins = ScoreManager.instance.coin;
        }
        else
            player.gameObject.SetActive(true);

    }

    public void StageStart()
    {
        player.gameObject.SetActive(true);
        isBattle = true;

        if (GamePanel != null)
            GamePanel.SetActive(true);

        StartCoroutine(InBattle());
    }
    public void Stage1End()
    {
        player.gameObject.SetActive(false);
        _playerCoins = GameManager.Instance.playerCoins;

        if (ScoreManager.instance.monster1 >= 13 && _playerCoins >= 26 && playTime < 61.0f)
        {
            _stage = stage;
            audioSources[0].Play();
            UpgradePanel.SetActive(true);
        }
        else
        {
            audioSources[1].Play();
            UpgradePanel.SetActive(false);
            GameoverPanel.SetActive(true);
        }
    }


    void Level1End()
    {

        isBattle = false;
        if (!gameoverSoundPlayed)
        {
            Stage1End();
            gameoverSoundPlayed = true;


        }
    }


    IEnumerator InBattle()
    {
        yield return new WaitForSeconds(0.2f);
        // StageEnd();
    }

    void LateUpdate()
    {
        //상단UI 
        stageText.text = "STAGE " + stage;

        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600) / 60);
        int second = 60 - (int)(playTime % 60);

        //플레이어UI
        playTimeText.text = "00:00 :" + string.Format("{0:00}", second);
        playerHealthText.text = player.health + " /  " + player.maxHealth;


    }
}