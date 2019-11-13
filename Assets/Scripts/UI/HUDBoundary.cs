using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Checks if the cursor is within bounds of this rect.
/// </summary>
public class HUDBoundary : MonoBehaviour
{
	public static HUDBoundary instance;
	private RectTransform rectTransform;
	private int canvasResolutionHeight = 720;

	private void Start()
	{
		rectTransform = GetComponent<RectTransform>();
		canvasResolutionHeight = Mathf.RoundToInt(GetComponentInParent<CanvasScaler>().referenceResolution.y);
		instance = this;
	}

	/// <summary>
	/// Checks if the cursor is within the bounds of the rect. 
	/// </summary>
	/// <returns>True when mouse is over, false when mouse is outside.</returns>
	public bool GetMouseOverState()
	{
		float resolutionScalar = (float)Screen.height / canvasResolutionHeight;
		float objective = rectTransform.rect.height;
		return (Input.mousePosition.y / resolutionScalar) <= objective;
	}
}
