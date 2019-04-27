using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
	[Space]
	public TileProperties properties;
	private bool purchased = false;
	[Header("References")]
	public TextMeshPro textMesh;
	public SpriteRenderer fill, border, buiilding;

	private void Start()
	{
		if(properties == null)
		{
			border.color = new Color(0.3f, 0.3f, 0.3f, 1f);
			fill.color = new Color(0.3f, 0f, 0f, 1f);
			textMesh.SetText("");
			Destroy(this);
		}
		else
		{
			border.color = Color.white;
			fill.color = Color.black;
			textMesh.SetText(properties.tilePurchaseCost.ToString());
		}
	}

	private void OnEnable()
	{
		GameManager.DayTick += GameManager_DayTick;
	}

	private void OnDisable()
	{
		GameManager.DayTick -= GameManager_DayTick;
	}

	private void GameManager_DayTick()
	{
		if(purchased)
		{
			Player.ForceWithdraw(properties.tileRentCost);
		}
	}

	private void OnMouseEnter()
	{
		border.color = new Color(0.5f, 0.5f, 0.5f, 1f);
	}

	private void OnMouseExit()
	{
		border.color = Color.white;
	}

	private void OnMouseDown()
	{
		if(!purchased)
		{
			if(Player.Withdraw(properties.tilePurchaseCost))
			{
				purchased = true;
				fill.color = new Color(0.2f, 0.3f, 0.2f, 1f);
				textMesh.SetText("");
				Debug.Log("Tile has been purchased");
			}
			else
			{
				Debug.Log("Insufficient funds");
			}
		}
	}
}
