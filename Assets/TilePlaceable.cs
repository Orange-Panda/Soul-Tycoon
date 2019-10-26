using TMPro;
using UnityEngine;

/// <summary>
/// Handles user placement of tiles on the map.
/// </summary>
public class TilePlaceable : MonoBehaviour
{
	private bool obstructed;
	private bool withinRegion;
	private TileProperties regionProperties;
	private Collider2D[] results = new Collider2D[32];
	private TextMeshPro textMesh;
	new private Camera camera;

	private bool Placeable => !obstructed && withinRegion;

	private void Start()
	{
		camera = FindObjectOfType<Camera>();
		textMesh = GetComponentInChildren<TextMeshPro>();
		InvokeRepeating("CheckBuildState", 0f, 1 / 60f);
	}

	private void Update()
	{
		transform.position = camera.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, .8f, 10f);
	}

	private void CheckBuildState()
	{
		int arraySize;
		//Decide if obstructed by another building 
		obstructed = false;
		arraySize = Physics2D.OverlapCircleNonAlloc(transform.position, 1.28f, results);
		for (int i = 0; i < arraySize; i++)
		{
			if (results[i].CompareTag("Tile"))
			{
				obstructed = true;
				break;
			}
		}

		//Decide if within a district
		withinRegion = false;
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

		textMesh.SetText(string.Format("O: {0}\nR: {1}\nP: {2}", obstructed, withinRegion, Placeable));
	}
}
