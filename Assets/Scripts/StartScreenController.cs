using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenController : MonoBehaviour {

	public Button startButton;
	public Toggle easyToggle;
	public Toggle mediumToggle;
	public Toggle hardToggle;

	// Use this for initialization
	void Start () {
		Button btn = startButton.GetComponent<Button>();
		btn.onClick.AddListener(newGame);
	}

	private void newGame() {
		if (easyToggle.isOn) {
			GameConstants.numFoodPerLevel = 4;
			GameConstants.numPickaxesPerLevel = 2;
			GameConstants.numLevels = 2;
			GameConstants.minimumKeyDistance = 6;
			GameConstants.numEnemies = 5;
			GameConstants.levelSize = 15; // must be an odd number
		} else if (mediumToggle.isOn) {
			GameConstants.numFoodPerLevel = 5;
			GameConstants.numPickaxesPerLevel = 2;
			GameConstants.numLevels = 4;
			GameConstants.minimumKeyDistance = 12;
			GameConstants.numEnemies = 18;
			GameConstants.levelSize = 21; // must be an odd number
		} else if (hardToggle.isOn) {
			GameConstants.numFoodPerLevel = 6;
			GameConstants.numPickaxesPerLevel = 1;
			GameConstants.numLevels = 7;
			GameConstants.minimumKeyDistance = 17;
			GameConstants.numEnemies = 35;
			GameConstants.levelSize = 35; // must be an odd number
		}
		SceneManager.LoadScene("Scene1", LoadSceneMode.Single);
	}
}
