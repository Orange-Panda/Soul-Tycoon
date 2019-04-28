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
	private ParticleSystem productionParticles, smokeParticles;
	private Color standard = new Color(1, 0.9f, 0.5f);
	private Color broken = new Color(0.25f, 0.22f, 0.125f);

	public uint RepairCost
	{
		get
		{
			return (uint)Mathf.CeilToInt(Mathf.Abs((float)durability / thisBuilding.durability - 1) * thisBuilding.repairCost);
		}
	}

	public float DurabilityYieldModifier
	{
		get
		{
			if (durability <= 0) return 0;
			return (float)durability / thisBuilding.durability > thisBuilding.damagedThreshold ? 1 : thisBuilding.damagedMultiplier;
		}
	}

	private void Start()
	{
		tile = GetComponent<Tile>();
		productionParticles = GetComponent<ParticleSystem>();
		smokeParticles = tile.buildingSprite.GetComponent<ParticleSystem>();
		productionParticles.Play();
		smokeParticles.Emit(5);
		smokeParticles.Stop();
	}

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

	private void GameManager_HourTick()
	{
		yieldProgress += powered ? (thisBuilding.yieldBaseRate * tile.properties.traffic * DurabilityYieldModifier) / 24 : 0;

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
		List<BuildingDistaster> disasters = new List<BuildingDistaster>();
		disasters.AddRange(thisBuilding.potentialDisasters);
		disasters.AddRange(tile.properties.environmentalDisaster);
		foreach(BuildingDistaster disaster in disasters)
		{
			if(durability > 0 && SoulTycoon.AttemptRisk(disaster.risk))
			{
				smokeParticles.Emit(15);
				int damageDealt = (int)SoulTycoon.VariableValue(disaster.damageBase, disaster.damageVariance);
				durability = Mathf.Max(durability - damageDealt, 0);
				if (durability <= 0)
				{
					smokeParticles.Play();
				}
				GameObject go = Instantiate(Resources.Load<GameObject>("Disaster"), transform.position - new Vector3(0, 0, 1), Quaternion.identity);
				TMPro.TextMeshPro textMesh = go.GetComponent<TMPro.TextMeshPro>();
				textMesh.SetText(string.Format("{0}!\nIntegrity {2}%", disaster.name, damageDealt, Mathf.Max((float)durability / thisBuilding.durability * 100, 0).ToString("N1")));
				textMesh.color = new Color(1f, 0.7f, 0.7f);
				break;
			}
		}
	}

	internal void AttemptRepair()
	{
		if(Player.Withdraw(RepairCost, transform.position))
		{
			durability = (int)thisBuilding.durability;
			var main = productionParticles.main;
			main.startColor	= standard;
			smokeParticles.Stop();
		}
	}

	private void YieldCurrency()
	{
		heldCurrency = (uint)Mathf.Min(thisBuilding.maxCurrency, heldCurrency + SoulTycoon.VariableValue(thisBuilding.yieldBase, thisBuilding.yieldVariance));
		var emission = productionParticles.emission;
		emission.rateOverTime = new ParticleSystem.MinMaxCurve(Mathf.Log(heldCurrency, 1.3f) + 3);
	}

	public bool GatherCurrency()
	{
		if(heldCurrency > 0)
		{
			Player.Deposit(heldCurrency, transform.position);
			heldCurrency = 0;
			var emission = productionParticles.emission;
			emission.rateOverTime = new ParticleSystem.MinMaxCurve(0);
			productionParticles.Emit(30);
			return true;
		}
		else
		{
			return false;
		}
	}
}
