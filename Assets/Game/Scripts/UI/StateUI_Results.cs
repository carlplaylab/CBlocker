using UnityEngine;
using UnityEngine.UI;

public class StateUI_Results : StateUI
{
	[SerializeField] private Text resultText;

	public override void UpdateResults (Results result)
	{
		string text = "Score   " + result.score.ToString("N0");
		text += "\nEnemies Killed  " + result.enemiesKilled.ToString("N0");
		text += "\nGirlfriend: NONE!";
		resultText.text = text;
	}
}
