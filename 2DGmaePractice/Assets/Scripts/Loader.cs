using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ���ӸŴ����� �ν��Ͻ�ȭ �Ǿ����� �ʴٸ� �ν��Ͻ�ȭ ���ִ� ��ũ��Ʈ
public class Loader : MonoBehaviour
{
    public GameObject gameManager;


    void Awake()
    {
        if (GameManager1.instance == null)
            Instantiate(gameManager);
    }


}
