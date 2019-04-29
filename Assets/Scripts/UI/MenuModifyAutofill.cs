using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Automatically sets the value of the modify menu attributes.
/// </summary>
public class MenuModifyAutofill : MonoBehaviour
{
	public enum AutofillTarget { Repair, Sell }
	public AutofillTarget type;
	public TextMeshProUGUI cost, title;

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(UpdateText());
	}

	IEnumerator UpdateText()
	{
		MenuChoice choice = GetComponentInParent<MenuChoice>();
		bool useSign;

		//Waits until the next frame to continue. This is necessary because it is unlikely that the building is defined at this time.
		yield return null;

		switch (type)
		{
			//For the repair type, set the cost to the current repair cost and add a description of the durability to the title.
			case AutofillTarget.Repair:
				ProductionBuilding building = choice.targetTile.GetComponent<ProductionBuilding>();
				useSign = building.RepairCost != 0;
				cost.SetText(useSign ? "-" + building.RepairCost.ToString() : "" + building.RepairCost.ToString());
				title.SetText(title.text + " (" + building.durability + "/" + building.thisBuilding.durability + ")");
				cost.color = useSign ? new Color(0.25f, 0f, 0f) : Color.black;
				break;
			//For the sell type we set the cost field to the amount refuned to the player.
			case AutofillTarget.Sell:
				useSign = choice.targetTile.building.value != 0;
				cost.SetText(useSign ? "+" + choice.targetTile.building.value : "" + choice.targetTile.building.value);
				cost.color = useSign ? new Color(0f, 0.25f, 0f) : Color.black;
				break;
		}

		yield break;
	}
}
