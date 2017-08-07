using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Chunk : MonoBehaviour
{
    public static int _chunkSize = 16;
    public Block[,,] _blocks = new Block[_chunkSize, _chunkSize, _chunkSize];
    public bool _update = false;
    public bool _rendered;
    public World _world;
    public WorldPos _pos;

    private MeshFilter _filter;
    private MeshCollider _collider;

    // Use this for initialization
    void Start()
    {
        _filter = gameObject.GetComponent<MeshFilter>();
        _collider = gameObject.GetComponent<MeshCollider>();
    }

    //Update is called once per frame
    void Update()
    {
        if (_update)
        {
            _update = false;
            UpdateChunk();
            _rendered = true;
        }
    }

    public void SetBlocksUnmodified()
    {
        foreach (var block in _blocks)
        {
            block.changed = false;
        }
    }

    public Block GetBlock(int x, int y, int z)
    {
        if (InRange(x) && InRange(y) && InRange(z))
            return _blocks[x, y, z];

        return _world.GetBlock(_pos.x + x, _pos.y + y, _pos.z + z);
    }

    public void SetBlock(int x, int y, int z, Block block)
    {
        if (InRange(x) && InRange(y) && InRange(z))
        {
            _blocks[x, y, z] = block;
        }
        else
        {
            _world.SetBlock(_pos.x + x, _pos.y + y, _pos.z + z, block);
        }
    }

    public static bool InRange(int index)
    {
        if (index < 0 || index >= _chunkSize)
            return false;

        return true;
    }

    // Updates the chunk based on its contents
    void UpdateChunk()
    {
        MeshData meshData = new MeshData();

        for (int x = 0; x < _chunkSize; x++)
        {
            for (int y = 0; y < _chunkSize; y++)
            {
                for (int z = 0; z < _chunkSize; z++)
                {
                    meshData = _blocks[x, y, z].Blockdata(this, x, y, z, meshData);
                }
            }
        }

        RenderMesh(meshData);
    }

    // Sends the calculated mesh information
    // to the mesh and collision components
    void RenderMesh(MeshData meshData)
    {
        _filter.mesh.Clear();
        _filter.mesh.vertices = meshData.vertices.ToArray();
        _filter.mesh.triangles = meshData.triangles.ToArray();
        _filter.mesh.uv = meshData.uv.ToArray();
        _filter.mesh.RecalculateNormals();

        _collider.sharedMesh = null;
        Mesh mesh = new Mesh();
        mesh.vertices = meshData.colVertices.ToArray();
        mesh.triangles = meshData.colTriangles.ToArray();
        mesh.RecalculateNormals();

        _collider.sharedMesh = mesh;
    }

}