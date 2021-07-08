using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager1 : MonoBehaviour
{
    public BoardManager boardScript;
    public static GameManager1 instance = null; // �̱��� (�ΰ��� ���ӸŴ����� �߻����� �ʵ���) �� ���� ����
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true; // �ۺ������� �����Ϳ����� �������� �ϱ� ���� [HideInInspector]

    private int level = 3; //f���� 2���� ������

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject); // �ΰ��� ���ӸŴ����� �ı�

        DontDestroyOnLoad(gameObject); // ������ ����ϱ����� ���� �Ѿ�� ���ӿ�����Ʈ�� �������� �ʵ��� ��
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        boardScript.SetupScene(level); // ���� ��ũ��Ʈ�� �����ϴ� ���� �� �������� �˷��� �� �ִ�.
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
