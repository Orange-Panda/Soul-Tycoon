using TMPro;
using UnityEngine;

/// <summary>
/// Updates the text of the gameObject to reflect the currency balance.
/// </summary>
public class UICurrency : MonoBehaviour
{
	public string prefix = "$";
	private int displayValue;
	private TextMeshProUGUI textMesh;

	private void Start()
	{
		textMesh = GetComponent<TextMeshProUGUI>();
	}

	void FixedUpdate()
	{
		if (displayValue != Player.currency)
		{
			displayValue = displayValue > Player.currency ? Mathf.FloorToInt(Mathf.Lerp(displayValue, Player.currency, 0.1f)) : Mathf.CeilToInt(Mathf.Lerp(displayValue, Player.currency, 0.1f));
			textMesh.SetText(string.Format("{0}{1}", prefix, displayValue));
		}
	}
}
