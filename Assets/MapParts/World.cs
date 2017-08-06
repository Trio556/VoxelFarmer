using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

    public Dictionary<WorldPos, Chunk> _chunks = new Dictionary<WorldPos, Chunk>();

    //TODO: Research why an underscore prevents prefab from binding
    public GameObject chunkPrefab;
    public string _worldName = "world";

    public void CreateChunk(int x, int y, int z)
    {
        var worldPos = new WorldPos(x, y, z);

        //Instantiate the chunk at the coordinates using the chunk prefab
        var newChunkObject = Instantiate(chunkPrefab, new Vector3(x, y, z), Quaternion.Euler(Vector3.zero));
        var newChunk = newChunkObject.GetComponent<Chunk>();

        newChunk._pos = worldPos;
        newChunk._world = this;

        //Add  it to the chunks dictionary with the position as the key
        _chunks.Add(worldPos, newChunk);

        var terrainGen = new TerrainGen();
        newChunk = terrainGen.ChunkGen(newChunk);

        newChunk.SetBlocksUnmodified();
        var loaded = Serialization.Load(newChunk);
    }

    public void DestroyChunk(int x, int y, int z)
    {
        Chunk chunk = null;
        if (_chunks.TryGetValue(new WorldPos(x, y, z), out chunk))
        {
            Serialization.SaveChunk(chunk);
            Destroy(chunk.gameObject);
            _chunks.Remove(new WorldPos(x, y, z));
        }
    }

    public Chunk GetChunk(int x, int y, int z)
    {
        WorldPos pos = new WorldPos();
        float multiple = Chunk._chunkSize;
        pos.x = Mathf.FloorToInt(x / multiple) * Chunk._chunkSize;
        pos.y = Mathf.FloorToInt(y / multiple) * Chunk._chunkSize;
        pos.z = Mathf.FloorToInt(z / multiple) * Chunk._chunkSize;
        Chunk containerChunk = null;
        _chunks.TryGetValue(pos, out containerChunk);

        return containerChunk;
    }
    public Block GetBlock(int x, int y, int z)
    {
        Chunk containerChunk = GetChunk(x, y, z);
        if (containerChunk != null)
        {
            Block block = containerChunk.GetBlock(x - containerChunk._pos.x, y - containerChunk._pos.y, z - containerChunk._pos.z);

            return block;
        }

        return new BlockAir();
    }

    public void SetBlock(int x, int y, int z, Block block, bool isAdd = false)
    {
        Chunk chunk = GetChunk(x, y, z);
        //Added position modifier for flipping between negative and positives
        //Used mainly for Leaf blocks
        var positionModifier = isAdd ? -1 : 1;

        if (chunk != null)
        {
            chunk.SetBlock(x - (positionModifier * chunk._pos.x), y - (positionModifier * chunk._pos.y), z - (positionModifier * chunk._pos.z), block);
            chunk._update = true;
            
            UpdateIfEqual(x - (positionModifier * chunk._pos.x), 0, new WorldPos(x - positionModifier, y, z));
            UpdateIfEqual(x - (positionModifier * chunk._pos.x), Chunk._chunkSize - positionModifier, new WorldPos(x + 1, y, z));
            UpdateIfEqual(y - (positionModifier * chunk._pos.y), 0, new WorldPos(x, y - positionModifier, z));
            UpdateIfEqual(y - (positionModifier * chunk._pos.y), Chunk._chunkSize - positionModifier, new WorldPos(x, y + positionModifier, z));
            UpdateIfEqual(z - (positionModifier * chunk._pos.z), 0, new WorldPos(x, y, z - positionModifier));
            UpdateIfEqual(z - (positionModifier * chunk._pos.z), Chunk._chunkSize - positionModifier, new WorldPos(x, y, z + positionModifier));
        }
    }

    void UpdateIfEqual(int value1, int value2, WorldPos pos)
    {
        if (value1 == value2)
        {
            Chunk chunk = GetChunk(pos.x, pos.y, pos.z);
            if (chunk != null)
                chunk._update = true;
        }
    }
}
