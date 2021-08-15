using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
	public class MainUI : MonoBehaviour
	{
		public void BackToMenu()
		{
			GameState.Instance?.Save();
			SceneManager.LoadScene("Start Menu");
		}
	}
}