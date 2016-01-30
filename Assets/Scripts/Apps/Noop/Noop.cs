using UnityEngine;
using System.Collections;

public class Noop : AppBehaviourBase
{
    private bool _isActive;

    public override void Launch()
    {
        Debug.Log("Noop launch");

        _isActive = true;
    }

    public void OnGUI()
    {
        if (!_isActive)
        {
            return;
        }

        if (GUILayout.Button("NOPE"))
        {
            Kill();
        }
    }

    public override void Cleanup()
    {
        _isActive = false;

        Debug.Log("Noop cleanup");
    }
}
