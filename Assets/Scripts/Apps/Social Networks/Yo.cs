﻿using UnityEngine;
using System.Collections;

public class Yo : AppBehaviourBase {
    public override void Launch()
    {
        Debug.Log("yo");


    }

    public override void Cleanup()
    {
        throw new System.NotImplementedException();
    }
}
