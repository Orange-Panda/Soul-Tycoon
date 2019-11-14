using TMPro;
using UnityEngine;

/// <summary>
/// Responsible for juding if a tile is placeable on the map and controlling the game space appearance of the tile.
/// There should be only one present in the scene.
/// </summary>
public class TilePlaceable : MonoBehaviour
{
	//Private fields
	private bool obstructed;
	[System.Obsolete]
	private bool withinRegion;

	//Component references
	private DistrictRegion hoverRegion;
	private Collider2D[] results = new Collider2D[32];
	private SpriteRenderer[] spriteRenderers;
	private TextMeshPro textMesh;
	new private Camera camera;

	public static TilePlaceable instance;

	private bool Placeable => !obstructed && hoverRegion != null && HUDBuildCommand.selected;
	private uint Cost => HUDBuildCommand.selected && hoverRegion ? (uint)Mathf.RoundToInt((HUDBuildCommand.selected.building.cost + hoverRegion.districtProperties.tilePurchaseCost) * Mathf.Pow(hoverRegion.districtProperties.growthScale, hoverRegion.BuildingsPlaced)) : 0;
	private bool Affordable => HUDBuildCommand.selected && hoverRegion && Player.Currency >= Cost;

	private void Awake()
	{
		instance = this;
		camera = FindObjectOfType<Camera>();
		textMesh = GetComponentInChildren<TextMeshPro>();
		spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
	}

	private void Update()
	{
		transform.position = camera.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, .8f, 10f);

		//Initialize variables
		int arraySize;
		obstructed = false;
		hoverRegion = null;

		//Decide if obstructed by another building 
		arraySize = Physics2D.OverlapCircleNonAlloc(transform.position - new Vector3(0f, 0.4f), 0.9f, results);
		for (int i = 0; i < arraySize; i++)
		{
			if (results[i].CompareTag("Tile"))
			{
				obstructed = true;
				break;
			}
		}

		//Decide if within a district
		arraySize = Physics2D.OverlapCircleNonAlloc(transform.position - new Vector3(0f, 1.28f), 0.01f, results);
		for (int i = 0; i < arraySize; i++)
		{
			if (results[i].CompareTag("District"))
			{
				DistrictRegion region = results[i].GetComponent<DistrictRegion>();
				hoverRegion = region;
				break;
			}
		}

		//Set text
		textMesh.color = Placeable && Affordable ? Color.white : Color.red;
		if (hoverRegion != null)
		{
			textMesh.SetText(string.Format("{0}\n${1}\n{2}%", hoverRegion.districtProperties.tileName, Cost, Mathf.FloorToInt(hoverRegion.districtProperties.traffic * 100)));
		}
		else
		{
			textMesh.SetText("Move within a district to purchase");
		}

		//Visibility
		bool visibility = HUDBuildCommand.GetBuildableStateGlobal() == BuildableState.Dragging;
		textMesh.enabled = visibility;
		foreach (SpriteRenderer renderer in spriteRenderers)
		{
			renderer.enabled = visibility;
		}
	}

	/// <summary>
	/// Attempts to build the currently selected unit.
	/// </summary>
	internal void AttemptBuild()
	{
		if (Placeable && Player.Withdraw(Cost, camera.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, .8f, 10f), true))
		{
			GameObject newTile = Instantiate(Resources.Load<GameObject>("Tile"), transform.position, transform.rotation);
			Tile tile = newTile.GetComponent<Tile>();
			tile.properties = hoverRegion.districtProperties;
			tile.AcquireTile();
			tile.PlaceBuilding(HUDBuildCommand.selected.building);
			hoverRegion.BuildingsPlaced++;
		}

		HUDBuildCommand.selected.Return();
	}
}
