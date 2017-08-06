using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Save
{
    public Dictionary<WorldPos, Block> blocks = new Dictionary<WorldPos, Block>();

    public Save(Chunk chunk)
    {
        for (int x = 0; x < Chunk._chunkSize; x++)
        {
            for (int y = 0; y < Chunk._chunkSize; y++)
            {
                for (int z = 0; z < Chunk._chunkSize; z++)
                {
                    if (!chunk._blocks[x, y, z].changed)
                        continue;

                    WorldPos pos = new WorldPos(x, y, z);
                    blocks.Add(pos, chunk._blocks[x, y, z]);
                }
            }
        }
    }
}
