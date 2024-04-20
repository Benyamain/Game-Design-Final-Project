using UnityEngine;
using Fusion;

namespace SimpleFPS
{
	public class UIMenuView : MonoBehaviour
	{
		private GameUI _gameUI;

		private CursorLockMode _previousLockState;
		private bool _previousCursorVisibility;

		// Called from button OnClick event.
		public void ResumeGame()
		{
			gameObject.SetActive(false);
		}

		// Called from button OnClick event.
		public void OpenSettings()
		{
			_gameUI.SettingsView.gameObject.SetActive(true);
		}

		// Called from button OnClick event.
		public void LeaveGame()
		{
			// Clear previous cursor state so it does not get locked when unloading scene.
			_previousLockState = CursorLockMode.None;
			_previousCursorVisibility = true;

			_gameUI.GoToMenu();
		}

		public void Respawn()
		{
			//Debug.Log("Respawn");
			if (_gameUI != null && _gameUI.Gameplay != null && _gameUI.Gameplay.Runner != null && _gameUI.Gameplay.Runner.LocalPlayer != null)
			{
				//Debug.Log("Respawn 2");
				var playerRef = _gameUI.Gameplay.Runner.LocalPlayer;
				if (_gameUI.Gameplay.PlayerData.ContainsKey(playerRef))
				{
					StartCoroutine(_gameUI.Gameplay.RespawnPlayer(playerRef, 0f));
				}
			}
			gameObject.SetActive(false);
			Cursor.lockState = _previousLockState;
		}



		private void Awake()
		{
			_gameUI = GetComponentInParent<GameUI>();
		}

		private void OnEnable()
		{
			_previousLockState = Cursor.lockState;
			_previousCursorVisibility = Cursor.visible;

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		private void OnDisable()
		{
			Cursor.lockState = _previousLockState;
			Cursor.visible = _previousCursorVisibility;
		}
	}
}
