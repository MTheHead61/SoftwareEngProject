using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
	public bool isShown=false;
	public int cardValue;
	public bool isHearts;
	
	public bool IsAMatch(Card c)
	{
		if(!(this.isHearts ^ c.isHearts)&&(this.cardValue == c.cardValue))
		{
			return true;
		}
		else
		{
			this.isShown=false;
			c.isShown=false;
			return false;
		}
	}
}
