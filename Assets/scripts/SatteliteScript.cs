using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatteliteScript : EnemyBase {
    protected override void DoDeathSequence()
    {
        Destroy(this.gameObject);
    }

    protected override void EnemySpecificFixedUpdate()
    {
        //nothing
    }

    protected override void EnemySpecificStart()
    {
        //nothing
    }

    protected override void EnemySpecificUpdate()
    {
        //nothing here
    }
}
