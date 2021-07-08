using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public int wallDamage = 1; //플레이어가 가하는 공격력
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    private Animator animator; //에니메이터 레퍼런스를 가져오기 위한 변수
    private int food;

    // Start is called before the first frame update
    protected override void Start() //MovingObject 에 있는 start 와 다르게 구현하기 위해 오버라이딩
    {
        animator = GetComponent<Animator>();
        food = GameManager1.instance.playerFoodPoints; // 해당레벨동안 음식점수를 관리할 수 있음, 레벨이 바뀔 때 마다 게임메니저에 저장 

        base.Start(); // 부모 클래스의 스타트 호출
    }

    private void OnDisable()
    {
        GameManager1.instance.playerFoodPoints = food;  //food 를 게임메니저에 저장하는 과정  
    }

    void Update()
    {
        if (!GameManager1.instance.playersTurn) return; //플레이어의 턴이 아니라면 종료

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertical = 0;//플레이어가 대각선으로 움직이는 것을 막음

        if (horizontal != 0 || vertical != 0)// 움직이려고 하면 
            AttemptMove<Wall>(horizontal, vertical);//벽에 부딪힐 때(움직이려는 방향으로)
    }

    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        food--;//움직이면 푸드 -1

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit; // 충돌여부 변수 

        CheckIfGameOver();

        GameManager1.instance.playersTurn = false;
    }

    private void OnTriggerEnter2D (Collider2D other)//출구, 소다 , 음식의 태그를 트리거로 설정했으므로 태그를 체크
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);//restartLevelDelay 만큼 정지 후 함수 호출 
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            other.gameObject.SetActive(false);//푸드, 소다 오브젝트를 비활성화
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnCantMove <T> (T component)//wall 에 의해 block 되는 경우를 표현 
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);//플레이어가 벽에 얼마나 데미지를 가하는지
        animator.SetTrigger("playerChop");//애니메이션 트리거
    }

    private void Restart()//Exit 오브젝트와 충돌시 발생하는 함수
    {
        SceneManager.GetActiveScene(); // 레벨을 다시 불러옴 , 이 게임에는 하나 밖에 없는 main 신을 불러옴, 많은 게임들이 이 함수를 이용해 다른 신을 불러온다
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
