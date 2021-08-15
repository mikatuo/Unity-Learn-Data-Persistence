using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
	public class GameState : MonoBehaviour
	{
		public string PlayerName;
		public int PlayerScore;
		public GameScore BestScore;

		public static GameState Instance;

		public void SetPlayerName(string value)
		{
			PlayerName = value;
		}

		public void SetPoints(int value)
		{
			PlayerScore = value;
		}

		public void MaybeAddPlayersScoreToBestScore()
		{
			Debug.Log("Checking player's score...");
			if (BestScore == null || PlayerScore > 0 && PlayerScore > BestScore.ScorePoints) {
				Debug.Log("WELL DONE! BEST SCORE!");
				BestScore = new GameScore {
					PlayerName = PlayerName,
					ScorePoints = PlayerScore,
				};
			}
		}

		void Awake()
		{
			if (Instance != null) {
				Destroy(gameObject);
				return;
			}

			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		public void Save()
		{
			Debug.Log("Saving state...");
			var stateFilePath = $"{Application.persistentDataPath}/game-state.json";
			
			var saveState = new GameStateMemento {
				PlayerName = PlayerName,
				BestScore = BestScore,
			};
			var json = JsonUtility.ToJson(saveState);
			Debug.Log($"State JSON: {json}");
			File.WriteAllText(stateFilePath, json);
		}

		public void Load()
		{
			PlayerName = "";
			BestScore = null;

			Debug.Log("Loading state...");
			var stateFilePath = $"{Application.persistentDataPath}/game-state.json";
			if (!File.Exists(stateFilePath))
				return;

			var json = File.ReadAllText(stateFilePath);
			Debug.Log($"State JSON: {json}");
			var loadedState = JsonUtility.FromJson<GameStateMemento>(json);

			PlayerName = loadedState.PlayerName;
			if (loadedState.BestScore != null && loadedState.BestScore.ScorePoints > 0)
				BestScore = loadedState.BestScore;
		}
	}

	[System.Serializable]
	public class GameStateMemento
	{
		public string PlayerName;
		public GameScore BestScore;
	}
	
	[System.Serializable]
	public class GameScore
	{
		public string PlayerName;
		public int ScorePoints;
	}
}