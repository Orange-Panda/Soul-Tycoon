using TMPro;
using UnityEngine;

public static class Player
{
	public static int currency = 0;

	public delegate void CurrencyHandler();
	public static event CurrencyHandler CurrencyValueChanged = delegate { };

	/// <summary>
	/// Adds currency to the players balance. Will never fail.
	/// </summary>
	public static void Deposit(uint value, Vector3 position)
	{
		currency += (int)value;
		CurrencyValueChanged();
		GameObject go = Object.Instantiate(Resources.Load<GameObject>("Currency"), position - new Vector3(0, 0, 1), Quaternion.identity);
		TextMeshPro textMesh = go.GetComponent<TextMeshPro>();
		textMesh.SetText(string.Format("+{0}", value));
		textMesh.color = new Color(1f, 1f, 0.7f);
	}

	public static void SetCurrency(int value)
	{
		currency = value;
		CurrencyValueChanged();
	}

	/// <summary>
	/// Attempts to remove currency from the player account. If they have insufficient funds will deny the withdrawal and return false.
	/// </summary>
	public static bool Withdraw(uint value, Vector3 position)
	{
		bool insufficient = currency - value < 0;
		currency = insufficient ? currency : currency - (int)value;
		CurrencyValueChanged();
		GameObject go = Object.Instantiate(Resources.Load<GameObject>("Currency"), position - new Vector3(0, 0, 1), Quaternion.identity);
		TextMeshPro textMesh = go.GetComponent<TextMeshPro>();
		textMesh.SetText(string.Format(insufficient ? "Insufficient Funds!\n{1}/{0}" : "-{0}", value, currency));
		textMesh.color = new Color(1f, 0.7f, 0.7f);
		return !insufficient;
	}

	/// <summary>
	/// Forcibly withdraws currency.
	/// </summary>
	public static void ForceWithdraw(uint value, Vector3 position)
	{
		currency -= (int)value;
		CurrencyValueChanged();
		GameObject go = Object.Instantiate(Resources.Load<GameObject>("Currency"), position - new Vector3(0, 0, 1), Quaternion.identity);
		TextMeshPro textMesh = go.GetComponent<TextMeshPro>();
		textMesh.SetText(string.Format("-{0}", value));
		textMesh.color = new Color(1f, 0.7f, 0.7f);
	}
}
