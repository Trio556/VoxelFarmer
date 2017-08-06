using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoadChunks : MonoBehaviour {

    public World world;
    int timer = 0;
    List<WorldPos> updateList = new List<WorldPos>();
    List<WorldPos> buildList = new List<WorldPos>();

    //chunk positions that never change
    static WorldPos[] chunkPositions; 

    private void Start()
    {
        var worldList = new List<WorldPos>();
        
        worldList.Add(new WorldPos(0, 0, 0));
        for (int i = 0; i < 20; i++)
        {
            for (int y = i; y >= 0; y--)
            {
                if (i == 0 && y == 0)
                    continue;

                worldList.Add(new WorldPos(i, 0, y));
                worldList.Add(new WorldPos(i, 0, y * -1));
                worldList.Add(new WorldPos(i * -1, 0, y));
                worldList.Add(new WorldPos(i * -1, 0, y * -1));
                worldList.Add(new WorldPos(y, 0, i));
                worldList.Add(new WorldPos(y, 0, i * -1));
                worldList.Add(new WorldPos(y * -1, 0, i));
                worldList.Add(new WorldPos(y * -1, 0, i * -1));
            }
        }

        chunkPositions = worldList.ToArray();
    }

    // Update is called once per frame
    void Update ()
    {
        if (!DeleteChunks())
        {
            FindChunksToLoad();
            LoadAndRenderChunks();
        }
    }

    void FindChunksToLoad()
    {
        //Get the position of this gameobject to generate around
        WorldPos playerPos = new WorldPos(
            Mathf.FloorToInt(transform.position.x / Chunk._chunkSize) * Chunk._chunkSize,
            Mathf.FloorToInt(transform.position.y / Chunk._chunkSize) * Chunk._chunkSize,
            Mathf.FloorToInt(transform.position.z / Chunk._chunkSize) * Chunk._chunkSize
            );
        //If there aren't already chunks to generate
        if (updateList.Count == 0)
        {
            //Cycle through the array of positions
            for (int i = 0; i < chunkPositions.Length; i++)
            {
                //translate the player position and array position into chunk position
                WorldPos newChunkPos = new WorldPos(
                    chunkPositions[i].x * Chunk._chunkSize + playerPos.x,
                    0,
                    chunkPositions[i].z * Chunk._chunkSize + playerPos.z
                    );
                //Get the chunk in the defined position
                Chunk newChunk = world.GetChunk(
                    newChunkPos.x, newChunkPos.y, newChunkPos.z);
                //If the chunk already exists and it's already
                //rendered or in queue to be rendered continue
                if (newChunk != null
                    && (newChunk._rendered || updateList.Contains(newChunkPos)))
                    continue;
                //load a column of chunks in this position
                for (int y = -4; y < 4; y++)
                {
                    for (int x = newChunkPos.x - Chunk._chunkSize; x <= newChunkPos.x + Chunk._chunkSize; x += Chunk._chunkSize)
                    {
                        for (int z = newChunkPos.z - Chunk._chunkSize; z <= newChunkPos.z + Chunk._chunkSize; z += Chunk._chunkSize)
                        {
                            buildList.Add(new WorldPos(
                                x, y * Chunk._chunkSize, z));
                        }
                    }
                    updateList.Add(new WorldPos(
                                newChunkPos.x, y * Chunk._chunkSize, newChunkPos.z));
                }
                return;
            }
        }
    }

    void BuildChunk(WorldPos pos)
    {
        if (world.GetChunk(pos.x, pos.y, pos.z) == null)
            world.CreateChunk(pos.x, pos.y, pos.z);
    }

    void LoadAndRenderChunks()
    {
        if (buildList.Count != 0)
        {
            for (int i = 0; i < buildList.Count && i < 8; i++)
            {
                BuildChunk(buildList[0]);
                buildList.RemoveAt(0);
            }
            //If chunks were built return early
            return;
        }
        if (updateList.Count != 0)
        {
            Chunk chunk = world.GetChunk(updateList[0].x, updateList[0].y, updateList[0].z);
            if (chunk != null)
                chunk._update = true;
            updateList.RemoveAt(0);
        }
    }

    bool DeleteChunks()
    {
        if (timer == 10)
        {
            var chunksToDelete = new List<WorldPos>();
            foreach (var chunk in world.chunks)
            {
                float distance = Vector3.Distance(
                    new Vector3(chunk.Value._pos.x, 0, chunk.Value._pos.z),
                    new Vector3(transform.position.x, 0, transform.position.z));
                if (distance > 256)
                    chunksToDelete.Add(chunk.Key);
            }
            foreach (var chunk in chunksToDelete)
                world.DestroyChunk(chunk.x, chunk.y, chunk.z);
            timer = 0;
            return true;
        }
        timer++;
        return false;
    }
}
