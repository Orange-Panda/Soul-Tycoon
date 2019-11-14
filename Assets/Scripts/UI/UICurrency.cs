using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Updates the text of the gameObject to reflect the currency balance.
/// </summary>
public class UICurrency : MonoBehaviour
{
	private string prefix = "";
	private int displayValue;
	private TextMeshProUGUI textMesh;

	private void Start()
	{
		textMesh = GetComponent<TextMeshProUGUI>();
		StartCoroutine(UpdateCurrencyValue());
	}

	private void OnEnable()
	{
		Player.CurrencyValueChanged += Player_CurrencyValueChanged;
	}

	private void OnDisable()
	{
		Player.CurrencyValueChanged -= Player_CurrencyValueChanged;
	}

	private void Player_CurrencyValueChanged()
	{
		StopAllCoroutines();
		StartCoroutine(UpdateCurrencyValue());
	}

	/// <summary>
	/// Lerps the display value to the actual value over time.
	/// </summary>
	IEnumerator UpdateCurrencyValue()
	{
		//Set the color and wait a moment to let the player process the color change.
		yield return null;
		textMesh.color = displayValue > Player.Currency ? new Color(1f, 0.8f, 0.8f) : new Color(0.8f, 1f, 0.8f);
		yield return new WaitForSecondsRealtime(0.1f);

		//Every 0.065 seconds, change the value to be closer to the true value.
		do
		{
			displayValue = displayValue > Player.Currency ? Mathf.FloorToInt(Mathf.Lerp(displayValue, Player.Currency, 0.5f)) : Mathf.CeilToInt(Mathf.Lerp(displayValue, Player.Currency, 0.3f));
			textMesh.SetText(string.Format("{0}{1}", prefix, displayValue));
			yield return new WaitForSecondsRealtime(0.065f);
		} while (displayValue != Player.Currency);

		textMesh.color = Color.white;
		yield break;
	}
}
