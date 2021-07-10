using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�Ϲ���(generic) �� ����ϴ� ���� �÷��̾�, ��, �� ���� ��ȣ�ۿ뿡�� ���μ��� hitComponent �� ������ �� �� ���� ������ ���� OnCantMove �Լ��� �Է��� �ް� ������ ����� Ŭ�������� ������ ���� �����ϰ� �� �� �ֱ� �����̴�.
public abstract class MovingObject : MonoBehaviour
{
    public float moveTime = 0.1f;
    public LayerMask blockingLayer; //�浹���� ����


    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D; //��ü�� ������ ���۷����� ���� ����
    private float inverseMoveTime; //�������� ȿ�����̰� ����ϱ� ���� ����
    private bool isMoving;

    // Start is called before the first frame update
    protected virtual void Start()//�ڽ� Ŭ������ ����Ἥ ������ �ϱ� ���� start �� �ٽ� ������ �����־
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime; //������ ��ſ� ȿ������ ���ϱ⸦ ����ϱ� ���� �������� ����ġ�� ����
    }

    protected bool Move (int xDir, int yDir, out RaycastHit2D hit)//bool ���ϰ�, hit ���ϰ� �ΰ��� ���ϰ��� ����
    {
        Vector2 start = transform.position; //������ġ�� �ޱ����� ����
        // position �� ���� 3���� ������ Vector 2�� ���������� z ���� ���󰡰Ե� 
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;//�ڱ��ڽŰ� �ε����� �ʱ� ���� boxCollider ����
        hit = Physics2D.Linecast(start, end, blockingLayer); // ���������� ������ ������ ������ �������� blockinglayer �� �浹�˻�
        boxCollider.enabled = true; 

        if(hit.transform == null && !isMoving)//�ε����� ���ٸ�
        {
            StartCoroutine(SmoothMovement(end));//end �������� �̵�
            return true;
        }
        return false;
    }


    protected IEnumerator SmoothMovement (Vector3 end)//��ü�� �����̰� �� �Լ� end�� ���� �������� ����
    {
        isMoving = true;
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude; // ������ġ - end ���Ϳ� sqrMagnitude �� �Ÿ���� Magnitude : ���ͱ��� , sqrMagnitude : ���ͱ��� ���� 

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime); // epsilon (0�� ����� �������� �� ) newposition �� ����Ͽ� end ���� ���尡��� ��ġ�� �̵���Ű�� ���� ����
            //(������ġ(rigidpositon), �̵�����ġ, �ð�(�� ������ ������ƴ �������� �������)) 
            rb2D.MovePosition(newPosition); // ���ο� ��ġ�� �̵�
            sqrRemainingDistance = (transform.position - end).sqrMagnitude; // ������ �� ���� �Ÿ� ��� 
            yield return null; //������ �����ϱ� ���� ���� �������� ��ٸ�
        }
        rb2D.MovePosition(end);
        isMoving = false;
    }

    protected virtual void AttemptMove <T> (int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);//�̵��ϴµ� �����ϸ� canMove = true else flase    

        if (hit.transform == null)//�ٸ��Ͱ� �ε����� �ʾҴٸ� �ڵ� ����
            return;

        T hitComponent = hit.transform.GetComponent<T>(); //�浿�� ���۷����� T Ÿ�� ������Ʈ�� �Ҵ� 

        if (!canMove && hitComponent != null)//�������� ���� -> �浹
            OnCantMove(hitComponent);
    }

    protected abstract void OnCantMove<T>(T component)
        where T : Component;// �ڽ� Ŭ�������� �������̵� ���� ���ܵ� 
}
