using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MemoryTable : MonoBehaviour
{
	public static MemoryTable Instance { set; get; }
	
	public bool canPlay=true;
	
	//public int numPlayers=2;
	
	public int actualPlayer=0;
	
	public int pairsCount = 20;
	public int[] scores = new int[2];
	
	public Card[] deck = new Card[40];
	public GameObject[] cardsPrefab;
	
	public Card shownCard=null;
	public Card shownCard2=null;
	private float timeFlip;
	
	public Text score1;
	public Text score2;
	public Text pairsLeft;
	public CanvasGroup winAlert;
	
	private bool gameIsOver=false;
	private float winTime;
	
	private void Start()
	{
		Instance = this;
		
		GenerateDeck();
	}
	
	private void Update()
	{
		CardClicker();
		UpdateReflip();
		if(gameIsOver)
		{
			if(Time.time - winTime > 2.0f)
			{
				SceneManager.LoadScene("MemoryMenu");
			}
		}
	}
	
	private void UpdateReflip()
	{
		if(shownCard2)
		{
			if(Time.time - timeFlip > 1.5f)
			{
				ReFlip(shownCard2, shownCard);
				if(actualPlayer==0)
				{
					actualPlayer=1;
				}
				else
				{
					actualPlayer=0;
				}
				shownCard=null;
				shownCard2=null;
				canPlay=true;
			}
		}
	}
	
	private void CardClicker()
	{
		if(!Camera.main)
		{
			Debug.Log("Camera main non trovata");
			return;
		}
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit, 100.0f))
			{
				if(hit.transform!=null)
				{
					Card c;
					
					if(c=hit.transform.GetComponent<Card>())
					{
						if(canPlay){
							if(!c.isShown)
								FlipCard(c);
						}
					}
				}
			}
		}
	}
	
	private void FlipCard(Card c)
	{
		if(!c.isShown)
		{
			canPlay=false;
			c.transform.Rotate(Vector3.right * 180);
			c.isShown = true;
		}
		else
		{
			return;
		}
		if(shownCard)
		{
			if(c.IsAMatch(shownCard))
			{
				scores[actualPlayer]++;
				//Debug.Log("Il giocatore " + (actualPlayer+1) + " ha " + scores[actualPlayer] + " punti.");
				pairsCount--;
				//Debug.Log("Restano " + pairsCount + " coppie.");
				UpdateScore();
				shownCard = null;
				canPlay=true;
				if(pairsCount==0)
					EndGame();
			}
			else
			{
				shownCard2=c;
				timeFlip=Time.time;
				//Wait
				//ReFlip(c, shownCard);
				//if(actualPlayer==0)
				//{
				//	actualPlayer=1;
				//}
				//else
				//{
				//	actualPlayer=0;
				//}
			}
		}
		else
		{
			shownCard = c;
			canPlay=true;
		}
	}
	
	private void ReFlip(Card c1, Card c2)
	{
		c1.transform.Rotate(Vector3.right * 180);
		c2.transform.Rotate(Vector3.right * 180);
	}
	
	private void EndGame()
	{
		if(scores[0]>scores[1])
		{
			winAlert.GetComponentInChildren<Text>().text = "Vince il player 1!";
			//Debug.Log("Il giocatore 1 vince!");
		}
		else if(scores[0]<scores[1])
		{
			winAlert.GetComponentInChildren<Text>().text = "Vince il player 2!";
			//Debug.Log("Il giocatore 2 vince!");
		}
		else
		{
			winAlert.GetComponentInChildren<Text>().text = "Pareggio!";
		}
		winAlert.alpha = 1;
		gameIsOver=true;
		winTime=Time.time;
	}
	
	private void GenerateDeck()
	{
		for(int i=0;i<20;i++)
		{
			GenerateCard(i);
		}
		for(int i=20;i<40;i++)
		{
			GenerateCopy(i);
		}
		ShuffleDeck();
		PositionCards();
	}
	
	private void GenerateCard(int i)
	{
		GameObject go = Instantiate(cardsPrefab[i]) as GameObject;
		go.transform.SetParent(transform);
		Card c = go.GetComponent<Card>();
		c.cardValue = (i%10)+1;
		c.isHearts = (i%10==i)?false:true;
		deck[i] = c;
	}
	
	private void GenerateCopy(int i)
	{
		int i_up=i-20;
		GameObject go = Instantiate(cardsPrefab[i_up]) as GameObject;
		go.transform.SetParent(transform);
		Card c = go.GetComponent<Card>();
		c.cardValue = (i_up%10)+1;
		c.isHearts = (i_up%10==i_up)?false:true;
		deck[i] = c;
	}
	
	private void ShuffleDeck()
	{
		System.Random r = new System.Random();
		for(int i=0;i<40;i++)
		{
			int j = (i+r.Next())%40;
			ChangeCards(i,j);
		}
	}
	
	private void ChangeCards(int i, int j)
	{
		Card c = deck[i];
		deck[i] = deck[j];
		deck[j] = c;
	}
	
	private void PositionCards()
	{
		float x = 0.24f;
		float y = 0.2f;
		int i = 0;
		foreach(Card c in deck)
		{
			c.transform.position = (Vector3.right * x) + (Vector3.forward * y) + (Vector3.up * 0.42f);
			c.transform.Rotate(Vector3.right * 180);
			x -= 0.07f;
			i ++;
			if(i%8==0)
			{
				y-=0.1f;
				x=0.24f;
			}
		}
	}
	
	private void UpdateScore()
	{
		score1.text = scores[0].ToString();
		score2.text = scores[1].ToString();
		pairsLeft.text = "Coppie rimaste: " + pairsCount.ToString();
	}
}
