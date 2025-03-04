using StarterAssets;
using UnityEngine;

public class VirtualInput : MonoBehaviour
{
    [Header("Output")]
    public StarterAssetsInputs StarterAssetsInputs;

    public void VirtualMoveInput(Vector2 virtualMoveDirection)
    {
        StarterAssetsInputs.MoveInput(virtualMoveDirection);
    }

    public void VirtualLookInput(Vector2 virtualLookDirection)
    {
        StarterAssetsInputs.LookInput(virtualLookDirection);
    }

    public void VirtualFireInput(bool virtualJumpState)
    {
        StarterAssetsInputs.FireInput(virtualJumpState);
    }

    public void VirtualSprintInput(bool virtualSprintState)
    {
        StarterAssetsInputs.SprintInput(virtualSprintState);
    }
}
