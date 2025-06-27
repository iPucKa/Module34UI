using Assets._Project.Develop.Runtime.Gameplay.GameModeManagement;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;

namespace Assets._Project.Develop.Runtime.UI.Core.EndGamePopup
{
	public class EndGamePopupPresenter : PopupPresenterBase
	{
		private readonly EndGamePopupView _view;
		private readonly GameplayCycle _gameLogic;

		public EndGamePopupPresenter(
			EndGamePopupView view, 
			ICoroutinesPerformer coroutinesPerformer, 
			GameplayCycle gameLogic) : base(coroutinesPerformer)
		{
			_view = view;
			_gameLogic = gameLogic;
		}

		protected override PopupViewBase PopupView => _view;

		public override void Initialize()
		{
			base.Initialize();
			_view.SetText("GAME OVER");
		}

		protected override void OnPreHide()
		{
			_gameLogic.CanResetGame(true);

			base.OnPreHide();			
		}
	}
}
