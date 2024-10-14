using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure
{
    public interface ITileGetable
    {
        Tile GetTile(Vector2Int gridPosition);
        IEnumerable<int> GetLineNeighbors(int linePosition);
    }
}