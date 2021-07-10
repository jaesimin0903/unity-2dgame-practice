using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ���ӸŴ����� �ν��Ͻ�ȭ �Ǿ����� �ʴٸ� �ν��Ͻ�ȭ ���ִ� ��ũ��Ʈ
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