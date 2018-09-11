using System;
using UnityEngine;

namespace CRI.HitBox.Game
{
    /// <summary>
    /// Manages the animator of the movement controller
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class MovementControllerAnimator : MonoBehaviour
    {
        private void Update()
        {
            GetComponent<Animator>().SetInteger("Score", GameManager.instance.playerScore);
            GetComponent<Animator>().SetBool("1P", GameManager.instance.gameMode == GameMode.P1);
        }
    }
}
