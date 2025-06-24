using Assets._Project.Develop.Runtime.Gameplay.Progress;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Progress;
using Assets._Project.Develop.Runtime.UI.Wallet;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
	public class MainMenuScreenPresenter : IPresenter
	{
		private readonly MainMenuScreenView _screen;
		private readonly ProjectPresentersFactory _projectPresentersFactory;
		private readonly ProgressRestoreService _progressRestoreService;

		//private readonly MainMenuPopupService _popupService;

		private readonly List<IPresenter> _childPresenters = new();

		public MainMenuScreenPresenter(
			MainMenuScreenView screen,
			ProjectPresentersFactory projectPresentersFactory,
			ProgressRestoreService progressRestoreService)
		{
			_screen = screen;
			_projectPresentersFactory = projectPresentersFactory;
			_progressRestoreService = progressRestoreService;
		}

		public void Initialize()
		{
			_screen.ResetProgressButtonClicked += OnResetProgressButtonClicked;

			CreateWallet();

			CreateProgressBar();

			foreach (IPresenter presenter in _childPresenters)
				presenter.Initialize();
		}

		public void Dispose()
		{
			_screen.ResetProgressButtonClicked -= OnResetProgressButtonClicked;

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
			//Time.timeScale = 0;

			//_popupService.OpenLevelsMenuPopup();
			_progressRestoreService.SetInitialValues();
		}
	}
}
