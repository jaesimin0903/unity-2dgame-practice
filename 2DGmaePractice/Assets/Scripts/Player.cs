using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public int wallDamage = 1; //�÷��̾ ���ϴ� ���ݷ�
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    private Animator animator; //���ϸ����� ���۷����� �������� ���� ����
    private int food;

    // Start is called before the first frame update
    protected override void Start() //MovingObject �� �ִ� start �� �ٸ��� �����ϱ� ���� �������̵�
    {
        animator = GetComponent<Animator>();
        food = GameManager1.instance.playerFoodPoints; // �ش緹������ ���������� ������ �� ����, ������ �ٲ� �� ���� ���Ӹ޴����� ���� 

        base.Start(); // �θ� Ŭ������ ��ŸƮ ȣ��
    }

    private void OnDisable()
    {
        GameManager1.instance.playerFoodPoints = food;  //food �� ���Ӹ޴����� �����ϴ� ����  
    }

    void Update()
    {
        if (!GameManager1.instance.playersTurn) return; //�÷��̾��� ���� �ƴ϶�� ����

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertical = 0;//�÷��̾ �밢������ �����̴� ���� ����

        if (horizontal != 0 || vertical != 0)// �����̷��� �ϸ� 
            AttemptMove<Wall>(horizontal, vertical);//���� �ε��� ��(�����̷��� ��������)
    }

    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        food--;//�����̸� Ǫ�� -1

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit; // �浹���� ���� 

        CheckIfGameOver();

        GameManager1.instance.playersTurn = false;
    }

    private void OnTriggerEnter2D (Collider2D other)//�ⱸ, �Ҵ� , ������ �±׸� Ʈ���ŷ� ���������Ƿ� �±׸� üũ
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);//restartLevelDelay ��ŭ ���� �� �Լ� ȣ�� 
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            other.gameObject.SetActive(false);//Ǫ��, �Ҵ� ������Ʈ�� ��Ȱ��ȭ
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnCantMove <T> (T component)//wall �� ���� block �Ǵ� ��츦 ǥ�� 
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);//�÷��̾ ���� �󸶳� �������� ���ϴ���
        animator.SetTrigger("playerChop");//�ִϸ��̼� Ʈ����
    }

    private void Restart()//Exit ������Ʈ�� �浹�� �߻��ϴ� �Լ�
    {
        SceneManager.GetActiveScene(); // ������ �ٽ� �ҷ��� , �� ���ӿ��� �ϳ� �ۿ� ���� main ���� �ҷ���, ���� ���ӵ��� �� �Լ��� �̿��� �ٸ� ���� �ҷ��´�
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
            GameManager1.instance.GameOver();
    }
}
