using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class NetOP
{

    public const int None = 0;
    public const int AnyMesage = 1;

}
[System.Serializable]
public abstract class NetMeg
{
    public byte OP { set; get; }
    public NetMeg()
        {
        OP = NetOP.None;

        }
}
