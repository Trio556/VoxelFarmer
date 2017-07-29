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

        newChunk.pos = worldPos;
        newChunk.world = this;

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
        float multiple = Chunk.chunkSize;
        pos.x = Mathf.FloorToInt(x / multiple) * Chunk.chunkSize;
        pos.y = Mathf.FloorToInt(y / multiple) * Chunk.chunkSize;
        pos.z = Mathf.FloorToInt(z / multiple) * Chunk.chunkSize;
        Chunk containerChunk = null;
        chunks.TryGetValue(pos, out containerChunk);

        return containerChunk;
    }
    public Block GetBlock(int x, int y, int z)
    {
        Chunk containerChunk = GetChunk(x, y, z);
        if (containerChunk != null)
        {
            Block block = containerChunk.GetBlock(x - containerChunk.pos.x, y - containerChunk.pos.y, z - containerChunk.pos.z);

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
            chunk.SetBlock(x - (isAdd ? -1 * chunk.pos.x : chunk.pos.x), y - (isAdd ? -1 * chunk.pos.y : chunk.pos.y), z - (isAdd ? -1 * chunk.pos.z : chunk.pos.z), block);
            chunk.update = true;
            
            UpdateIfEqual(x - (isAdd ? -1 * chunk.pos.x : chunk.pos.x), 0, new WorldPos(x - (isAdd ? -1 * 1 : 1), y, z));
            UpdateIfEqual(x - (isAdd ? -1 * chunk.pos.x : chunk.pos.x), Chunk.chunkSize - (isAdd ? -1 * 1 : 1), new WorldPos(x + 1, y, z));
            UpdateIfEqual(y - (isAdd ? -1 * chunk.pos.y : chunk.pos.y), 0, new WorldPos(x, y - (isAdd ? -1 * 1 : 1), z));
            UpdateIfEqual(y - (isAdd ? -1 * chunk.pos.y : chunk.pos.y), Chunk.chunkSize - (isAdd ? -1 * 1 : 1), new WorldPos(x, y + (isAdd ? -1 * 1 : 1), z));
            UpdateIfEqual(z - (isAdd ? -1 * chunk.pos.z : chunk.pos.z), 0, new WorldPos(x, y, z - (isAdd ? -1 * 1 : 1)));
            UpdateIfEqual(z - (isAdd ? -1 * chunk.pos.z : chunk.pos.z), Chunk.chunkSize - (isAdd ? -1 * 1 : 1), new WorldPos(x, y, z + (isAdd ? -1 * 1 : 1)));
        }
    }

    void UpdateIfEqual(int value1, int value2, WorldPos pos)
    {
        if (value1 == value2)
        {
            Chunk chunk = GetChunk(pos.x, pos.y, pos.z);
            if (chunk != null)
                chunk.update = true;
        }
    }
}
