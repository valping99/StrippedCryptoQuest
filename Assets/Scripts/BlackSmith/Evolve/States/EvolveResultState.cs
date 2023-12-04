namespace CryptoQuest.BlackSmith.Evolve.States
{
    public class EvolveSuccess : EvolveStateBase
    {
        public EvolveSuccess(EvolveStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            // _evolveResultPresenter.gameObject.SetActive(true);
            // _evolveResultPresenter.SetResultSuccess(null);
        }

        public override void OnExit()
        {
            base.OnExit();
            // _evolveResultPresenter.gameObject.SetActive(false);
        }

        public override void OnCancel()
        {
            base.OnCancel();
            // _evolvableEquipmentsPresenter.ReloadEquipments();
            // _evolvableEquipmentsPresenter.ClearMaterialEquipmentsIfExist();
            // _evolveStateMachine.RequestStateChange(State.SELECT_EQUIPMENT);
        }
    }
}