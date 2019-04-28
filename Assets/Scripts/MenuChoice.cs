using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuChoice : MonoBehaviour
{
	public Tile targetTile;
	public ProductionProperties[] buildings;

	public void Repair()
	{
		targetTile.GetComponent<ProductionBuilding>().AttemptRepair();
		Close();
	}

	public void Sell()
	{
		targetTile.Sell();
		Close();
	}

	public void Purchase(int index)
	{
		if(targetTile.PurchaseBuilding(buildings[index]))
		{
			Close();
		}
		else
		{
			//TODO: Insufficient funds.
		}
	}

	public void Close()
	{
		Destroy(gameObject);
	}
}
