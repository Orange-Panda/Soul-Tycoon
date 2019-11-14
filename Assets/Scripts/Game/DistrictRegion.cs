using UnityEngine;

/// <summary>
/// Contains data for a region of the map.
/// </summary>
public class DistrictRegion : MonoBehaviour
{
	public TileProperties districtProperties;
	public int BuildingsPlaced { get; set; } = 0;
}
