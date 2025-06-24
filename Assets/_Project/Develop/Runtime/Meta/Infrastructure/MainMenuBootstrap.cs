using Assets._Project.Develop.Runtime.Gameplay.Progress;
using Assets._Project.Develop.Runtime.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.GameProgress;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
	public class MainMenuBootstrap : SceneBootstrap
	{
		private DIContainer _container;
		private ModeService _modeService;

		private WalletService _walletService;
		private ProgressService _progressService;
		private ProgressRestoreService _progressRestoreService;

		public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
		{
			_container = container;

			MainMenuContextRegistrations.Process(_container);
		}

		public override IEnumerator Initialize()
		{
			Debug.Log("Инициализация сцены меню");
			_walletService = _container.Resolve<WalletService>();
			_progressService = _container.Resolve<ProgressService>();
			_progressRestoreService = _container.Resolve<ProgressRestoreService>();

			_modeService = new ModeService(_container);

			yield break;
		}

		public override void Run()
		{			
			Debug.Log("Старт сцены меню");

			_container.Resolve<ICoroutinesPerformer>().StartPerform(_modeService.SelectMode());
		}

		private void Update()
		{

			if (Input.GetKeyDown(KeyCode.R))
			{
				_progressRestoreService.SetInitialValues();
			}

			if (Input.GetKeyDown(KeyCode.Space))
			{
				Debug.Log("Золото в наличии: " + _walletService.GetCurrency(CurrencyTypes.Gold).Value);
			}			

			if (Input.GetKeyDown(KeyCode.F))
			{
				Debug.Log("Число побед: " + _progressService.GetProgress(GameProgressTypes.Win).Value + ", число поражений: " + _progressService.GetProgress(GameProgressTypes.Defeat).Value);				
			}
		}
	}
}
