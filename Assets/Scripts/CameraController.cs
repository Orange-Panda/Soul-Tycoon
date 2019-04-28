using System.Collections;
using UnityEngine;

/// <summary>
/// Allows the player to control the camera.
/// </summary>
public class CameraController : MonoBehaviour
{
	bool enableInput = true;
	private Camera mainCamera;

	private void Start()
	{
		mainCamera = GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update()
	{
		//Camera x and y movement
		if (enableInput)
		{
			Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			transform.Translate(input * Time.unscaledDeltaTime * mainCamera.orthographicSize * 2f, Space.Self);
		}

		//Zooming
		if (Tile.inputEnabled)
		{
			mainCamera.orthographicSize = Mathf.Max(2, Mathf.Min(mainCamera.orthographicSize + Input.mouseScrollDelta.y * -1 / 3, 15));
		}

		//Return to center
		if (Input.GetKeyDown(KeyCode.Escape) || Mathf.Abs(transform.position.x) > 28 || Mathf.Abs(transform.position.y) > 18)
		{
			StopAllCoroutines();
			StartCoroutine(TravelToPosition(new Vector3(0, 0, -10f)));
		}

		//Move to cursor
		if (Input.GetKeyDown(KeyCode.Mouse2))
		{
			StopAllCoroutines();
			StartCoroutine(TravelToPosition(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
		}
	}

	/// <summary>
	/// Lerps the current position to the destination and disables x,y movement.
	/// </summary>
	/// <param name="position">Target location</param>
	IEnumerator TravelToPosition(Vector3 position)
	{
		enableInput = false;
		for (int i = 0; i < 20; i++)
		{
			transform.position = Vector3.Lerp(transform.position, position, 0.15f);
			yield return null;
		}
		enableInput = true;
	}
}
