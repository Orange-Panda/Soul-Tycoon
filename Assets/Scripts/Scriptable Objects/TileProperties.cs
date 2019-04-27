using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data for a tile on the map.
/// </summary>
[CreateAssetMenu(menuName = "Soul Tycoon/Tile")]
public class TileProperties : ScriptableObject
{
	[Tooltip("Flavor name of the tile.")]
	public string tileName = "Tile on 5th St.";

	[Tooltip("Dictates the type of buildings that can be built on this tile.")]
	public TileType tileType;

	[Tooltip("Cost to build on this tile.")]
	public uint tilePurchaseCost = 5;

	[Tooltip("Cost per turn to continue using the tile.")]
	public uint tileRentCost = 1;

	[Tooltip("Modifier to the frequency of yields.")]
	[Range(0f, 4f)]
	public float traffic = 1;

	[Tooltip("The potential disaster that this tile presents.")]
	public List<BuildingDistaster> environmentalDisaster;
}

public enum TileType
{
	/// <summary>
	/// Only the initial building can be built here.
	/// </summary>
	Starter,
	/// <summary>
	/// Only production buildings can be built here.
	/// </summary>
	Production,
	/// <summary>
	/// Only utility buildings can be built here.
	/// </summary>
	Utility,
	/// <summary>
	/// Utility and production buildings can be built here.
	/// </summary>
	Variety
}