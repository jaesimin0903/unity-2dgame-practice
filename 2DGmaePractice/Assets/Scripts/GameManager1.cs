using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager1 : MonoBehaviour
{
    public float levelStartDelay = 2f;//레벨이 시작하기 전 초단위로 대기할 시간
    public float turnDelay = 0.1f;
    public static GameManager1 instance = null; // 싱글턴 (두개의 게임매니저가 발생하지 않도록) 을 위한 변수
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true; // 퍼블릭이지만 에디터에서는 숨겨지게 하기 위한 [HideInInspector]


    private Text levelText;//Day 1 텍스트
    private GameObject levelImage;//이미지 레퍼런스 저장
    private bool doingSetup = true;//게임 보드를 만드는 중인지 체크, 만드는 중이라면 플레이어가 움직이는 것을 방지
    private int level = 2; //f레벨 2부터 적등장
    private List<Enemy> enemies;//적들의 위치 계속 추적, 명령
    private bool enemiesMoving;
    private BoardManager boardScript;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject); // 두개의 게임매니저면 파괴

        DontDestroyOnLoad(gameObject); // 점수를 계산하기위해 신이 넘어갈때 게임오브젝트를 제거하지 않도록 함
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
       
        InitGame();
    }

 
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    /* void OnDisable()
     {
         SceneManager.sceneLoaded -= OnSceneLoaded;
     }
     void OnEnable()
     {
         SceneManager.sceneLoaded += OnSceneLoaded;

     }

     private void OnSceneLoaded(Scene arg1, LoadSceneMode arg2)
     {
         level++;
         InitGame();
     }*/
   

    //This is called each time a scene is loaded.
     private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log("here");
       level++;
       InitGame();
    }
    private void OnEnable()
    {
        //register the callback to be called everytime the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    private void OnDisable()
    {
        //register the callback to be called everytime the scene is loaded
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }

    void InitGame()
    {
        doingSetup = true;//플레이어 움직일 수 없음

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay); //타이틀 카드가 보여진 뒤 꺼질 때 까지 2초 대기

        enemies.Clear();// 이전 레벨 적들 정리
        boardScript.SetupScene(level); // 보드 스크립트에 구성하는 씬이 몇 레벨인지 알려줄 수 있다.
    }
   
    void HideLevelImage()//레벨을 시작할 준비가 됐을 때 레벨 이미지를 끄는 함수, Init game invoke, invoke : 지연 시간 후에 함수 호출 
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "After " + level + " days, you starved.";
        levelImage.SetActive(true);
        enabled = false;
    }


    void Update()
    {
        if (playersTurn || enemiesMoving || doingSetup)// 플레이어 턴 혹은 적이 이동중이라면 실행 취소
            return;

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)//게임메니저가 적들이 움직이도록 명령
    {
        enemies.Add(script);
    }
    // Update is called once per frame
    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if(enemies.Count == 0)// 적의 유무 확인
        {
            yield return new WaitForSeconds(turnDelay);//적이 없지만 플레이어는 기다리게
        }

        for(int i = 0;i<enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime); // 다음 적을 호출하기 전에 movetime 변수를 입력하여 대기
        }

        playersTurn = true;
        enemiesMoving = false;
    }


}
