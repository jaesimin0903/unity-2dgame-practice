using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager1 : MonoBehaviour
{
    public float turnDelay = .1f;
    public BoardManager boardScript;
    public static GameManager1 instance = null; // 싱글턴 (두개의 게임매니저가 발생하지 않도록) 을 위한 변수
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true; // 퍼블릭이지만 에디터에서는 숨겨지게 하기 위한 [HideInInspector]

    private int level = 3; //f레벨 2부터 적등장
    private List<Enemy> enemies;//적들의 위치 계속 추적, 명령
    private bool enemiesMoving;

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

    void InitGame()
    {
        enemies.Clear();// 이전 레벨 적들 정리
        boardScript.SetupScene(level); // 보드 스크립트에 구성하는 씬이 몇 레벨인지 알려줄 수 있다.
    }

    public void GameOver()
    {
        enabled = false;
    }


    void Update()
    {
        if (playersTurn || enemiesMoving)// 플레이어 턴 혹은 적이 이동중이라면 실행 취소
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
