using Assets._Project.Develop.Runtime.Configs.Gameplay;
using Assets._Project.Develop.Runtime.Gameplay.GameRules;
using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Gameplay.PlayerInput;
using Assets._Project.Develop.Runtime.Gameplay.Progress;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.GameProgress;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.GameModeManagement
{
	public class GameplayCycle : IDisposable
	{
		private readonly DIContainer _container;
		private readonly PlayerInputHandler _playerInputHandler;
		private readonly SymbolInputMode _mode;

		private WalletService _walletService;
		private ProgressService _progressService;

		private ConfigsProviderService _configProviderService;
		private PlayerDataProvider _playerDataProvider;
		private ICoroutinesPerformer _coroutinesPerformer;		
		private IRule _gameRule;

		public GameplayCycle(DIContainer container, PlayerInputHandler playerInputHandler, IInputSceneArgs sceneArgs = null)
		{
			_container = container;

			if (sceneArgs is not GameplayInputArgs gameplayInputArgs)
				throw new ArgumentException($"{nameof(sceneArgs)} is not match with {typeof(GameplayInputArgs)}");

			_playerInputHandler = playerInputHandler;
			_mode = gameplayInputArgs.Mode;
		}

		public IEnumerator Launch()
		{
			_walletService = _container.Resolve<WalletService>();
			_progressService = _container.Resolve<ProgressService>();

			_playerDataProvider = _container.Resolve<PlayerDataProvider>();
			_coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();
			_configProviderService = _container.Resolve<ConfigsProviderService>();

			_gameRule = new MatchSymbolsRule(_configProviderService, _mode, _playerInputHandler);
		
			_gameRule.IsMatch += OnGameModeWin;
			_gameRule.IsNotMatch += OnGameModeDefeat;

			yield return null;
		}

		public void Start() => _gameRule.Start();		

		public void Dispose() => OnGameModeEnded();		

		private void OnGameModeDefeat()
		{
			int goldValueToSpend = _container.Resolve<ConfigsProviderService>().GetConfig<GameplayConfig>().GetDefeatValue;

			if (_walletService.Enough(CurrencyTypes.Gold, goldValueToSpend))
			{
				_walletService.Spend(CurrencyTypes.Gold, goldValueToSpend);
				_progressService.Increase(GameProgressTypes.Defeat);

				_coroutinesPerformer.StartPerform(_playerDataProvider.Save());
				Debug.Log("Золота осталось: " + _walletService.GetCurrency(CurrencyTypes.Gold).Value);
			}

			OnGameModeEnded();
			Debug.Log("ПОРАЖЕНИЕ");
			_container.Resolve<ICoroutinesPerformer>().StartPerform(ResetProcess(Scenes.Gameplay));
		}

		private void OnGameModeWin()
		{
			int goldValueToAdd = _container.Resolve<ConfigsProviderService>().GetConfig<GameplayConfig>().GetWinValue;

			_walletService.Add(CurrencyTypes.Gold, goldValueToAdd);
			_progressService.Increase(GameProgressTypes.Win);

			_coroutinesPerformer.StartPerform(_playerDataProvider.Save());
			Debug.Log("Золота стало: " + _walletService.GetCurrency(CurrencyTypes.Gold).Value);

			OnGameModeEnded();
			Debug.Log("ПОБЕДА");
			_container.Resolve<ICoroutinesPerformer>().StartPerform(ResetProcess(Scenes.MainMenu));
		}

		private void OnGameModeEnded()
		{
			if (_gameRule != null)
				_gameRule.Dispose();
		}

		private IEnumerator ResetProcess(string sceneName)
		{
			yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

			SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();

			_container.Resolve<ICoroutinesPerformer>().StartPerform(sceneSwitcherService.ProcessSwitchTo(sceneName, new GameplayInputArgs(_mode)));
		}
	}
}
