using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    public Sprite dmgSprite;  // �÷��̾ ���� �Ѵ� ������ �� ������ ��������Ʈ  
    public int hp = 4; // ���� ü��

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall (int loss)
    {
        spriteRenderer.sprite = dmgSprite; // �÷��̾ ���������� ���� ���������� �ð����� ��ȭ�� ��
        hp -= loss;
        if (hp <= 0)
            gameObject.SetActive(false); //hp 0 �̶�� ���ӿ�����Ʈ ��Ȱ��ȭ
    }
}
