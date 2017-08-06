using UnityEngine;
using System;

[Serializable]
public class Block
{
    public struct Tile { public int x; public int y; }
    const float tileSize = 0.25f;
    public bool changed = true;

    //Base block constructor
    public Block()
    {

    }

    public virtual Tile TexturePosition(Direction direction)
    {
        var tile = new Tile();
        tile.x = 0;
        tile.y = 0;
        return tile;
    }

    public virtual Vector2[] FaceBlockUVs(Direction direction)
    {
        var UVs = new Vector2[4];
        var tilePos = TexturePosition(direction);
        UVs[0] = new Vector2(tileSize * tilePos.x + tileSize, tileSize * tilePos.y);
        UVs[1] = new Vector2(tileSize * tilePos.x + tileSize, tileSize * tilePos.y + tileSize);
        UVs[2] = new Vector2(tileSize * tilePos.x, tileSize * tilePos.y + tileSize);
        UVs[3] = new Vector2(tileSize * tilePos.x, tileSize * tilePos.y);
        return UVs;
    }

    public virtual MeshData Blockdata(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.useRenderDataForCol = true;

        for (int i = 0; i < BlockHelper.GetDirectionArray().Length; i++)
        {
            var processingDirection = BlockHelper.GetDirectionArray()[i];
            var calculatingTemplate = BlockHelper.GetNeighboringBlockCalculationTemplate(processingDirection);

            if (!chunk.GetBlock(x + calculatingTemplate[0], y + calculatingTemplate[1], z + calculatingTemplate[2]).IsSolid(processingDirection))
                GetFaceData(x, y, z, BlockHelper.GetOppositeDirection(processingDirection), ref meshData);
        }

        return meshData;

    }

    protected virtual MeshData GetFaceData(int x, int y, int z, Direction uvDirection, ref MeshData blockMesh)
    {
        int[][] vertexOperators = BlockHelper.GetVertexCalculationTemplate(uvDirection);
        
        for (int i = 0; i < vertexOperators.Length; i++)
        {
            blockMesh.AddVertex(new Vector3(x + (vertexOperators[i][0] * 0.5f), y + (vertexOperators[i][1] * 0.5f), z + (vertexOperators[i][2] * 0.5f)));
        }

        blockMesh.AddQuadTriangles();
        blockMesh.uv.AddRange(FaceBlockUVs(uvDirection));
        return blockMesh;
    }

    /// <summary>
    /// Determines if a block is solid from a direction
    /// </summary>
    /// <remarks>
    /// Method will always return true unless overridden
    /// </remarks>
    /// <param name="direction"></param>
    /// <returns></returns>
    public virtual bool IsSolid(Direction direction)
    {
        return true;
    }
}