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
		public event Action CharModeSelected;
		public event Action NumberModeSelected;

		[field: SerializeField] public IconTextListView WalletView { get; private set; }
		[field: SerializeField] public IconTextListView ProgressView { get; private set; }

		[SerializeField] private Button _resetProgressButton;
		[SerializeField] private Button _charModeSelectButton;
		[SerializeField] private Button _numberModeSelectButton;

		private void OnEnable()
		{
			_resetProgressButton.onClick.AddListener(OnOpenLevelsMenuButtonClicked);
			_charModeSelectButton.onClick.AddListener(OnCharModeSelected);
			_numberModeSelectButton.onClick.AddListener(OnNumberModeSelected);
		}

		private void OnDisable()
		{
			_resetProgressButton.onClick.RemoveListener(OnOpenLevelsMenuButtonClicked);
			_charModeSelectButton.onClick.RemoveListener(OnCharModeSelected);
			_numberModeSelectButton.onClick.RemoveListener(OnNumberModeSelected);
		}

		private void OnNumberModeSelected() => NumberModeSelected?.Invoke();

		private void OnCharModeSelected() => CharModeSelected?.Invoke();


		private void OnOpenLevelsMenuButtonClicked() => ResetProgressButtonClicked?.Invoke();
	}
}
