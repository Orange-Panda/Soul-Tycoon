using System.Collections;
using UnityEngine;

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
		if (enableInput)
		{
			Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			transform.Translate(input * Time.unscaledDeltaTime * mainCamera.orthographicSize * 2f, Space.Self);
		}

		mainCamera.orthographicSize = Mathf.Max(2, Mathf.Min(mainCamera.orthographicSize + Input.mouseScrollDelta.y * -1 / 3, 15));

		if (Input.GetKeyDown(KeyCode.Escape) || Mathf.Abs(transform.position.x) > 22 || Mathf.Abs(transform.position.y) > 14)
		{
			StopAllCoroutines();
			StartCoroutine(TravelToPosition(new Vector3(0, 0, -10f)));
		}
		if (Input.GetKeyDown(KeyCode.Mouse2))
		{
			StopAllCoroutines();
			StartCoroutine(TravelToPosition(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
		}
	}

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
