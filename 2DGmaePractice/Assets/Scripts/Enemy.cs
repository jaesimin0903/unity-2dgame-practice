using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    // Start is called before the first frame update

    public int playerDamage;
    public AudioClip enemyAttack1;
    public AudioClip enemyAttack2;

    private Animator animator;
    private Transform target; //�÷��̾� ��ġ�� ����, ���� ���� ������ �˷���
    private bool skipMove; //���� �����̰Բ� �ϴ� ����

    protected override void Start()
    {
        GameManager1.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;//�÷��̾� ��ġ�� �޾ƿ�
        base.Start();
    }

    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        if (skipMove)//�Ź� ���� ���ƿ� ���� ������
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }

    public void MoveEnemy()//������ �����̷� �Ҷ� ���� �Ŵ����� ���� ȣ��� �Լ�
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)//���� x ��ǥ�� ������ üũ, ���� �÷��̾ ���� ��
            yDir = target.position.y > transform.position.y ? 1 : -1; //�÷��̾��� y ��ǥ�� �� ũ�ٸ� ���� y++
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove <T> (T component)
    {
        Player hitPlayer = component as Player;

        animator.SetTrigger("enemyAttack");

        hitPlayer.LoseFood(playerDamage);//�÷��̾��� losefood  �� ȣ���ϰ� ���� �������� �Ҿ���� ���������� �� playerDamage �Է�

        SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);
    }
}
