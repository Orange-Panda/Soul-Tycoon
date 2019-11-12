﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the flow of time and game states.
/// </summary>
public class GameManager : MonoBehaviour
{
	public const byte hourLength = 2;
	public static byte hour = 1;
	public static uint day = 1;
	Coroutine hourCycle;
	public static GameSpeed gameSpeed = GameSpeed.Paused;
	GameManager instance;

	public delegate void TimeEvent();
	public static event TimeEvent HourTick = delegate { };
	public static event TimeEvent DayTick = delegate { };

	public static Dictionary<GameSpeed, float> speedValues = new Dictionary<GameSpeed, float>()
	{
		{ GameSpeed.Paused, 0.01f },
		{ GameSpeed.Slow, 0.55f },
		{ GameSpeed.Standard, 1f },
		{ GameSpeed.Fast, 1.75f },
		{ GameSpeed.VeryFast, 2.5f },
	};

	/// <summary>
	/// Modifies the time scale.
	/// </summary>
	public static void SetGameSpeed(GameSpeed speed)
	{
		Time.timeScale = speedValues[speed];
		gameSpeed = speed;
	}

	private void Start()
	{
		SetGameSpeed(GameSpeed.Paused);
		hourCycle = StartCoroutine(HourCycle());
		instance = this;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			if(Tile.highlightedTile != null)
			{
				Tile.highlightedTile.AttemptModification();
			}
		}
	}

	/// <summary>
	/// Responsible for progressing time in game.
	/// </summary>
	IEnumerator HourCycle()
	{
		while (true)
		{
			yield return new WaitForSeconds(hourLength);
			hour++;
			if (hour > 24)
			{
				hour = 1;
				day++;
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