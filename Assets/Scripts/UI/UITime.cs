﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates the textMesh to reflect the current time in the game
/// </summary>
public class UITime : MonoBehaviour
{
	TextMeshProUGUI textMesh;
	public Image image;

	// Start is called before the first frame update
	void Start()
	{
		textMesh = GetComponent<TextMeshProUGUI>();
	}

	private void OnEnable()
	{
		GameManager.HourTick += UpdateText;
	}

	private void OnDisable()
	{
		GameManager.HourTick -= UpdateText;
	}

	private void UpdateText()
	{
		textMesh.SetText(string.Format("Day {0}", GameManager.day));
		image.fillAmount = (float)(GameManager.hour - 1) / 23;
	}
}