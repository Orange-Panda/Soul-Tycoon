using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data for tiles placed in a region of the map.
/// </summary>
[CreateAssetMenu(menuName = "Soul Tycoon/Tile")]
public class TileProperties : ScriptableObject
{
	[Tooltip("Name of the region this tile occupies")]
	public string tileName = "Tile on 5th St.";

	[Tooltip("Description of the region this tile occupies")]
	[TextArea]
	public string tileDescription = "It is a place.";

	[Space]
	[Tooltip("Flat amount added to the purchase of a building in this region.")]
	public uint tilePurchaseCost = 5;

	[Tooltip("The rate at which the cost is amplified based off number of buildings in the region. The equation for this is: (Building Cost + Tile Cost) * Growth Scale^NumBuilt")]
	[Range(1f, 1.5f)]
	public float growthScale = 1;

	[Tooltip("Scalar for how often production buildings will yield.")]
	[Range(0f, 5f)]
	public float traffic = 1;

	[Space]
	[Tooltip("The potential disasters that this tile presents.")]
	public List<BuildingDistaster> environmentalDisaster;

	[Header("Obsolete")]
	[Tooltip("OBSOLETE: Dictates the type of buildings that can be built on this tile.")]
	[System.Obsolete]
	public TileType tileType;

	[Tooltip("OBSOLETE: Cost per turn to continue using the tile.")]
	[System.Obsolete]
	public uint tileRentCost = 1;
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