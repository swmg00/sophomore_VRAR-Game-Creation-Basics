using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LvBOSSButtonController : MonoBehaviour
{

    public Button Bonus;
    public Button Quit;
    public Button Attack;

    public AudioClip Button;
    private AudioSource playerAudio;

    private bool isAttackExecuting = false;


    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
      
    }

    public void quit()
    {
        playerAudio.Play();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    public void NextLevel()
    {
        playerAudio.Play();

        
        SceneSwitcher.instance.SwitchScene("Level Bonus");
    }

    public void attack()
    {
        if (!isAttackExecuting)
        {
            isAttackExecuting = true;

            PlayerControllerLvBOSS.iNstance.fDown = true;
            PlayerControllerLvBOSS.iNstance.playerAttack();

            StartCoroutine(WaitForAttackCompletion());
        }
    }

    public void OnAttackButtonUp()
    {
        PlayerControllerLvBOSS.iNstance.fDown = false;
    }
    IEnumerator WaitForAttackCompletion()
    {
        yield return new WaitForSeconds(0.1f);

        isAttackExecuting = false;
        PlayerControllerLvBOSS.iNstance.ResetAttackFlag();
    }





}

    
