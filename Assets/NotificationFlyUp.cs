using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationFlyUp : MonoBehaviour
{
	public float duration = 1.5f;
	private TextMeshPro textMesh;

	private void Start()
	{
		textMesh = GetComponent<TextMeshPro>();
		StartCoroutine(FlyUp());
	}

	IEnumerator FlyUp()
	{
		float timeRemaining = duration;
		while (timeRemaining > 0)
		{
			timeRemaining -= 0.02f;
			textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, timeRemaining / duration);
			transform.Translate(0, 0.045f * (timeRemaining / duration), 0, Space.Self);
			yield return new WaitForSecondsRealtime(0.02f);
		}
		Destroy(gameObject);
	}
}
