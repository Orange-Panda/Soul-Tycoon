using UnityEngine;

/// <summary>
/// Handles the input of a buy menu.
/// </summary>
public class MenuChoice : MonoBehaviour
{
	public Tile targetTile;
	public ProductionProperties[] buildings;

	/// <summary>
	/// Attempts to repair the building on the tile the menu originated from.
	/// </summary>
	public void Repair()
	{
		targetTile.GetComponent<ProductionBuilding>().AttemptRepair();
		Close();
	}

	/// <summary>
	/// Sells the building at the tile the menu originated from.
	/// </summary>
	public void Sell()
	{
		targetTile.Sell();
		Close();
	}

	/// <summary>
	/// Buys the building at the index provided on the tile the menu originated from.
	/// </summary>
	public void Purchase(int index)
	{
		if (targetTile.PurchaseBuilding(buildings[index]))
		{
			Close();
		}
	}

	/// <summary>
	/// Closes the menu.
	/// </summary>
	public void Close()
	{
		Destroy(gameObject);
	}
}
