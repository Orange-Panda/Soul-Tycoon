using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Causes the attached gameObject to fly up and then be destroyed over time.
/// </summary>
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

		//Every 0.02 seconds, move the object up a little bit and set the alpha to fade away.
		while (timeRemaining > 0)
		{
			textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, timeRemaining / duration);
			transform.Translate(0, 0.05f * (timeRemaining / duration) + 0.025f, 0, Space.Self);
			yield return new WaitForSecondsRealtime(0.02f);
			timeRemaining -= 0.02f;
		}

		Destroy(gameObject);
	}
}
