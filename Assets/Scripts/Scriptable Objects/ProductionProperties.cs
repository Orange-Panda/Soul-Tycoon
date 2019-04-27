using UnityEngine;

/// <summary>
/// Data for buildings that produce currency.
/// </summary>
[CreateAssetMenu(menuName = "Soul Tycoon/Production")]
public class ProductionProperties : BuildingProperties
{
	[Header("Production")]
	[Tooltip("The minimum amount of currency available in a yield")]
	public uint yieldBase = 4;

	[Tooltip("How much currency should be randomly added to the base?")]
	public uint yieldVariance = 2;

	[Tooltip("How many times an ingame day should the yield occur?")]
	public float yieldBaseRate = 5;

	[Tooltip("What is the maximum amount of currency this building can carry until it will halt production?")]
	public uint maxCurrency = 100;
}