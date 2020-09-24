using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Net_MessageTest : NetMeg
{
    public Net_MessageTest()
    {
        OP = NetOP.AnyMesage;
    }
    public string Test { set; get; }
}
