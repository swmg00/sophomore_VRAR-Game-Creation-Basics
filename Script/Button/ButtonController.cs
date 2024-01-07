using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ButtonController : MonoBehaviour
{
    public GameObject Ruleimage;  //규칙 이미지를 표시할 게임 오브젝트
    public Button Rule; //규칙 버튼
    public Button Close; //닫기 버튼

    public AudioClip Button; //버튼 클릭 시 재생할 오디오 클립 
    AudioSource playerAudio; //오디오 소스 컴포넌트

    void Start()
    {
        Ruleimage.SetActive(false); //시작할 때는 규칙 이미지를 비활성화 상태로 설정

        playerAudio = GetComponent<AudioSource>(); // 자신에게 부착된 오디오 소스 컴포넌트 가져오기

    }

    //종료 버튼 동작
    public void Exit()
    {
        playerAudio.Play(); //버튼 클릭 사운드 재생
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    //다음 레벨로 이동 버튼 동작
    public void Next()
    {
        playerAudio.Play();
        SceneManager.LoadScene("Level 1");  //Level 1"씬으로 이동 
    }

    //규칙 보기 버튼 동작
    public void ShowRule()
    {
        playerAudio.Play();
        Close.interactable = true; //닫기 버튼 활성화
        Ruleimage.SetActive(true); //규칙 이미지 활성화
    }

    //규칙 닫기 버튼 동작
    public void CloseRule()
    {
        playerAudio.Play();
        Ruleimage.SetActive(false);  //규칙 이미지 비활성화
    }
}
