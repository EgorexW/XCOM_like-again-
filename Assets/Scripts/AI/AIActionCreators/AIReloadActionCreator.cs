public class AIReloadActionCreator : BasicAIActionCreator{
    public override AIAction CreateAIAction(AIContext context){
        var action = base.CreateAIAction(context);
        var ammoComponent = context.unit.GetCombatComponent<AmmoComponent>();
        if (ammoComponent != null){
            if (ammoComponent.IsEmpty){
                action.AddFlag(AIActionFlags.MagazineEmpty);
            }
        }
        return action;
    }
}