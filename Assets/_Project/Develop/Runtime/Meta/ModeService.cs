using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta
{
	public class ModeService
	{
		private readonly SceneSwitcherService _sceneSwitcherService;
		private readonly ICoroutinesPerformer _coroutinesPerformer;

		private GameplayInputArgs _args;

		public ModeService(ICoroutinesPerformer coroutinesPerformer, SceneSwitcherService sceneSwitcherService)
		{
			_sceneSwitcherService = sceneSwitcherService;
			_coroutinesPerformer = coroutinesPerformer;
		}

		public IEnumerator SelectMode()
		{
			while (true)
			{
				if (Input.GetKeyUp(KeyCode.Alpha1))
				{
					_args = new GameplayInputArgs(SymbolInputMode.Numbers);
					yield return  MoveToGameplayScene();

					Debug.Log("Выбран режим генерации цифр");

					yield break;
				}

				if (Input.GetKeyUp(KeyCode.Alpha2))
				{
					_args = new GameplayInputArgs(SymbolInputMode.Chars);
					yield return MoveToGameplayScene();

					Debug.Log("Выбран режим генерации букв");

					yield break;
				}

				yield return null;
			}					
		}

		private IEnumerator MoveToGameplayScene()
		{
			yield return _coroutinesPerformer.StartPerform(_sceneSwitcherService.ProcessSwitchTo(Scenes.Gameplay, _args));
		}
	}
}
