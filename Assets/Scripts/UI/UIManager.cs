using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the methods used by the interface.
/// </summary>
public class UIManager : MonoBehaviour
{
	public static AudioSource audioSource;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	/// <summary>
	/// Destroy the gameObject for a bit of currency.
	/// </summary>
	public void Sacrifice(GameObject gameObject)
	{
		Player.SetCurrency(1);
		GameManager.SetGameSpeed(GameSpeed.Standard);
		audioSource.PlayOneShot(Resources.Load<AudioClip>("small-click"));
		Destroy(gameObject);
	}

	/// <summary>
	/// Method for buttons to change the game speed.
	/// </summary>
	public void SetGameSpeed(int gameSpeed)
	{
		GameManager.SetGameSpeed((GameSpeed)gameSpeed);
		audioSource.PlayOneShot(Resources.Load<AudioClip>("small-click"));
	}

	/// <summary>
	/// Change to the argument scene
	/// </summary>
	public void SetScene(int index)
	{
		SceneManager.LoadScene(index);
		audioSource.PlayOneShot(Resources.Load<AudioClip>("small-click"));
	}
}
