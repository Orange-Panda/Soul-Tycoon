using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the flow of time and game states.
/// </summary>
public class GameManager : MonoBehaviour
{
	//Fields
	public const byte HourLength = 2;
	public static GameSpeed gameSpeed = GameSpeed.Paused;

	//Events
	public delegate void TimeEvent();
	public static event TimeEvent HourTick = delegate { };
	public static event TimeEvent DayTick = delegate { };

	//Dictionaries
	public static Dictionary<GameSpeed, float> speedValues = new Dictionary<GameSpeed, float>()
	{
		{ GameSpeed.Paused, 0.01f },
		{ GameSpeed.Slow, 0.55f },
		{ GameSpeed.Standard, 1f },
		{ GameSpeed.Fast, 1.75f },
		{ GameSpeed.VeryFast, 2.5f },
	};

	//Properties
	public static byte Hour { get; private set; } = 1;
	public static uint Day { get; private set; } = 1;

	private void Start()
	{
		SetGameSpeed(GameSpeed.Paused);
		StartCoroutine(HourCycle());
		Day = 1;
		Hour = 1;
		Player.SetCurrency(0);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse1) && Tile.highlightedTile != null)
		{
			Tile.highlightedTile.AttemptModification();
		}
	}

	/// <summary>
	/// Modifies the time scale.
	/// </summary>
	public static void SetGameSpeed(GameSpeed speed)
	{
		Time.timeScale = speedValues[speed];
		gameSpeed = speed;
	}

	/// <summary>
	/// Responsible for progressing time in game.
	/// </summary>
	IEnumerator HourCycle()
	{
		while (true)
		{
			yield return new WaitForSeconds(HourLength);
			Hour++;
			if (Hour > 24)
			{
				Hour = 1;
				Day++;
				DayTick();
			}
			HourTick();
		}
	}
}

public enum GameSpeed
{
	Paused, Slow, Standard, Fast, VeryFast
}