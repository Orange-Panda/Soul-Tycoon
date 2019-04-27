using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract ScriptableObject which contains the basic data that every building has.
/// </summary>
public abstract class BuildingProperties : ScriptableObject
{
	[Header("Building Metadata")]
	public string title = "Building";
	public string description = "Does some cool stuff";
	public Sprite icon;
	public bool starter = false;

	[Header("Building Properties")]
	[Tooltip("The amount it costs to initially place the building.")]
	public uint cost = 10;

	[Tooltip("The amount the building is worth if sold.")]
	public uint value = 5;

	[Tooltip("The amount of strength this building has until it is put out of repaired.")]
	public uint durability = 100;

	[Tooltip("How much currency is required to repair this building to max durability.")]
	public uint repairCost = 5;

	[Tooltip("The threshold for production to be modified when damaged.")]
	[Range(0f, 1f)]
	public float damagedThreshold = 0.2f;

	[Tooltip("The yield frequency modifier when damaged.")]
	[Range(0f, 2f)]
	public float damagedMultiplier = 0.5f;

	[Tooltip("The potential disasters that can occur to this building.")]
	public List<BuildingDistaster> potentialDisasters;
}