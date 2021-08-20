using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinder : MonoBehaviour
{
    public enum Direction
    {
        North,
        South,
        East,
        West,
        NorthEast,
        NorthWest,
        SouthEast,
        SouthWest
    }

    private class TileDetail
    {
        public Vector3Int? parent = null;
        public float g = 0.0f;
        public float h = 0.0f;
        public bool opened = false;
        public bool closed = false;
    }

    public Tilemap tilemap;
    public float straightenStrictness = 0.1f;
    public bool straightenPath;

    private Dictionary<Vector3Int, TileDetail> _tileDetails;

    public bool isPositionValid(Vector2 destination)
    {
        return IsTileValid(new Vector3Int((int)destination.x, (int)destination.y, 0));
    }

    public List<Vector2> GetPath(Vector2 end)
    {
        ResetTileDetails();

        List<Vector2> path = new List<Vector2>();
        List<Vector3Int> openList = new List<Vector3Int>();
        List<Vector3Int> closedList = new List<Vector3Int>();

        var source = tilemap.WorldToCell(transform.position);
        var desintation = tilemap.WorldToCell(end.ToVector3());

        if (!IsTileValid(source))
            return new List<Vector2>();
        if (!IsTileValid(desintation))
            return new List<Vector2>();

        openList.Add(source);
        _tileDetails[source].opened = true;
        _tileDetails[source].g = 0.0f;

        bool found = false;
        while(!found && openList.Count > 0)
        {
            var nextTile = openList[0];
            openList.RemoveAt(0);

            if (nextTile == desintation)
            {
                found = true;
            }
            else
            {
                List<Vector3Int> neighbors = GetNeighbors(nextTile);
                foreach(var n in neighbors)
                {
                    if (IsTileValid(n))
                    {
                        if (!_tileDetails[n].opened)
                        {
                            _tileDetails[n].parent = nextTile;
                            _tileDetails[n].opened = true;
                            _tileDetails[n].g = _tileDetails[nextTile].g + GCost(nextTile, n);
                            _tileDetails[n].h = HCost(n, desintation);
                            SortedInsert(openList, n);
                        }
                        else if (!_tileDetails[n].closed)
                        {
                            float g = _tileDetails[nextTile].g + GCost(nextTile, n);
                            if (g < _tileDetails[n].g)
                            {
                                _tileDetails[n].parent = nextTile;
                                _tileDetails[n].g = g;
                                openList.Remove(n);
                                SortedInsert(openList, n);
                            }
                        }                       
                    }
                }
            }

            _tileDetails[nextTile].closed = true;
            closedList.Add(nextTile);
        }

        var tile = desintation;
        while (true)
        {
            path.Add(new Vector2(tile.x + 0.5f, tile.y + 0.5f));

            if (_tileDetails[tile].parent == null)
                break;

            tile = (Vector3Int)_tileDetails[tile].parent;
        }

        path.Reverse();

        if (straightenPath)
            path = StraightenPath(path);

        path.RemoveAt(0);
        return path;
    }

    private void Awake()
    {
        tilemap.CompressBounds();
        _tileDetails = new Dictionary<Vector3Int, TileDetail>();

        foreach (var tile in tilemap.cellBounds.allPositionsWithin)
        {
            if (IsTileValid(tile))
            {
                _tileDetails[tile] = new TileDetail();
            }
        }
    }

    private List<Vector2> StraightenPath(List<Vector2> path)
    {
        if (path.Count == 0)
            return null;

        List<Vector2> straigtenedPath = new List<Vector2>();

        int prev = 0;
        int curr = 1;

        straigtenedPath.Add(path[0]);
        while (curr < path.Count)
        {
            Vector2 prevToCurr = path[curr] - path[prev];
            RaycastHit2D hit = Physics2D.CircleCast(path[prev], 0.25f, prevToCurr.normalized, prevToCurr.magnitude, LayerMask.GetMask("Walls"));
            if (hit.collider != null)
            {
                prev = curr - 1;
                straigtenedPath.Add(path[prev]);
            }
            else
            {
                ++curr;
            }
        }
        straigtenedPath.Add(path[path.Count - 1]);

        return straigtenedPath;
    }

    private List<Vector3Int> GetNeighbors(Vector3Int tile)
    {
        Vector3Int[] neighbors = new Vector3Int[8];

        neighbors[(int)Direction.North] = new Vector3Int(tile.x, tile.y + 1, tile.z);
        neighbors[(int)Direction.South] = new Vector3Int(tile.x, tile.y - 1, tile.z);
        neighbors[(int)Direction.East] = new Vector3Int(tile.x + 1, tile.y, tile.z);
        neighbors[(int)Direction.West] = new Vector3Int(tile.x - 1, tile.y, tile.z);
        neighbors[(int)Direction.NorthEast] = new Vector3Int(tile.x + 1, tile.y + 1, tile.z);
        neighbors[(int)Direction.NorthWest] = new Vector3Int(tile.x - 1, tile.y + 1, tile.z);
        neighbors[(int)Direction.SouthEast] = new Vector3Int(tile.x + 1, tile.y - 1, tile.z);
        neighbors[(int)Direction.SouthWest] = new Vector3Int(tile.x - 1, tile.y - 1, tile.z);

        List<Vector3Int> validNeighbors = new List<Vector3Int>();
        bool northValid = IsTileValid(neighbors[(int)Direction.North]);
        bool southValid = IsTileValid(neighbors[(int)Direction.South]);
        bool eastValid = IsTileValid(neighbors[(int)Direction.East]);
        bool westValid = IsTileValid(neighbors[(int)Direction.West]);

        if (northValid)
            validNeighbors.Add(neighbors[(int)Direction.North]);
        if (southValid)
            validNeighbors.Add(neighbors[(int)Direction.South]);
        if (eastValid)
            validNeighbors.Add(neighbors[(int)Direction.East]);
        if (westValid)
            validNeighbors.Add(neighbors[(int)Direction.West]);

        if (IsTileValid(neighbors[(int)Direction.NorthEast]) && northValid && eastValid)
            validNeighbors.Add(neighbors[(int)Direction.NorthEast]);
        if (IsTileValid(neighbors[(int)Direction.NorthWest]) && northValid && westValid)
            validNeighbors.Add(neighbors[(int)Direction.NorthWest]);
        if (IsTileValid(neighbors[(int)Direction.SouthEast]) && southValid && eastValid)
            validNeighbors.Add(neighbors[(int)Direction.SouthEast]);
        if (IsTileValid(neighbors[(int)Direction.SouthWest]) && southValid && westValid)
            validNeighbors.Add(neighbors[(int)Direction.SouthWest]);

        return validNeighbors;
    }

    private float GCost(Vector3Int src, Vector3Int dest)
    {
        return Vector3Int.Distance(src, dest);
    }

    private float HCost(Vector3Int src, Vector3Int dest)
    {
        float dx = Mathf.Abs(src.x - dest.x);
        float dy = Mathf.Abs(src.y - dest.y);
        return dx * dy;
    }

    private bool IsTileValid(Vector3Int tile)
    {
        return tilemap.HasTile(tile);
    }

    private void SortedInsert(List<Vector3Int> list, Vector3Int tile)
    {
        for(int i = 0; i < list.Count; ++i)
        {
            if (_tileDetails[list[i]].g + _tileDetails[list[i]].h > _tileDetails[tile].g + _tileDetails[tile].h)
            {
                list.Insert(i, tile);
                return;
            }
        }

        list.Add(tile);
    }

    private void ResetTileDetails()
    {
        foreach (var tile in _tileDetails)
        {
            var detail = tile.Value;
            detail.parent = null;
            detail.g = 0.0f;
            detail.h = 0.0f;
            detail.opened = false;
            detail.closed = false;
        }
    }
}