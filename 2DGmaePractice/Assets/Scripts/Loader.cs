using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 게임매니저가 인스턴스화 되어있지 않다면 인스턴스화 해주는 스크립트
public class Loader : MonoBehaviour
{
	public GameObject gameManager;          //GameManager prefab to instantiate.
	public GameObject soundManager;         //SoundManager prefab to instantiate.


	void Awake()
	{
		//Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
		if (GameManager1.instance == null)

			//Instantiate gameManager prefab
			Instantiate(gameManager);

		//Check if a SoundManager has already been assigned to static variable GameManager.instance or if it's still null
		if (SoundManager.instance == null)

			//Instantiate SoundManager prefab
			Instantiate(soundManager);
	}
}