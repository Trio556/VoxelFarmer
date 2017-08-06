using UnityEngine;
using System.Collections;
using SimplexNoise;
public class TerrainGen
{
    private float _stoneBaseHeight = -24;
    private float _stoneBaseNoise = 0.05f;
    private float _stoneBaseNoiseHeight = 4;
    private float _stoneMountainHeight = 48;
    private float _stoneMountainFrequency = 0.008f;
    private float _stoneMinHeight = -12;
    private float _dirtBaseHeight = 1;
    private float _dirtNoise = 0.04f;
    private float _dirtNoiseHeight = 3;
    private float _caveFrequency = 0.025f;
    private int _caveSize = 7;
    private float _treeFrequency = 0.2f;
    private int _treeDensity = 3;

    public Chunk ChunkGen(Chunk chunk)
    {
        for (int x = chunk._pos.x - 3; x < chunk._pos.x + Chunk._chunkSize + 3; x++)
        {
            for (int z = chunk._pos.z - 3; z < chunk._pos.z + Chunk._chunkSize + 3; z++)
            {
                chunk = ChunkColumnGen(chunk, x, z);
            }
        }
        return chunk;
    }

    public Chunk ChunkColumnGen(Chunk chunk, int x, int z)
    {
        int stoneHeight = GetStoneHeight(x, z);
        int dirtHeight = GetDirtHeight(x, z, stoneHeight);

        for (int y = chunk._pos.y - 8; y < chunk._pos.y + Chunk._chunkSize; y++)
        {
            //Get a value to base cave generation on
            int caveChance = GetNoise(x, y, z, _caveFrequency, 100);
            if (y <= stoneHeight && _caveSize < caveChance)
            {
                SetBlock(x, y, z, new Block(), chunk);
            }
            else if (y <= dirtHeight && _caveSize < caveChance)
            {
                SetBlock(x, y, z, new BlockGrass(), chunk);
                if (y == dirtHeight && GetNoise(x, 0, z, _treeFrequency, 100) < _treeDensity)
                    CreateTree(x, y + 1, z, chunk);
            }
            else
            {
                SetBlock(x, y, z, new BlockAir(), chunk);
            }
        }
        return chunk;
    }

    private int GetDirtHeight(int x, int z, int stoneHeight)
    {
        int dirtHeight = stoneHeight + Mathf.FloorToInt(_dirtBaseHeight);
        dirtHeight += GetNoise(x, 100, z, _dirtNoise, Mathf.FloorToInt(_dirtNoiseHeight));
        return dirtHeight;
    }

    private int GetStoneHeight(int x, int z)
    {
        int stoneHeight = Mathf.FloorToInt(_stoneBaseHeight);
        stoneHeight += GetNoise(x, 0, z, _stoneMountainFrequency, Mathf.FloorToInt(_stoneMountainHeight));
        if (stoneHeight < _stoneMinHeight)
            stoneHeight = Mathf.FloorToInt(_stoneMinHeight);
        stoneHeight += GetNoise(x, 0, z, _stoneBaseNoise, Mathf.FloorToInt(_stoneBaseNoiseHeight));
        return stoneHeight;
    }

    void CreateTree(int x, int y, int z, Chunk chunk)
    {
        //create leaves
        for (int xi = -2; xi <= 2; xi++)
        {
            for (int yi = 4; yi <= 8; yi++)
            {
                for (int zi = -2; zi <= 2; zi++)
                {
                    SetBlock(x + xi, y + yi, z + zi, new BlockLeaves(), chunk, true);
                }
            }
        }
        //create trunk
        for (int yt = 0; yt < 6; yt++)
        {
            SetBlock(x, y + yt, z, new BlockWood(), chunk, true);
        }
    }

    public static void SetBlock(int x, int y, int z, Block block, Chunk chunk, bool replaceBlocks = false)
    {
        x -= chunk._pos.x;
        y -= chunk._pos.y;
        z -= chunk._pos.z;
        if (Chunk.InRange(x) && Chunk.InRange(y) && Chunk.InRange(z))
        {
            if (replaceBlocks || chunk._blocks[x, y, z] == null)
                chunk.SetBlock(x, y, z, block);
        }
    }

    public static int GetNoise(int x, int y, int z, float scale, int max)
    {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }
}