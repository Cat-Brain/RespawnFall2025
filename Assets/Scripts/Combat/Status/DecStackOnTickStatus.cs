using UnityEngine;

[CreateAssetMenu(fileName = "New DecStackOnTickStatus", menuName = "StatusComponents/DecStackOnTickStatus")]
public class DecStackOnTickStatus : StatusComponent
{
    [Tooltip("The amount of stacks to decrease by per tick")]
    public int decrement;

    protected StackStatus stackStatus;

    public override void Str(StatusEffect effect)
    {
        stackStatus = effect.GetComponent<StackStatus>();
    }

    public override void Tick(StatusEffect effect)
    {
        stackStatus.ModifyStack(effect, -decrement);
    }
}
