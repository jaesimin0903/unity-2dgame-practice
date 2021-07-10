using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager1 : MonoBehaviour
{
    public float levelStartDelay = 2f;//������ �����ϱ� �� �ʴ����� ����� �ð�
    public float turnDelay = 0.1f;
    public static GameManager1 instance = null; // �̱��� (�ΰ��� ���ӸŴ����� �߻����� �ʵ���) �� ���� ����
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true; // �ۺ������� �����Ϳ����� �������� �ϱ� ���� [HideInInspector]


    private Text levelText;//Day 1 �ؽ�Ʈ
    private GameObject levelImage;//�̹��� ���۷��� ����
    private bool doingSetup = true;//���� ���带 ����� ������ üũ, ����� ���̶�� �÷��̾ �����̴� ���� ����
    private int level = 2; //f���� 2���� ������
    private List<Enemy> enemies;//������ ��ġ ��� ����, ���
    private bool enemiesMoving;
    private BoardManager boardScript;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject); // �ΰ��� ���ӸŴ����� �ı�

        DontDestroyOnLoad(gameObject); // ������ ����ϱ����� ���� �Ѿ�� ���ӿ�����Ʈ�� �������� �ʵ��� ��
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
        doingSetup = true;//�÷��̾� ������ �� ����

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay); //Ÿ��Ʋ ī�尡 ������ �� ���� �� ���� 2�� ���

        enemies.Clear();// ���� ���� ���� ����
        boardScript.SetupScene(level); // ���� ��ũ��Ʈ�� �����ϴ� ���� �� �������� �˷��� �� �ִ�.
    }
   
    void HideLevelImage()//������ ������ �غ� ���� �� ���� �̹����� ���� �Լ�, Init game invoke, invoke : ���� �ð� �Ŀ� �Լ� ȣ�� 
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
        if (playersTurn || enemiesMoving || doingSetup)// �÷��̾� �� Ȥ�� ���� �̵����̶�� ���� ���
            return;

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)//���Ӹ޴����� ������ �����̵��� ���
    {
        enemies.Add(script);
    }
    // Update is called once per frame
    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if(enemies.Count == 0)// ���� ���� Ȯ��
        {
            yield return new WaitForSeconds(turnDelay);//���� ������ �÷��̾�� ��ٸ���
        }

        for(int i = 0;i<enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime); // ���� ���� ȣ���ϱ� ���� movetime ������ �Է��Ͽ� ���
        }

        playersTurn = true;
        enemiesMoving = false;
    }


}
