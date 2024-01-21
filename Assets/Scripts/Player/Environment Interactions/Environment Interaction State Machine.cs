using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Animations.Rigging;

public class EnvironmentInteractionStateMachine : StateManager<EnvironmentInteractionStateMachine.EEnvironmentInteractionStateMachine>
{
    public enum EEnvironmentInteractionStateMachine {
        Search,
        Rise,
        Approach,
        Touch,
        Reset
    }

    [SerializeField] private TwoBoneIKConstraint _LeftIkConstraint;
    [SerializeField] private TwoBoneIKConstraint _RightIkConstraint;
    [SerializeField] private MultiRotationConstraint _LeftMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint _RightMultiRotationConstraint;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CapsuleCollider _rootCollider;

    private void Awake()
    {
        ValidateConstraints();
    }

    private void ValidateConstraints() {
        Assert.IsNotNull(_LeftIkConstraint, "Left IK not Assigned");
        Assert.IsNotNull(_RightIkConstraint, "Right IK not Assigned");
        Assert.IsNotNull(_LeftMultiRotationConstraint, "Left Multi Rotation not Assigned");
        Assert.IsNotNull(_RightMultiRotationConstraint, "Right Multi Rotation not Assigned");
        Assert.IsNotNull(_rigidbody, "Rigidbody not Assigned");
        Assert.IsNotNull(_rootCollider, "Capsule Collider not Assigned");
    }
}
