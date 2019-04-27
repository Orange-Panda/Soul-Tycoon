using System.Collections;
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

	IEnumerator UpdateCurrencyValue()
	{
		while(displayValue != Player.currency)
		{
			displayValue = displayValue > Player.currency ? Mathf.FloorToInt(Mathf.Lerp(displayValue, Player.currency, 0.1f)) : Mathf.CeilToInt(Mathf.Lerp(displayValue, Player.currency, 0.15f));
			textMesh.SetText(string.Format("{0}{1}", prefix, displayValue));
			yield return new WaitForSecondsRealtime(0.1f);
		}
		yield break;
	}
}
