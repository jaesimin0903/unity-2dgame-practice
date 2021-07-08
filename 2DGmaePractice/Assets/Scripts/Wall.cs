using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    public Sprite dmgSprite;  // 플레이어가 벽을 한대 때렸을 때 보여줄 스프라이트  
    public int hp = 4; // 벽의 체력

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall (int loss)
    {
        spriteRenderer.sprite = dmgSprite; // 플레이어가 성공적으로 벽을 공격했을떄 시각적인 변화를 줌
        hp -= loss;
        if (hp <= 0)
            gameObject.SetActive(false); //hp 0 이라면 게임오브젝트 비활성화
    }
}
