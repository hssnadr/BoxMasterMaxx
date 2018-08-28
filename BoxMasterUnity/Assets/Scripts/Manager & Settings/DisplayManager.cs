using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour {
    /// <summary>
    /// The camera for the menu.
    /// </summary>
    [SerializeField]
    [Tooltip("The camera for the menu.")]
    private Camera _menuCamera = null;
    /// <summary>
    /// The camera for the big screen.
    /// </summary>
    [SerializeField]
    [Tooltip("The camera for the big screen.")]
    private Camera _bigScreenCamera = null;
	void Start () {
#if !UNITY_EDITOR
        Debug.Log("Displays connected: " + Display.displays.Length);
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();
        }
#endif
	}

    public void SwapDisplays()
    {
        int temp = _menuCamera.targetDisplay;
        _menuCamera.targetDisplay = _bigScreenCamera.targetDisplay;
        _bigScreenCamera.targetDisplay = temp;
    }
}
