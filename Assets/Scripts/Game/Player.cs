using TMPro;
using UnityEngine;

public static class Player
{
	public static int Currency { get; private set; } = 0;

	public delegate void CurrencyHandler();
	public static event CurrencyHandler CurrencyValueChanged = delegate { };

	/// <summary>
	/// Adds currency to the players balance. Will never fail.
	/// </summary>
	public static void Deposit(uint value, Vector3 position, bool playSound = true)
	{
		Currency += (int)value;
		CurrencyValueChanged();
		GameObject go = Object.Instantiate(Resources.Load<GameObject>("Currency"), position - new Vector3(0, 0, 1), Quaternion.identity);
		TextMeshPro textMesh = go.GetComponent<TextMeshPro>();
		textMesh.SetText(string.Format("+{0}", value));
		textMesh.color = new Color(1f, 1f, 0.7f);
		if (playSound)
		{
			UIManager.audioSource.PlayOneShot(Resources.Load<AudioClip>("deposit"));
		}
	}

	/// <summary>
	/// Sets the currency to the exact value provided.
	/// </summary>
	/// <param name="value"></param>
	public static void SetCurrency(int value)
	{
		Currency = value;
		CurrencyValueChanged();
	}

	/// <summary>
	/// Attempts to remove currency from the player account. If they have insufficient funds will deny the withdrawal and return false.
	/// </summary>
	public static bool Withdraw(uint value, Vector3 position, bool playSound = true)
	{
		bool insufficient = Currency - value < 0;
		Currency = insufficient ? Currency : Currency - (int)value;
		CurrencyValueChanged();
		GameObject go = Object.Instantiate(Resources.Load<GameObject>("Currency"), position - new Vector3(0, 0, 1), Quaternion.identity);
		TextMeshPro textMesh = go.GetComponent<TextMeshPro>();
		textMesh.SetText(string.Format(insufficient ? "Insufficient Funds!\n{1}/{0}" : "-{0}", value, Currency));
		textMesh.color = new Color(1f, 0.7f, 0.7f);
		if (playSound)
		{
			UIManager.audioSource.PlayOneShot(Resources.Load<AudioClip>(insufficient ? "insufficient" : "withdraw"));
		}
		return !insufficient;
	}

	/// <summary>
	/// Forcibly withdraws currency.
	/// </summary>
	public static void ForceWithdraw(uint value, Vector3 position, bool playSound = true)
	{
		Currency -= (int)value;
		CurrencyValueChanged();
		GameObject go = Object.Instantiate(Resources.Load<GameObject>("Currency"), position - new Vector3(0, 0, 1), Quaternion.identity);
		TextMeshPro textMesh = go.GetComponent<TextMeshPro>();
		textMesh.SetText(string.Format("-{0}", value));
		textMesh.color = new Color(1f, 0.7f, 0.7f);
		if (playSound)
		{
			UIManager.audioSource.PlayOneShot(Resources.Load<AudioClip>("withdraw"));
		}
	}
}
