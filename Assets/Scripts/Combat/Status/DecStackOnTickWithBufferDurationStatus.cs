using UnityEngine;

[CreateAssetMenu(fileName = "New DecStackOnTickWithBufferDurationStatus", menuName = "StatusComponents/DecStackOnTickWithBufferDurationStatus")]
public class DecStackOnTickWithBufferDurationStatus : DecStackOnTickStatus
{
    public enum StackMode
    {
        ADD, MAX
    }

    public float bufferDuration;
    public StackMode stackMode;

    public override void Reapply(StatusEffect effect, StatusComponent newApplicant)
    {
        DecStackOnTickWithBufferDurationStatus newStack = (DecStackOnTickWithBufferDurationStatus)newApplicant;

        switch (newStack.stackMode)
        {
            case StackMode.ADD:
                bufferDuration += newStack.bufferDuration;
                break;
            case StackMode.MAX:
                bufferDuration = Mathf.Max(bufferDuration, newStack.bufferDuration);
                break;
        }
    }

    public override void Str(StatusEffect effect)
    {
        stackStatus = effect.GetComponent<StackStatus>();
    }

    public override void Upd(StatusEffect effect)
    {
        bufferDuration -= Time.deltaTime;
    }

    public override void Tick(StatusEffect effect)
    {
        if (bufferDuration > 0)
            return;

        stackStatus.ModifyStack(effect, -decrement);
    }
}
