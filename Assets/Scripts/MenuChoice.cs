using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuChoice : MonoBehaviour
{
	public Tile targetTile;
	public BuildingProperties[] buildings;

	public void Repair()
	{
		targetTile.GetComponent<ProductionBuilding>().AttemptRepair();
		Destroy(gameObject);
	}

	public void Sell()
	{
		Destroy(gameObject);
	}

	public void Purchase(int index)
	{
		Destroy(gameObject);
	}
}
