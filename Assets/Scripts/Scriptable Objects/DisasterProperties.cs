using System;
using UnityEngine;

/// <summary>
/// Metadata for a disaster. Does not have any gameplay implications by itself.
/// </summary>
[CreateAssetMenu(menuName = "Soul Tycoon/Disaster")]
public class DisasterProperties : ScriptableObject
{
	[Tooltip("Image shown in the UI when this disaster occurs")]
	public Sprite disasterSprite;

	[Tooltip("Label of the disaster")]
	public string disasterTitle = "Meltdown";

	[Tooltip("Description of the disaster. Arg 0 is building, arg 1 is the tile.")]
	public string disasterDescription = "The building";
}

/// <summary>
/// Struct that contains the metadata of a disaster and the gameplay properties of that disaster.
/// </summary>
[Serializable]
public struct BuildingDistaster
{
	/// <summary>
	/// Metadata for the disaster.
	/// </summary>
	public DisasterProperties properties;
	/// <summary>
	/// Probability of the disaster occuring.
	/// </summary>
	public Risk risk;
	/// <summary>
	/// Base amount of damage dealt.
	/// </summary>
	public uint damageBase;
	/// <summary>
	/// Potential damage added to the base.
	/// </summary>
	public uint damageVariance;
}

/// <summary>
/// The probability that an unfavorable outcome will occur.
/// </summary>
public enum Risk
{
	/// <summary>
	/// Bad outcomes will never occur.
	/// </summary>
	Protected,
	/// <summary>
	/// Bad outcomes will rarely occur.
	/// </summary>
	Low,
	/// <summary>
	/// Bad outcomes will occasionally occur.
	/// </summary>
	Medium,
	/// <summary>
	/// Bad outcomes will frequently occur.
	/// </summary>
	High,
	/// <summary>
	/// Bad outcomes will ALWAYS occur.
	/// </summary>
	Definite
}