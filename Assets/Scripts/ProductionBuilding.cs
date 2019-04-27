using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionBuilding : MonoBehaviour
{
	private Tile tile;
	public ProductionProperties thisBuilding;
	private float yieldProgress;
	public uint heldCurrency;
	public bool powered = true;
	public int durability = 100;

	public float DurabilityYieldModifier
	{
		get
		{
			return durability / thisBuilding.durability > thisBuilding.damagedThreshold ? 1 : thisBuilding.damagedMultiplier;
		}
	}

	private void Start()
	{
		tile = GetComponent<Tile>();
	}

	private void OnEnable()
	{
		GameManager.HourTick += GameManager_HourTick;
		GameManager.DayTick += GameManager_DayTick;
	}

	private void OnDisable()
	{
		GameManager.HourTick -= GameManager_HourTick;
		GameManager.DayTick -= GameManager_DayTick;
	}

	private void GameManager_HourTick()
	{
		yieldProgress += (thisBuilding.yieldBaseRate * tile.properties.traffic * DurabilityYieldModifier) / 24;

		while(yieldProgress >= 1)
		{
			YieldCurrency();
			yieldProgress--;
		}

		CheckForDisaster();
	}

	private void GameManager_DayTick()
	{

	}

	private void CheckForDisaster()
	{
		List<BuildingDistaster> disasters = thisBuilding.potentialDisasters;
		disasters.AddRange(tile.properties.environmentalDisaster);
		foreach(BuildingDistaster disaster in disasters)
		{
			if(SoulTycoon.AttemptRisk(disaster.risk))
			{
				durability -= (int)SoulTycoon.VariableValue(disaster.damageBase, disaster.damageVariance);
			}
		}
	}

	internal void AttemptRepair()
	{
		if(Player.Withdraw((uint)Mathf.CeilToInt(Mathf.Max(durability, 0) / thisBuilding.durability * thisBuilding.repairCost)))
		{
			durability = (int)thisBuilding.durability;
		}
	}

	private void YieldCurrency()
	{
		heldCurrency = (uint)Mathf.Min(thisBuilding.maxCurrency, SoulTycoon.VariableValue(thisBuilding.yieldBase, thisBuilding.yieldVariance));
	}

	public bool GatherCurrency()
	{
		if(heldCurrency > 0)
		{
			Player.Deposit(heldCurrency);
			heldCurrency = 0;
			return true;
		}
		else
		{
			return false;
		}
	}
}
