using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;

public class StartMenu : MonoBehaviour
{
	[SerializeField]
	TMP_InputField PlayerNameInput;
	[SerializeField]
	TMP_Text[] HallOfFamePlayers;

	void Start()
	{
		var gameState = GameState.Instance;
		gameState.Load();

		// set player name
		PlayerNameInput.text = gameState.PlayerName;
		// set high scores
		var bestScore = gameState.BestScore;
		if (bestScore != null)
			HallOfFamePlayers[0].text = $"{bestScore.PlayerName} - {bestScore.ScorePoints} point(s)";
	}

	public void Play()
	{
		GameState.Instance.Save();
		SceneManager.LoadScene("Main");
	}

	public void Quit()
	{
		GameState.Instance.Save();
#if UNITY_EDITOR
		EditorApplication.ExitPlaymode();
#else
		Application.Quit();
#endif
	}
}
