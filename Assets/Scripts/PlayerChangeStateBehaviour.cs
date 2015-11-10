using UnityEngine;
using System.Collections;

public class PlayerChangeStateBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.OnStartChangeStateAnimation();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.OnEndChangeStateAnimation();
    }

    #region Properties

    protected PlayerController player
    {
        get { return GameManager.player; }
    }

    #endregion
}
