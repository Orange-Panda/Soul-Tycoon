using UnityEngine;

/// <summary>
/// Data for buildings that maintain or boost production buildings.
/// </summary>
[CreateAssetMenu(menuName = "Soul Tycoon/Utility")]
public class UtilityProperties : BuildingProperties
{
	[Header("Utility")]
	[Tooltip("How far reaching is the effect of this utility?")]
	public float range = 5f;

	[Tooltip("How powerful is the effect of this utility?")]
	public float strength = 1f;

	[Tooltip("How much currency should be spent every day to maintain this utility?")]
	public uint dailyCost = 1;
}