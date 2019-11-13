using System.Collections;
using UnityEngine;

/// <summary>
/// Allows the player to control the camera.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	private bool inputEnabled = true;
	private Camera mainCamera;

	private void Start()
	{
		mainCamera = GetComponent<Camera>();
	}

	void Update()
	{
		//Camera input movement and zooming
		if (inputEnabled)
		{
			Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			transform.Translate(input * Time.unscaledDeltaTime * mainCamera.orthographicSize * 2f, Space.Self);
			mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize + (Input.mouseScrollDelta.y * -Time.unscaledDeltaTime * 30), 3, 15);
		}

		//Return to center
		if (Input.GetKeyDown(KeyCode.Escape) || Mathf.Abs(transform.position.x) > 28 || Mathf.Abs(transform.position.y) > 18)
		{
			StopAllCoroutines();
			StartCoroutine(TravelToPosition(new Vector3(0, 0, -10f)));
		}

		//Move to cursor
		if (Input.GetKeyDown(KeyCode.Mouse2) || Input.GetKeyDown(KeyCode.Space))
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
		inputEnabled = false;
		for (int i = 0; i < 15; i++)
		{
			transform.position = Vector3.Lerp(transform.position, position, 0.15f);
			yield return null;
		}
		inputEnabled = true;
	}
}
