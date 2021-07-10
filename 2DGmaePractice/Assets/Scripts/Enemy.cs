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
    private Transform target; //플레이어 위치를 저장, 적이 어디로 향할지 알려줌
    private bool skipMove; //적이 움직이게끔 하는 변수

    protected override void Start()
    {
        GameManager1.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;//플레이어 위치를 받아옴
        base.Start();
    }

    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        if (skipMove)//매번 턴이 돌아올 때만 움직임
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }

    public void MoveEnemy()//적들을 움직이려 할때 게임 매니저에 의해 호출될 함수
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)//대충 x 좌표가 같은지 체크, 적과 플레이어가 같은 열
            yDir = target.position.y > transform.position.y ? 1 : -1; //플레이어의 y 좌표가 더 크다면 적을 y++
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove <T> (T component)
    {
        Player hitPlayer = component as Player;

        animator.SetTrigger("enemyAttack");

        hitPlayer.LoseFood(playerDamage);//플레이어의 losefood  를 호출하고 적의 공격으로 잃어버릴 음식점수가 될 playerDamage 입력

        SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);
    }
}
