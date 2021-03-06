﻿using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the logic for generating and collecting currency.
/// </summary>
public class ProductionBuilding : MonoBehaviour
{
	private Tile tile;
	public ProductionProperties thisBuilding;
	private float yieldProgress;
	public uint heldCurrency;
	public bool powered = true;
	public int durability = 100;

	private ParticleSystem GoldParticles => tile.goldParticles;
	private ParticleSystem SmokeParticles => tile.smokeParticles;
	public float YieldValue => thisBuilding.yieldBaseRate * tile.properties.traffic * DurabilityYieldModifier;
	public uint RepairCost => (uint)Mathf.CeilToInt(Mathf.Abs((float)durability / thisBuilding.durability - 1) * thisBuilding.repairCost);
	public float DurabilityYieldModifier => durability > 0 ? (float)durability / thisBuilding.durability > thisBuilding.damagedThreshold ? 1 : thisBuilding.damagedMultiplier : 0;
	public bool Repairable => durability < thisBuilding.durability;

	private void Start()
	{
		tile = GetComponent<Tile>();
		GoldParticles.Play();
		SmokeParticles.Emit(5);
		SmokeParticles.Stop();
	}

	/// <summary>
	/// Sets the building properties values.
	/// </summary>
	public void InitializeBuilding(ProductionProperties properties)
	{
		thisBuilding = properties;
		durability = (int)properties.durability;
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

	/// <summary>
	/// Make Progress every hour of the game
	/// </summary>
	private void GameManager_HourTick()
	{
		yieldProgress += powered ? YieldValue / 24 : 0;

		while (yieldProgress >= 1)
		{
			YieldCurrency();
			yieldProgress--;
		}

		CheckForDisaster();
	}

	/// <summary>
	/// Events the would happen at the start of a day.
	/// </summary>
	private void GameManager_DayTick()
	{

	}

	/// <summary>
	/// Tries to damage the building if unlucky.
	/// </summary>
	private void CheckForDisaster()
	{
		//Creates a list of potential disasters
		List<BuildingDistaster> disasters = new List<BuildingDistaster>();
		disasters.AddRange(thisBuilding.potentialDisasters);
		disasters.AddRange(tile.properties.environmentalDisaster);

		foreach (BuildingDistaster disaster in disasters)
		{
			// If the building isn't already broken and the risk returns true a disaster occurs.
			if (durability > 0 && SoulTycoon.AttemptRisk(disaster.risk))
			{
				//Gameplay implications
				int damageDealt = (int)SoulTycoon.VariableValue(disaster.damageBase, disaster.damageVariance);
				durability = Mathf.Max(durability - damageDealt, 0);

				//Play particle effects
				SmokeParticles.Emit(15);
				if (durability <= 0)
				{
					SmokeParticles.Play();
				}

				//Create a notifiaction about the disaster.
				GameObject go = Instantiate(Resources.Load<GameObject>("Disaster"), transform.position - new Vector3(0, 0, 1), Quaternion.identity);
				TMPro.TextMeshPro textMesh = go.GetComponent<TMPro.TextMeshPro>();
				textMesh.SetText(string.Format(durability > 0 ? "{0}!\nIntegrity {2}%" : "{0}\nRepairs needed!", disaster.name, damageDealt, Mathf.Max((float)durability / thisBuilding.durability * 100, 0).ToString("N1")));
				textMesh.color = new Color(1f, 0.7f, 0.7f);

				//Only one disaster can occur per hour so we stop running through the foreach loop here.
				break;
			}
		}
	}

	/// <summary>
	/// If the player can afford it will fully repair the building.
	/// </summary>
	internal void AttemptRepair()
	{
		if (Player.Withdraw(RepairCost, transform.position))
		{
			durability = (int)thisBuilding.durability;
			SmokeParticles.Stop();
		}
	}

	/// <summary>
	/// Will add currency to the building's storage. Cannot store past the buildings limit.
	/// </summary>
	private void YieldCurrency()
	{
		heldCurrency = (uint)Mathf.Min(thisBuilding.maxCurrency, heldCurrency + SoulTycoon.VariableValue(thisBuilding.yieldBase, thisBuilding.yieldVariance));
		var emission = GoldParticles.emission;
		emission.rateOverTime = new ParticleSystem.MinMaxCurve(Mathf.Log(heldCurrency, 1.3f) + 3);
	}

	/// <summary>
	/// Takes the currency being held by the building and puts it into the player's balance.
	/// </summary>
	public bool GatherCurrency(bool playSound = true)
	{
		if (heldCurrency > 0)
		{
			Player.Deposit(heldCurrency, transform.position, playSound);
			heldCurrency = 0;
			var emission = GoldParticles.emission;
			emission.rateOverTime = new ParticleSystem.MinMaxCurve(0);
			GoldParticles.Emit(30);
			return true;
		}
		else
		{
			return false;
		}
	}
}
