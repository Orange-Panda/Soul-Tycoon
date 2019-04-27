using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains static methods used in various scripts.
/// </summary>
public static class SoulTycoon
{
	public static Dictionary<Risk, float> riskProbability = new Dictionary<Risk, float>()
	{
		{ Risk.Protected, 0.00f },
		{ Risk.Low, 0.05f },
		{ Risk.Medium, 0.10f },
		{ Risk.High, 0.20f },
		{ Risk.Definite, 1f }
	};

	/// <summary>
	/// Attempts to have a risky event occur.
	/// </summary>
	public static bool AttemptRisk(Risk risk)
	{
		switch (risk)
		{
			case Risk.Definite:
				return true;
			case Risk.Protected:
				return false;
			default:
				return Random.Range(0f, 1f) < riskProbability[risk];
		}
	}

	/// <summary>
	/// Takes the base value and potentially adds to it based on the variance.
	/// </summary>
	public static uint VariableValue(uint baseValue, uint variance)
	{
		return baseValue + (uint)Mathf.RoundToInt(Random.Range(0f, variance));
	}
}
