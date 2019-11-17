using TMPro;
using UnityEngine;

/// <summary>
/// Handles the interaction and management of any tile.
/// </summary>
public class Tile : MonoBehaviour
{
	//Public fields
	public TileProperties properties;
	public BuildingProperties building;
	public SpriteRenderer tile, buildPrimary, buildSecondary, buildExtra, buildBackground;
	public ParticleSystem goldParticles, smokeParticles;

	//Static fields
	public static Tile highlightedTile;
	public static bool inputEnabled = true;

	[System.Obsolete]
	private bool purchased = false;

	private void OnEnable()
	{
		GameManager.DayTick += GameManager_DayTick;
	}

	private void OnDisable()
	{
		GameManager.DayTick -= GameManager_DayTick;
	}

	private void OnMouseEnter()
	{
		tile.color = new Color(0.5f, 0.5f, 0.5f, 1f);
		highlightedTile = this;
	}

	private void OnMouseExit()
	{
		tile.color = Color.white;
		if (highlightedTile == this)
		{
			highlightedTile = null;
		}
	}

	private void OnMouseDown()
	{
		if (!inputEnabled) return;

		ProductionBuilding productionBuilding;

		//Handle purchasing if the tile is not already owned
		if (!purchased)
		{
			//If the player has enough money to purchase this tile, purchase it.
			if (Player.Withdraw(properties.tilePurchaseCost, transform.position))
			{
				AcquireTile();
			}
		}
		else if (productionBuilding = GetComponent<ProductionBuilding>())
		{
			if (productionBuilding.heldCurrency > 0)
			{
				productionBuilding.GatherCurrency();
			}
			else if (productionBuilding.Repairable)
			{
				productionBuilding.AttemptRepair();
			}

		}
		//If they own they tile and don't have a building, open up the buy menu.
		else
		{
			CreateMenu(properties.tileType.ToString());
		}
	}

	/// <summary>
	/// Gives the player ownership of the tile explictly.
	/// </summary>
	public void AcquireTile()
	{
		purchased = true;
	}

	public void AttemptModification()
	{
		if (!inputEnabled || !purchased) return;

		ProductionBuilding productionBuilding;

		if (productionBuilding = GetComponent<ProductionBuilding>())
		{
			productionBuilding.GatherCurrency();
			CreateMenu("Modify");
		}
		else
		{
			CreateMenu(properties.tileType.ToString());
		}
	}

	private void GameManager_DayTick()
	{
		ProductionBuilding building;

		//If the tile is owned and has a building, have the player pay rent. If they can't afford rent disable the building for today.
		if (purchased && (building = GetComponent<ProductionBuilding>()))
		{
			building.powered = Player.Withdraw(properties.tileRentCost, transform.position, false);
		}
	}

	/// <summary>
	/// Attempts to purchase a building. If the conditions are met, will place building.
	/// </summary>
	internal bool PurchaseBuilding(BuildingProperties properties)
	{
		if (building == null && Player.Withdraw(properties.cost, transform.position))
		{
			PlaceBuilding(properties);
			return true;
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	/// Explictly places a building on this tile.
	/// </summary>
	public void PlaceBuilding(BuildingProperties properties)
	{
		building = properties;
		buildPrimary.sprite = properties.spritePrimary;
		buildSecondary.sprite = properties.spriteSecondary;
		buildExtra.sprite = properties.spriteExtra;
		buildBackground.sprite = properties.spriteBackground;
		if (properties.GetType() == typeof(ProductionProperties))
		{
			ProductionBuilding production = gameObject.AddComponent<ProductionBuilding>();
			production.InitializeBuilding((ProductionProperties)properties);
		}
	}

	/// <summary>
	/// Sells the building on the tile.
	/// </summary>
	internal void Sell()
	{
		if (building != null)
		{
			Player.Deposit(building.value, transform.position);
			Destroy(GetComponent<ProductionBuilding>());
			building = null;

			//Stop particle systems to prevent them from lingering.
			var emission = goldParticles.emission;
			emission.rateOverTime = 0;
			smokeParticles.Stop();
		}
	}

	/// <summary>
	/// Creates a menu with the name of the argument string.
	/// </summary>
	private void CreateMenu(string value)
	{
		GameObject buyPanel = Instantiate(Resources.Load<GameObject>(value), FindObjectOfType<Canvas>().transform);
		buyPanel.GetComponent<MenuChoice>().targetTile = this;
	}
}
