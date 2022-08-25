using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm; //싱글톤 객체 선언
    public GameObject gameLabel; //게임 상태 UI오브젝트 변수
    Text gameText; // 게임 상태 UI오브젝트 컴포넌트 변수
    PlayerMove player;

    private void Awake()
    {
        if (gm == null) gm = this; // 싱글톤 객체 생성
    }

    public enum GameState // 게임 상태 열거형 변수
    {
        Ready,
        Run,
        GameOver
    }

    public GameState gState;

    // Start is called before the first frame update
    void Start()
    {
        gState = GameState.Ready; //초기상태는 Ready
        gameText = gameLabel.GetComponent<Text>(); // 컴포넌트 할당
        gameText.text = "Ready"; // 텍스트 내용 변경
        gameText.color = new Color32(255,185,0,255); // 색상 변경

        StartCoroutine(ReadyToStart());

        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    IEnumerator ReadyToStart(){
        yield return new WaitForSeconds(2f);
        gameText.text = "Go!";
        yield return new WaitForSeconds(0.5f);
        gameLabel.SetActive(false);
        gState = GameState.Run;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.hp <= 0)
        {
            // 플레이어의 애니메이션을 멈춘다.
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);
            gameLabel.SetActive(true); //상태 텍스트 활성화
            gameText.text = "Game Over"; 
            gameText.color = new Color32(255,0 ,0, 255);
            gState = GameState.GameOver;
        }
    }
}
