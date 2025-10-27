using UnityEngine;
using UnityEngine.SceneManagement;

namespace Starter.MainMenu
{
	public class UIMainMenu : MonoBehaviour
	{
		public void Select(int index)
		{
			SingletonChoose.Instance.OnRoleChanged(index);
		}
		public void LoadScene(int index)
		{
			//SingletonChoose.Instance.selectedMode = beHunter;
			SceneManager.LoadScene(index);
		}

		public void QuitGame()
		{
			Application.Quit();

			#if UNITY_EDITOR
				UnityEditor.EditorApplication.ExitPlaymode();
			#endif
		}

		private void OnEnable()
		{
			// Ensure the cursor is visible when coming back from the game
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		public void StartMenu()
        {

        }
	}
}
