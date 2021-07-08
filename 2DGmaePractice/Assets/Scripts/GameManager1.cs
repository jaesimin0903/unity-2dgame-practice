using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager1 : MonoBehaviour
{
    public BoardManager boardScript;
    public static GameManager1 instance = null; // 싱글턴 (두개의 게임매니저가 발생하지 않도록) 을 위한 변수
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true; // 퍼블릭이지만 에디터에서는 숨겨지게 하기 위한 [HideInInspector]

    private int level = 3; //f레벨 2부터 적등장

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject); // 두개의 게임매니저면 파괴

        DontDestroyOnLoad(gameObject); // 점수를 계산하기위해 신이 넘어갈때 게임오브젝트를 제거하지 않도록 함
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        boardScript.SetupScene(level); // 보드 스크립트에 구성하는 씬이 몇 레벨인지 알려줄 수 있다.
    }

    public void GameOver()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
