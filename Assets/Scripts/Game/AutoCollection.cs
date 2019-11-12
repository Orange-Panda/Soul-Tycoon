using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the ability to automatically collect currency from every building.
/// </summary>
public class AutoCollection : MonoBehaviour
{
	private bool collectionReady = false;
	private Button button;

	private void Start()
	{
		button = GetComponent<Button>();
		collectionReady = false;
		button.interactable = false;
	}

	private void OnEnable()
	{
		GameManager.DayTick += GameManager_DayTick;
	}

	private void OnDisable()
	{
		GameManager.DayTick -= GameManager_DayTick;
	}

	private void GameManager_DayTick()
	{
		Collect();
		collectionReady = true;
		button.interactable = true;
	}

	public void Collect()
	{
		if(collectionReady)
		{
			ProductionBuilding[] productionBuildings = FindObjectsOfType<ProductionBuilding>();
			foreach(ProductionBuilding building in productionBuildings)
			{
				building.GatherCurrency(false);
			}

			UIManager.audioSource.PlayOneShot(Resources.Load<AudioClip>("deposit"));
			collectionReady = false;
			button.interactable = false;
		}
		else
		{
			button.interactable = false;
		}
	}
}
