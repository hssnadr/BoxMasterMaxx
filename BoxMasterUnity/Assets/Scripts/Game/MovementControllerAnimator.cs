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
            GetComponent<Animator>().SetInteger("SphereCount", SphereNumber(GameManager.instance.gameplayManager.hitCount));
            GetComponent<Animator>().SetBool("1P", GameManager.instance.gameMode == GameMode.P1);
        }

        private int SphereNumber(int hitCount)
        {
            int[] threshold = GameManager.instance.gameplaySettings.sphereCountThreshold;
            GameMode mode = GameManager.instance.gameMode;
            for (int i = threshold.Length - 1; i >= 0; i--)
            {
                if ((mode == GameMode.P2 && hitCount >= threshold[i]) || (mode == GameMode.P1 && hitCount * 2 >= threshold[i]))
                    return i + 2;
            }
            return 0;
        }
    }
}
