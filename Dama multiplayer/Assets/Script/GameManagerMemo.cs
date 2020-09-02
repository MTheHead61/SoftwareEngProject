using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManagerMemo : MonoBehaviour
{
	public static GameManagerMemo Instance { set; get; }
	
	private void Start()
	{
		Instance = this;
	}
	
	public void GameButton()
	{
		SceneManager.LoadScene("Memory");
	}
	
	public void BackButton()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
