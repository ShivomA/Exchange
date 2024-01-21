using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum {
    protected Dictionary<EState, BaseState<EState>> states = new();
    protected BaseState<EState> currentState;

    private bool isTransitioningState;

    void Start() {
        currentState.EnterState();
    }

    void Update() {
        EState nextStateKey = currentState.GetNextState();

        if (!isTransitioningState) {
            if (nextStateKey.Equals(currentState.stateKey)) {
                TransitionToState(nextStateKey);
            } else {
                currentState.UpdateState();
            }
        }
    }

    public void TransitionToState(EState stateKey) {
        isTransitioningState = true;
        currentState.ExitState();
        currentState = states[stateKey];
        currentState.EnterState();
        isTransitioningState = false;
    }

    private void OnTriggerEnter(Collider other) {
        currentState.OnTriggerEnter(other);
    }

    private void OnTriggerStay(Collider other) {
        currentState.OnTriggerStay(other);
    }

    private void OnTriggerExit(Collider other) {
        currentState.OnTriggerExit(other);
    }
}
