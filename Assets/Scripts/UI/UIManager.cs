using UnityEngine;

/// <summary>
/// Manages the methods used by the interface.
/// </summary>
public class UIManager : MonoBehaviour
{
	/// <summary>
	/// Destroy the gameObject for a bit of currency.
	/// </summary>
	public void Sacrifice(GameObject gameObject)
	{
		Player.Deposit(1);
		GameManager.SetGameSpeed(GameSpeed.Standard);
		Destroy(gameObject);
	}

	/// <summary>
	/// Method for buttons to change the game speed.
	/// </summary>
	public void SetGameSpeed(int gameSpeed)
	{
		GameManager.SetGameSpeed((GameSpeed)gameSpeed);
	}
}
