using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Responsible for juding if a tile is placeable on the map and controlling the game space appearance of the tile.
/// </summary>
public class TilePlaceable : MonoBehaviour
{
	private bool obstructed;
	private bool withinRegion;
	private TileProperties regionProperties;
	private Collider2D[] results = new Collider2D[32];
	private SpriteRenderer[] spriteRenderers;
	private TextMeshPro textMesh;
	new private Camera camera;

	private bool Placeable => !obstructed && withinRegion;

	private void Awake()
	{
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
		withinRegion = false;

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
				withinRegion = true;
				DistrictRegion region = results[i].GetComponent<DistrictRegion>();
				regionProperties = region.districtProperties;
				break;
			}
		}

		//Set text
		textMesh.color = Placeable ? Color.white : Color.red;
		if (withinRegion && regionProperties != null)
		{
			textMesh.SetText(string.Format("{0}\n${1}\n{2}%", regionProperties.tileName, regionProperties.tilePurchaseCost, Mathf.FloorToInt(regionProperties.traffic * 100)));
		}
		else
		{
			textMesh.SetText("Move within a district to purchase");
		}

		//Visibility
		bool visible = HUDBuildCommand.instance.GetBuildableState() == BuildableState.Dragging ? true : false;
		textMesh.enabled = visible;
		foreach (SpriteRenderer renderer in spriteRenderers)
		{
			renderer.enabled = visible;
		}
	}

	internal static void AttemptBuild()
	{
		throw new NotImplementedException();
	}
}
