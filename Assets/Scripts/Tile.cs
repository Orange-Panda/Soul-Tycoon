using System;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
	[Space]
	public TileProperties properties;
	private bool purchased = false;
	[Header("References")]
	public TextMeshPro textMesh;
	public SpriteRenderer fill, border, buildingSprite;
	public static bool inputEnabled = true;
	public BuildingProperties building;

	internal bool PurchaseBuilding(ProductionProperties properties)
	{
		if(building == null && Player.Withdraw(properties.cost, transform.position))
		{
			building = properties;
			buildingSprite.sprite = building.sprite;
			ProductionBuilding production = gameObject.AddComponent<ProductionBuilding>();
			production.InitializeBuilding(properties);
			return true;
		}
		else
		{
			return false;
		}
	}

	internal void Sell()
	{
		buildingSprite.sprite = null;
		Player.Deposit(building.value, transform.position);
		building = null;
		Destroy(GetComponent<ProductionBuilding>());
	}

	private void OnEnable()
	{
		if (properties == null)
		{
			border.color = new Color(0.3f, 0.3f, 0.3f, 1f);
			fill.color = new Color(0.3f, 0f, 0f, 1f);
			textMesh.SetText("");
			enabled = false;
		}
		else
		{
			border.color = Color.white;
			fill.color = Color.black;
			textMesh.SetText(properties.tilePurchaseCost.ToString());
		}

		GameManager.DayTick += GameManager_DayTick;
	}

	private void OnDisable()
	{
		GameManager.DayTick -= GameManager_DayTick;
	}

	private void GameManager_DayTick()
	{
		if (purchased && GetComponent<ProductionBuilding>())
		{
			GetComponent<ProductionBuilding>().powered = Player.Withdraw(properties.tileRentCost, transform.position, false);
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
		if (!inputEnabled) return;

		ProductionBuilding productionBuilding;

		if (!purchased)
		{
			if (Player.Withdraw(properties.tilePurchaseCost, transform.position))
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
		else if (productionBuilding = GetComponent<ProductionBuilding>())
		{
			if(!productionBuilding.GatherCurrency())
			{
				CreateMenu("Modify");
			}
		}
		else
		{
			CreateMenu(properties.tileType.ToString());
		}
	}


	private void CreateMenu(string value)
	{
		GameObject buyPanel = Instantiate(Resources.Load<GameObject>(value), FindObjectOfType<Canvas>().transform);
		buyPanel.GetComponent<MenuChoice>().targetTile = this;
	}
}
