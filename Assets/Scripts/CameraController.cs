using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private float sensitivity = 8;
	bool enableInput = true;

	// Update is called once per frame
    void Update()
    {
		if(enableInput)
		{
			Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			transform.Translate(input * Time.unscaledDeltaTime * sensitivity, Space.Self);
		}


		if(Input.GetKeyDown(KeyCode.Escape) || Mathf.Abs(transform.position.x) > 22 || Mathf.Abs(transform.position.y) > 14)
		{
			StopAllCoroutines();
			StartCoroutine(ReturnToCenter());
		}
	}

	IEnumerator ReturnToCenter()
	{
		enableInput = false;
		for (int i = 0; i < 20; i++)
		{
			transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0, -10), 0.15f);
			yield return null;
		}
		enableInput = true;
	}
}
