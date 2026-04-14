using System.Collections.Generic;
using UnityEngine;

public class SimplePathfinder
{
    private class Node
    {
        public Vector2Int gridPos;
        public Vector2Int parent;
        public float gCost;
        public float hCost;
        public float fCost => gCost + hCost;

        public Node(Vector2Int pos, Vector2Int parent, float g, float h)
        {
            gridPos = pos;
            this.parent = parent;
            gCost = g;
            hCost = h;
        }
    }

    private float cellSize;
    private LayerMask obstacleLayer;
    private float agentRadius;

    public SimplePathfinder(float cellSize, LayerMask obstacleLayer, float agentRadius = 0.3f)
    {
        this.cellSize = cellSize;
        this.obstacleLayer = obstacleLayer;
        this.agentRadius = agentRadius;
    }

    private Vector2Int WorldToGrid(Vector2 worldPos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPos.x / cellSize),
            Mathf.RoundToInt(worldPos.y / cellSize)
        );
    }

    private Vector2 GridToWorld(Vector2Int gridPos)
    {
        return new Vector2(gridPos.x * cellSize, gridPos.y * cellSize);
    }

    private bool IsCellBlocked(Vector2Int cell)
    {
        Vector2 worldPos = GridToWorld(cell);
        return Physics2D.OverlapCircle(worldPos, agentRadius, obstacleLayer) != null;
    }

    public List<Vector2> FindPath(Vector2 startWorld, Vector2 endWorld)
    {
        Vector2Int startCell = WorldToGrid(startWorld);
        Vector2Int endCell = WorldToGrid(endWorld);

        if (startCell == endCell) return null;
        if (IsCellBlocked(endCell)) return null;

        Vector2Int[] directions = {
            new Vector2Int( 0,  1),
            new Vector2Int( 0, -1),
            new Vector2Int( 1,  0),
            new Vector2Int(-1,  0),
            new Vector2Int( 1,  1),
            new Vector2Int( 1, -1),
            new Vector2Int(-1,  1),
            new Vector2Int(-1, -1)
        };

        // CORRECCIÓN: Usamos un solo diccionario para todos los nodos visitados
        Dictionary<Vector2Int, Node> allNodes = new Dictionary<Vector2Int, Node>();
        HashSet<Vector2Int> openSet = new HashSet<Vector2Int>();
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

        Node startNode = new Node(startCell, startCell, 0f,
            Vector2Int.Distance(startCell, endCell));
        allNodes[startCell] = startNode;
        openSet.Add(startCell);

        int maxIterations = 1000;
        int iterations = 0;

        while (openSet.Count > 0 && iterations < maxIterations)
        {
            iterations++;

            // Nodo con menor fCost del openSet
            Vector2Int currentPos = GetLowestFCost(openSet, allNodes);
            Node current = allNodes[currentPos];

            if (currentPos == endCell)
            {
                Debug.Log($"[Pathfinder] Camino encontrado en {iterations} iteraciones");
                return ReconstructPath(allNodes, endCell, endWorld);
            }

            openSet.Remove(currentPos);
            closedSet.Add(currentPos);

            foreach (Vector2Int dir in directions)
            {
                Vector2Int neighborPos = current.gridPos + dir;

                if (closedSet.Contains(neighborPos)) continue;
                if (IsCellBlocked(neighborPos)) continue;

                float moveCost = (dir.x != 0 && dir.y != 0) ? 1.414f : 1f;
                float newG = current.gCost + moveCost;

                if (!allNodes.ContainsKey(neighborPos) || newG < allNodes[neighborPos].gCost)
                {
                    allNodes[neighborPos] = new Node(
                        neighborPos,
                        currentPos, // <-- guardamos la celda padre correctamente
                        newG,
                        Vector2Int.Distance(neighborPos, endCell)
                    );

                    openSet.Add(neighborPos);
                }
            }
        }

        Debug.LogWarning($"[Pathfinder] No se encontró camino tras {iterations} iteraciones");
        return null;
    }

    private Vector2Int GetLowestFCost(HashSet<Vector2Int> openSet, Dictionary<Vector2Int, Node> allNodes)
    {
        Vector2Int best = default;
        float bestF = float.MaxValue;

        foreach (Vector2Int pos in openSet)
        {
            float f = allNodes[pos].fCost;
            if (f < bestF)
            {
                bestF = f;
                best = pos;
            }
        }
        return best;
    }

    private List<Vector2> ReconstructPath(Dictionary<Vector2Int, Node> allNodes, Vector2Int endCell, Vector2 exactEndPos)
    {
        List<Vector2> path = new List<Vector2>();
        Vector2Int current = endCell;

        int safety = 0;
        while (safety < 2000)
        {
            safety++;
            path.Add(GridToWorld(current));

            Node node = allNodes[current];

            // Llegamos al inicio (el padre es él mismo)
            if (node.parent == current) break;

            current = node.parent;
        }

        // Sustituir el primer punto por la posición exacta del jugador
        if (path.Count > 0)
            path[0] = exactEndPos;

        path.Reverse();

        Debug.Log($"[Pathfinder] Path reconstruido con {path.Count} waypoints");
        return path;
    }
}