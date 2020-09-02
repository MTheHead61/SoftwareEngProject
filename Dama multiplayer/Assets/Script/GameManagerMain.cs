using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManagerMain : MonoBehaviour
{
	public static GameManagerMain Instance { set; get; }
	
	private void Start()
	{
		Instance = this;
	}
	
	public void MemoryButton()
	{
		SceneManager.LoadScene("MemoryMenu");
	}
	
	public void CheckersButton()
	{
		SceneManager.LoadScene("Menu");
	}
}
