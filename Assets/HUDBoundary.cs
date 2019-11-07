using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDBoundary : MonoBehaviour
{
	public static HUDBoundary instance;
	private RectTransform rectTransform;
	private const int CanvasResolutionHeight = 720;

	private void Start()
	{
		rectTransform = GetComponent<RectTransform>();
		instance = this;
	}

	/// <summary>
	/// Checks if the cursor is within the bounds of the rect. 
	/// This function makes the following assumptions: 
	/// - The rect of this game object is scaling from the right bound of the screen.
	/// - The canvas target resolution width is equal to CanvasResolutionWidth.
	/// </summary>
	/// <returns>True when mouse is over, false when mouse is outside.</returns>
	public bool GetMouseOverState()
	{
		float resolutionScalar = (float)Screen.height / CanvasResolutionHeight;
		float objective = rectTransform.rect.height;
		return (Input.mousePosition.y / resolutionScalar) <= objective;
	}
}
