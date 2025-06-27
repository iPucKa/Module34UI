using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Gameplay.Progress;
using Assets._Project.Develop.Runtime.Meta;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Progress;
using Assets._Project.Develop.Runtime.UI.Wallet;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
	public class MainMenuScreenPresenter : IPresenter
	{
		private readonly MainMenuScreenView _screen;
		private readonly ProjectPresentersFactory _projectPresentersFactory;
		private readonly ProgressRestoreService _progressRestoreService;
		private readonly ModeService _modeService;

		private GameplayInputArgs _args;

		//private readonly MainMenuPopupService _popupService;

		private readonly List<IPresenter> _childPresenters = new();

		public MainMenuScreenPresenter(
			MainMenuScreenView screen,
			ModeService modeService,
			ProjectPresentersFactory projectPresentersFactory,
			ProgressRestoreService progressRestoreService)
		{
			_screen = screen;
			_projectPresentersFactory = projectPresentersFactory;
			_progressRestoreService = progressRestoreService;
			_modeService = modeService;
		}

		public void Initialize()
		{
			_screen.ResetProgressButtonClicked += OnResetProgressButtonClicked;
			_screen.CharModeSelected += OnCharModeSelected;
			_screen.NumberModeSelected += OnNumberModeSelected;

			CreateWallet();

			CreateProgressBar();

			foreach (IPresenter presenter in _childPresenters)
				presenter.Initialize();
		}

		public void Dispose()
		{
			_screen.ResetProgressButtonClicked -= OnResetProgressButtonClicked;
			_screen.CharModeSelected -= OnCharModeSelected;
			_screen.NumberModeSelected -= OnNumberModeSelected;

			foreach (IPresenter presenter in _childPresenters)
				presenter.Dispose();

			_childPresenters.Clear();
		}

		private void CreateWallet()
		{
			WalletPresenter walletPresenter = _projectPresentersFactory.CreateWalletPresenter(_screen.WalletView);

			_childPresenters.Add(walletPresenter);
		}

		private void CreateProgressBar()
		{
			ProgressBarPresenter progressPresenter = _projectPresentersFactory.CreateProgressBarPresenter(_screen.ProgressView);

			_childPresenters.Add(progressPresenter);
		}

		private void OnResetProgressButtonClicked()
		{
			//_popupService.OpenLevelsMenuPopup();
			_progressRestoreService.SetInitialValues();
		}

		private void OnNumberModeSelected()
		{

			_args = new GameplayInputArgs(SymbolInputMode.Numbers);
			_modeService.MoveToGameplayScene(_args);

			Debug.Log("Выбран режим генерации цифр");
		}

		private void OnCharModeSelected()
		{
			_args = new GameplayInputArgs(SymbolInputMode.Chars);
			_modeService.MoveToGameplayScene(_args);

			Debug.Log("Выбран режим генерации букв");
		}
	}
}
