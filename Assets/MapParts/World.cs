using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

    public Dictionary<WorldPos, Chunk> chunks = new Dictionary<WorldPos, Chunk>();
    public GameObject chunkPrefab;
    public string worldName = "world";

    public void CreateChunk(int x, int y, int z)
    {
        var worldPos = new WorldPos(x, y, z);

        //Instantiate the chunk at the coordinates using the chunk prefab
        var newChunkObject = Instantiate(chunkPrefab, new Vector3(x, y, z), Quaternion.Euler(Vector3.zero));
        var newChunk = newChunkObject.GetComponent<Chunk>();

        newChunk._pos = worldPos;
        newChunk._world = this;

        //Add  it to the shunks dictionary with the position as the key
        chunks.Add(worldPos, newChunk);

        var terrainGen = new TerrainGen();
        newChunk = terrainGen.ChunkGen(newChunk);

        newChunk.SetBlocksUnmodified();
        var loaded = Serialization.Load(newChunk);
    }

    public void DestroyChunk(int x, int y, int z)
    {
        Chunk chunk = null;
        if (chunks.TryGetValue(new WorldPos(x, y, z), out chunk))
        {
            Serialization.SaveChunk(chunk);
            Destroy(chunk.gameObject);
            chunks.Remove(new WorldPos(x, y, z));
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
        chunks.TryGetValue(pos, out containerChunk);

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

        if (chunk != null)
        {
            //TODO: Remove logic for flipping signs using isAdd and create a better way of achiving the same results that is cleaner
            chunk.SetBlock(x - (isAdd ? -1 * chunk._pos.x : chunk._pos.x), y - (isAdd ? -1 * chunk._pos.y : chunk._pos.y), z - (isAdd ? -1 * chunk._pos.z : chunk._pos.z), block);
            chunk._update = true;
            
            UpdateIfEqual(x - (isAdd ? -1 * chunk._pos.x : chunk._pos.x), 0, new WorldPos(x - (isAdd ? -1 * 1 : 1), y, z));
            UpdateIfEqual(x - (isAdd ? -1 * chunk._pos.x : chunk._pos.x), Chunk._chunkSize - (isAdd ? -1 * 1 : 1), new WorldPos(x + 1, y, z));
            UpdateIfEqual(y - (isAdd ? -1 * chunk._pos.y : chunk._pos.y), 0, new WorldPos(x, y - (isAdd ? -1 * 1 : 1), z));
            UpdateIfEqual(y - (isAdd ? -1 * chunk._pos.y : chunk._pos.y), Chunk._chunkSize - (isAdd ? -1 * 1 : 1), new WorldPos(x, y + (isAdd ? -1 * 1 : 1), z));
            UpdateIfEqual(z - (isAdd ? -1 * chunk._pos.z : chunk._pos.z), 0, new WorldPos(x, y, z - (isAdd ? -1 * 1 : 1)));
            UpdateIfEqual(z - (isAdd ? -1 * chunk._pos.z : chunk._pos.z), Chunk._chunkSize - (isAdd ? -1 * 1 : 1), new WorldPos(x, y, z + (isAdd ? -1 * 1 : 1)));
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
