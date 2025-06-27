using Assets._Project.Develop.Runtime.Gameplay.GameRules;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Progress;
using Assets._Project.Develop.Runtime.UI.Wallet;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
	public class GameplayScreenPresenter : IPresenter
	{
		private readonly GameplayScreenView _screen;
		private readonly ProjectPresentersFactory _projectPresentersFactory;
		private readonly IRule _rule;
		private readonly GameplayPopupService _popupService;

		private readonly List<IPresenter> _childPresenters = new();

		public GameplayScreenPresenter(
			GameplayScreenView screen,
			ProjectPresentersFactory projectPresentersFactory,
			IRule rule,
			GameplayPopupService popupService)
		{
			_screen = screen;
			_projectPresentersFactory = projectPresentersFactory;
			_rule = rule;
			_popupService = popupService;
		}

		public void Initialize()
		{
			_screen.IsTyped += OnPlayerTyped;
			_rule.IsGenerated += OnGenerated;
			_rule.IsMatch += OnGameEnded;
			_rule.IsNotMatch += OnGameEnded;

			CreateWallet();

			CreateProgressBar();

			foreach (IPresenter presenter in _childPresenters)
				presenter.Initialize();
		}

		public void Dispose()
		{
			_screen.IsTyped -= OnPlayerTyped;
			_rule.IsGenerated -= OnGenerated;
			_rule.IsMatch += OnGameEnded;
			_rule.IsNotMatch += OnGameEnded;

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

		private void OnGameEnded() => _popupService.OpenEndGamePopup();		

		private void OnGenerated(string generatedText) => _screen.SetText(generatedText);		

		private void OnPlayerTyped(string playerInput) => _rule.Check(playerInput);		
	}
}
