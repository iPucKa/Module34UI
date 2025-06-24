using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.MainMenu;
using Assets._Project.Develop.Runtime.Utilities.AssetsManagement;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
	public class MainMenuContextRegistrations
	{
		public static void Process(DIContainer container)
		{
			//Регистрации			
			Debug.Log("Процесс регистрации сервисов на сцене меню");
			container.RegisterAsSingle(CreateModeService);
			container.RegisterAsSingle(CreateMainMenuPresentersFactory);
			container.RegisterAsSingle(CreateMainMenuScreenPresenter).NonLazy();
			container.RegisterAsSingle(CreateMainMenuUIRoot).NonLazy();

		}

		//Способ создания холста для Главного меню
		private static MainMenuUIRoot CreateMainMenuUIRoot(DIContainer c)
		{
			ResourcesAssetsLoader resourcesAssetsLoader = c.Resolve<ResourcesAssetsLoader>();

			MainMenuUIRoot mainMenuUIRootPrefab = resourcesAssetsLoader
				.Load<MainMenuUIRoot>("UI/MainMenu/MainMenuUIRoot");

			return Object.Instantiate(mainMenuUIRootPrefab);
		}

		//Способ создания фабрики всех презентеров главного меню
		private static MainMenuPresentersFactory CreateMainMenuPresentersFactory(DIContainer c)
			=> new MainMenuPresentersFactory(c);

		//Способ создания презентера главного меню
		private static MainMenuScreenPresenter CreateMainMenuScreenPresenter(DIContainer c)
		{
			MainMenuUIRoot uiRoot = c.Resolve<MainMenuUIRoot>();

			MainMenuScreenView view = c
				.Resolve<ViewsFactory>()
				.Create<MainMenuScreenView>(ViewIDs.MainMenuScreen, uiRoot.HUDLayer);

			MainMenuScreenPresenter presenter = c
				.Resolve<MainMenuPresentersFactory>()
				.CreateMainMenuScreen(view);

			return presenter;
		}

		//Способ создания режимов игры
		private static ModeService CreateModeService(DIContainer c) 
			=> new ModeService(c);		
	}
}
