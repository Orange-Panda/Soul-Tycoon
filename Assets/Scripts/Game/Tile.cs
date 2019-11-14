using TMPro;
using UnityEngine;

/// <summary>
/// Handles the purchasing of land and buildings.
/// </summary>
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
	public static Tile highlightedTile;

	private void Awake()
	{
		if (properties == null)
		{
			border.color = Color.white;
			fill.color = new Color(0.3f, 0f, 0f, 1f);
			textMesh.SetText("N/A");
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

	private void OnMouseEnter()
	{
		border.color = new Color(0.5f, 0.5f, 0.5f, 1f);
		highlightedTile = this;
	}

	private void OnMouseExit()
	{
		border.color = Color.white;
		if(highlightedTile = this)
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
		else if(productionBuilding = GetComponent<ProductionBuilding>())
		{
			productionBuilding.GatherCurrency();
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
		fill.color = new Color(0.2f, 0.3f, 0.2f, 1f);
		textMesh.SetText("");
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
		buildingSprite.sprite = building.sprite;
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
			buildingSprite.sprite = null;

			//Stop particle systems to prevent them from lingering.
			var emission = GetComponent<ParticleSystem>().emission;
			emission.rateOverTime = 0;
			buildingSprite.GetComponent<ParticleSystem>().Stop();
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
