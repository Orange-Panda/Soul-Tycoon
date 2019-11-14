using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates the textMesh to reflect the current time in the game
/// </summary>
public class UITime : MonoBehaviour
{
	TextMeshProUGUI textMesh;
	public Image image;

	// Start is called before the first frame update
	void Start()
	{
		textMesh = GetComponent<TextMeshProUGUI>();
		UpdateText();
	}

	private void OnEnable()
	{
		GameManager.HourTick += UpdateText;
	}

	private void OnDisable()
	{
		GameManager.HourTick -= UpdateText;
	}

	/// <summary>
	/// Updates the text AND updates the bar.
	/// </summary>
	private void UpdateText()
	{
		textMesh.SetText(string.Format("Day {0}", GameManager.Day));
		image.fillAmount = Mathf.Min(Mathf.Max(((float)GameManager.Hour - 1) / 23, 0), 1);
	}
}
