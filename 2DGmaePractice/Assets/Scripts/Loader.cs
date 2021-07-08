using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 게임매니저가 인스턴스화 되어있지 않다면 인스턴스화 해주는 스크립트
public class Loader : MonoBehaviour
{
    public GameObject gameManager;


    void Awake()
    {
        if (GameManager1.instance == null)
            Instantiate(gameManager);
    }


}
