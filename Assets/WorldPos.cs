using System.Collections;
using UnityEngine;

public struct WorldPos
{
    public int x;
    public int y;
    public int z;

    public WorldPos(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is WorldPos))
            return false;
        var pos = (WorldPos)obj;

        if (pos.x != x || pos.y != y || pos.z != z)
            return false;

        return true;
    }
}