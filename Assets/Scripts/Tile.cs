using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public TileProperties properties;
	private bool purchased = false;

	private void OnMouseDown()
	{
		if(!purchased)
		{
			if(Player.Withdraw(properties.tilePurchaseCost))
			{
				purchased = true;
				Debug.Log("Tile has been purchased");
			}
			else
			{
				Debug.Log("Insufficient funds");
			}
		}
	}
}
