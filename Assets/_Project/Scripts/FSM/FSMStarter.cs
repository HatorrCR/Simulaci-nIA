using UnityEngine;
using IAEngine.FSM;

public class FSMStarter : MonoBehaviour
{
    public FSMController fsmController;
    public FSMState initialState;

    void Start()
    {
        if (fsmController != null && initialState != null)
        {
            Debug.Log("[FSMStarter] Activando estado inicial: " + initialState.name);
            fsmController.ChangeState(initialState);
        }
        else
        {
            Debug.LogWarning("[FSMStarter] Falta asignar el FSMController o el estado inicial.");
        }
    }
}
