using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager1 : MonoBehaviour
{
    public float turnDelay = .1f;
    public BoardManager boardScript;
    public static GameManager1 instance = null; // �̱��� (�ΰ��� ���ӸŴ����� �߻����� �ʵ���) �� ���� ����
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true; // �ۺ������� �����Ϳ����� �������� �ϱ� ���� [HideInInspector]

    private int level = 3; //f���� 2���� ������
    private List<Enemy> enemies;//������ ��ġ ��� ����, ���
    private bool enemiesMoving;

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

    void InitGame()
    {
        enemies.Clear();// ���� ���� ���� ����
        boardScript.SetupScene(level); // ���� ��ũ��Ʈ�� �����ϴ� ���� �� �������� �˷��� �� �ִ�.
    }

    public void GameOver()
    {
        enabled = false;
    }


    void Update()
    {
        if (playersTurn || enemiesMoving)// �÷��̾� �� Ȥ�� ���� �̵����̶�� ���� ���
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
