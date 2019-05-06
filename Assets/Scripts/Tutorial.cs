using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
	TextMeshPro textMesh;
	TutorialState state = TutorialState.Start;

	private void Start()
	{
		textMesh = GetComponent<TextMeshPro>();
		textMesh.SetText("");
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
		switch(state)
		{
			case TutorialState.Start:
				textMesh.SetText("<- Click to Buy Land");
				state = TutorialState.BuyLand;
				break;
			case TutorialState.BuyLand:
				textMesh.SetText("<- Click to Purchase Building");
				state = TutorialState.BuyBuilding;
				break;
			case TutorialState.BuyBuilding:
				textMesh.SetText("<- Click to Collect Souls When Glowing");
				state = TutorialState.Collect;
				break;
			case TutorialState.Collect:
				textMesh.SetText("Use Right Click to Sell or Repair");
				state = TutorialState.Repair;
				break;
			case TutorialState.Repair:
				textMesh.SetText("");
				Destroy(gameObject);
				break;
		}
	}
}

public enum TutorialState
{
	Start, BuyLand, BuyBuilding, Collect, Repair
}