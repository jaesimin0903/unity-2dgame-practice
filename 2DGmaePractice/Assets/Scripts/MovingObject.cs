using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//일반형(generic) 을 사용하는 이유 플레이어, 적, 벽 과의 상호작용에서 서로서로 hitComponent 의 종류를 알 수 없기 때문에 당장 OnCantMove 함수에 입력을 받고 각각의 상속한 클래스들의 구현에 따라 동작하게 할 수 있기 때문이다.
public abstract class MovingObject : MonoBehaviour
{
    public float moveTime = 0.1f;
    public LayerMask blockingLayer; //충돌여부 변수


    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D; //객체의 움직임 레퍼런스를 담을 변수
    private float inverseMoveTime; //움직임을 효율적이게 계산하기 위한 변수
    private bool isMoving;

    // Start is called before the first frame update
    protected virtual void Start()//자식 클래스가 덮어써서 재정의 하기 위해 start 를 다시 정의할 수도있어서
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime; //나누기 대신에 효율적인 곱하기를 사용하기 위해 움직임의 역수치를 저장
    }

    protected bool Move (int xDir, int yDir, out RaycastHit2D hit)//bool 리턴값, hit 리턴값 두개의 리턴값을 가짐
    {
        Vector2 start = transform.position; //현재위치를 받기위한 변수
        // position 은 기존 3차원 이지만 Vector 2에 저장함으로 z 값은 날라가게됨 
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;//자기자신과 부딛히지 않기 위해 boxCollider 해제
        hit = Physics2D.Linecast(start, end, blockingLayer); // 시작지점과 끝지점 까지의 라인을 가져오고 blockinglayer 와 충돌검사
        boxCollider.enabled = true; 

        if(hit.transform == null && !isMoving)//부딪힌게 없다면
        {
            StartCoroutine(SmoothMovement(end));//end 방향으로 이동
            return true;
        }
        return false;
    }


    protected IEnumerator SmoothMovement (Vector3 end)//객체를 움직이게 할 함수 end에 어디로 움직일지 저장
    {
        isMoving = true;
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude; // 현재위치 - end 벡터에 sqrMagnitude 로 거리계산 Magnitude : 벡터길이 , sqrMagnitude : 벡터길이 제곱 

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime); // epsilon (0에 가까운 아주작은 수 ) newposition 을 계산하여 end 값과 가장가까운 위치로 이동시키기 위한 변수
            //(현재위치(rigidpositon), 이동할위치, 시간(이 변수의 단위만틈 목적지로 가까워짐)) 
            rb2D.MovePosition(newPosition); // 새로운 위치로 이동
            sqrRemainingDistance = (transform.position - end).sqrMagnitude; // 움직인 후 남은 거리 계산 
            yield return null; //루프를 갱신하기 전에 다음 프레임을 기다림
        }
        rb2D.MovePosition(end);
        isMoving = false;
    }

    protected virtual void AttemptMove <T> (int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);//이동하는데 성공하면 canMove = true else flase    

        if (hit.transform == null)//다른것과 부딪히지 않았다면 코드 종료
            return;

        T hitComponent = hit.transform.GetComponent<T>(); //충동할 레퍼런스를 T 타입 컴포넌트에 할당 

        if (!canMove && hitComponent != null)//움직임이 막힘 -> 충돌
            OnCantMove(hitComponent);
    }

    protected abstract void OnCantMove<T>(T component)
        where T : Component;// 자식 클래스에서 오버라이드 위해 남겨둠 
}
