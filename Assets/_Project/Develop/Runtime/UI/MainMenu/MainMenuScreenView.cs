using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
	public class MainMenuScreenView : MonoBehaviour, IView
	{
		public event Action ResetProgressButtonClicked;

		[field: SerializeField] public IconTextListView WalletView { get; private set; }
		[field: SerializeField] public IconTextListView ProgressView { get; private set; }

		[SerializeField] private Button _resetProgressButton;

		private void OnEnable()
		{
			_resetProgressButton.onClick.AddListener(OnOpenLevelsMenuButtonClicked);
		}

		private void OnDisable()
		{
			_resetProgressButton.onClick.RemoveListener(OnOpenLevelsMenuButtonClicked);
		}

		private void OnOpenLevelsMenuButtonClicked() => ResetProgressButtonClicked?.Invoke();
	}
}
