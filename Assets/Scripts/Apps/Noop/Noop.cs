using UnityEngine;
using System.Collections;

public class Noop : AppBehaviour {

    public override void Launch()
    {
        Debug.Log("Noop launch");
    }

    public void OnGUI()
    {
        if (GUILayout.Button("NOPE"))
        {
            Kill();
        }
    }

    public override void Cleanup()
    {
        Debug.Log("Noop cleanup");
    }
}
