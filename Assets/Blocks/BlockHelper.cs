using System;

public static class BlockHelper
{
    private static Direction[] directions = null;
    private static int[][] _vertexCalculationsEast = null;
    private static int[][] _vertexCalculationsWest = null;
    private static int[][] _vertexCalculationsNorth = null;
    private static int[][] _vertexCalculationsSouth = null;
    private static int[][] _vertexCalculationsUp = null;
    private static int[][] _vertexCalculationsDown = null;
    private static int[] _neighboringNorth = null;
    private static int[] _neighboringSouth = null;
    private static int[] _neighboringEast = null;
    private static int[] _neighboringWest = null;
    private static int[] _neighboringUp = null;
    private static int[] _neighboringDown = null;

    private static int[][] VertexCalculationsEast
    {
        get
        {
            if (_vertexCalculationsEast == null)
                _vertexCalculationsEast = SetVertexCalculations(Direction.east);
            return _vertexCalculationsEast;
        }
    }

    private static int[][] VertexCalculationsWest
    {
        get
        {
            if (_vertexCalculationsWest == null)
                _vertexCalculationsWest = SetVertexCalculations(Direction.west);
            return _vertexCalculationsWest;
        }
    }

    private static int[][] VertexCalculationsNorth
    {
        get
        {
            if (_vertexCalculationsNorth == null)
                _vertexCalculationsNorth = SetVertexCalculations(Direction.north);
            return _vertexCalculationsNorth;
        }
    }

    private static int[][] VertexCalculationsSouth
    {
        get
        {
            if (_vertexCalculationsSouth == null)
                _vertexCalculationsSouth = SetVertexCalculations(Direction.south);
            return _vertexCalculationsSouth;
        }
    }

    private static int[][] VertexCalculationsUp
    {
        get
        {
            if (_vertexCalculationsUp == null)
                _vertexCalculationsUp = SetVertexCalculations(Direction.up);
            return _vertexCalculationsUp;
        }
    }

    private static int[][] VertexCalculationsDown
    {
        get
        {
            if (_vertexCalculationsDown == null)
                _vertexCalculationsDown = SetVertexCalculations(Direction.down);
            return _vertexCalculationsDown;
        }
    }

    private static int[] NeighboringNorth
    {
        get
        {
            if (_neighboringNorth == null)
                _neighboringNorth = SetNeighboringBlockCalculations(Direction.north);
            return _neighboringNorth;
        }
    }

    private static int[] NeighboringSouth
    {
        get
        {
            if (_neighboringSouth == null)
                _neighboringSouth = SetNeighboringBlockCalculations(Direction.south);
            return _neighboringSouth;
        }
    }

    private static int[] NeighboringEast
    {
        get
        {
            if (_neighboringEast == null)
                _neighboringEast = SetNeighboringBlockCalculations(Direction.east);
            return _neighboringEast;
        }
    }

    private static int[] NeighboringWest
    {
        get
        {
            if (_neighboringWest == null)
                _neighboringWest = SetNeighboringBlockCalculations(Direction.west);
            return _neighboringWest;
        }
    }

    private static int[] NeighboringUp
    {
        get
        {
            if (_neighboringUp == null)
                _neighboringUp = SetNeighboringBlockCalculations(Direction.up);
            return _neighboringUp;
        }
    }

    private static int[] NeighboringDown
    {
        get
        {
            if (_neighboringDown == null)
                _neighboringDown = SetNeighboringBlockCalculations(Direction.down);
            return _neighboringDown;
        }
    }

    public static Direction[] GetDirectionArray()
    {
        if (directions == null)
        {
            directions = new Direction[Enum.GetNames(typeof(Direction)).Length];

            for (int i = 0; i < Enum.GetNames(typeof(Direction)).Length; i++)
            {
                directions[i] = (Direction)i;
            }
        }

        return directions;
    }

    /// <summary>
    /// Returns the vertex calculation template for the supplied direction
    /// </summary>
    /// <param name="vertexDirection"></param>
    /// <returns></returns>
    public static int[][] GetVertexCalculationTemplate(Direction vertexDirection)
    {
        switch (vertexDirection)
        {
            case Direction.north:
                return VertexCalculationsNorth;
            case Direction.east:
                return VertexCalculationsEast;
            case Direction.south:
                return VertexCalculationsSouth;
            case Direction.west:
                return VertexCalculationsWest;
            case Direction.up:
                return VertexCalculationsUp;
            case Direction.down:
                return VertexCalculationsDown;
            default:
                throw new InvalidOperationException(string.Format("Direction {0} is not supported! Method: GetVertexCalculations", vertexDirection.ToString()));
        }
    }

    /// <summary>
    /// Get's the opposite direction from the one supplied
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Direction GetOppositeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.north:
                return Direction.south;
            case Direction.east:
                return Direction.west;
            case Direction.south:
                return Direction.north;
            case Direction.west:
                return Direction.east;
            case Direction.up:
                return Direction.down;
            case Direction.down:
                return Direction.up;
            default:
                throw new InvalidOperationException(string.Format("Direction {0} is not supported! Method: GetOppositeDirection", direction.ToString()));
        }
    }

    /// <summary>
    /// Gets the Neighbording block calculation template for a given direction
    /// </summary>
    /// <param name="blockDirection"></param>
    /// <returns></returns>
    public static int[] GetNeighboringBlockCalculationTemplate(Direction blockDirection)
    {
        switch (blockDirection)
        {
            case Direction.north:
                return NeighboringNorth;
            case Direction.east:
                return NeighboringEast;
            case Direction.south:
                return NeighboringSouth;
            case Direction.west:
                return NeighboringWest;
            case Direction.up:
                return NeighboringUp;
            case Direction.down:
                return NeighboringDown;
            default:
                throw new InvalidOperationException(string.Format("Direction {0} is not supported! Method: GetOppositeDirection", blockDirection.ToString()));
        }
    }

    /// <summary>
    /// Grabs the Calculation template for determining where the neighboring block is
    /// </summary>
    /// <param name="blockDirection"></param>
    /// <returns></returns>
    private static int[] SetNeighboringBlockCalculations(Direction blockDirection)
    {
        int[] neighboringCalculations;

        switch (blockDirection)
        {
            case Direction.north:
                neighboringCalculations = new int[3] { 0, 0, -1 };
                break;
            case Direction.east:
                neighboringCalculations = new int[3] { -1, 0, 0 };
                break;
            case Direction.south:
                neighboringCalculations = new int[3] { 0, 0, 1 };
                break;
            case Direction.west:
                neighboringCalculations = new int[3] { 1, 0, 0 };
                break;
            case Direction.up:
                neighboringCalculations = new int[3] { 0, -1, 0 };
                break;
            case Direction.down:
                neighboringCalculations = new int[3] { 0, 1, 0 };
                break;
            default:
                throw new InvalidOperationException(string.Format("Direction {0} is not supported! Method: SetNeighboringBlockCalculations", blockDirection.ToString()));
        }

        return neighboringCalculations;
    }

    /// <summary>
    /// Grabs the Calculation template for determining how to calculate the Vertex data
    /// </summary>
    /// <remarks>
    /// Template per direction is for a face of the block. Each face has four vertices and three coordinates.
    /// So the returned multidimensional int array has a length of four and each child array has a length of three
    /// </remarks>
    /// <param name="setDirection"></param>
    /// <returns></returns>
    private static int[][] SetVertexCalculations(Direction setDirection)
    {
        var vertexOperators = new int[4][];

        switch (setDirection)
        {
            case Direction.north:
                vertexOperators[0] = new int[] { 1, -1, 1 };
                vertexOperators[1] = new int[] { 1, 1, 1 };
                vertexOperators[2] = new int[] { -1, 1, 1 };
                vertexOperators[3] = new int[] { -1, -1, 1 };
                break;
            case Direction.east:
                vertexOperators[0] = new int[] { 1, -1, -1 };
                vertexOperators[1] = new int[] { 1, 1, -1 };
                vertexOperators[2] = new int[] { 1, 1, 1 };
                vertexOperators[3] = new int[] { 1, -1, 1 };
                break;
            case Direction.south:
                vertexOperators[0] = new int[] { -1, -1, -1 };
                vertexOperators[1] = new int[] { -1, 1, -1 };
                vertexOperators[2] = new int[] { 1, 1, -1 };
                vertexOperators[3] = new int[] { 1, -1, -1 };
                break;
            case Direction.west:
                vertexOperators[0] = new int[] { -1, -1, 1 };
                vertexOperators[1] = new int[] { -1, 1, 1 };
                vertexOperators[2] = new int[] { -1, 1, -1 };
                vertexOperators[3] = new int[] { -1, -1, -1 };
                break;
            case Direction.up:
                vertexOperators[0] = new int[] { -1, 1, 1 };
                vertexOperators[1] = new int[] { 1, 1, 1 };
                vertexOperators[2] = new int[] { 1, 1, -1 };
                vertexOperators[3] = new int[] { -1, 1, -1 };
                break;
            case Direction.down:
                vertexOperators[0] = new int[] { -1, -1, -1 };
                vertexOperators[1] = new int[] { 1, -1, -1 };
                vertexOperators[2] = new int[] { 1, -1, 1 };
                vertexOperators[3] = new int[] { -1, -1, 1 };
                break;
            default:
                vertexOperators[0] = new int[] { 1, 1, 1 };
                vertexOperators[1] = new int[] { 1, 1, 1 };
                vertexOperators[2] = new int[] { 1, 1, 1 };
                vertexOperators[3] = new int[] { 1, 1, 1 };
                break;
        }

        return vertexOperators;
    }
}
