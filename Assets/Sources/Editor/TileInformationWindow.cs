using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Sources.Editor
{
    public class TileInformationWindow : EditorWindow
    {
        private WorldChanger _worldChanger;
        private IReadOnlyList<Tile> _tiles;
        private string _info ="info";

        [MenuItem("Window/Tiles information")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(TileInformationWindow));
        }

        private void Update()
        {
            if (_worldChanger != null)
                return;

            var target = FindObjectsByType<WorldGenerator>(FindObjectsSortMode.None).FirstOrDefault();

            if (target == null || target.WorldChanger == null)
                return;

            if (target.WorldChanger.Tiles == null || target.WorldChanger.Tiles.Count == 0)
                return;

            _worldChanger = target.WorldChanger;
            _tiles = target.WorldChanger.Tiles;

            target.WorldChanger.UpdatedInspect += OnUpdated;
            target.WorldChanger.TilesChanged += TileChanged;

            TileChanged();
        }


        private void OnDisable()
        {
            if (_worldChanger != null)
            {
                _worldChanger.UpdatedInspect -= OnUpdated;
                _worldChanger.TilesChanged -= TileChanged;
                _worldChanger = null;
            }
        }

        void OnGUI()
        {
            bool x = EditorGUILayout.LinkButton("", new GUILayoutOption[] {GUILayout.Height(48), GUILayout.Width(48)});

            if (x && _worldChanger != null)
            {
                _worldChanger.UpdatedInspect -= OnUpdated;
                _worldChanger.TilesChanged -= TileChanged;
                _worldChanger = null;
            }

            GUIStyle style = new()
            {
                fixedHeight = 1,
            };

            EditorGUILayout.LabelField(_info, style);
        }

        private void TileChanged()
        {
            _info = "";

            for (int i = 3; i >= 0; i--)
            {
                for (int j = 0; j < 4; j++)
                {
                    Tile tile = _worldChanger.GetTile(new Vector2Int(j, i));
                    _info += $"({j} {i}) {tile.Type}";

                    if (tile is RoadTile roadTile)
                    {
                        _info += $"{roadTile.Ground.Type.ToString()} {roadTile.Ground.RoadType.ToString()} (inspect count {roadTile.InspectCount})";
                    }

                    _info += " | ";
                }

                _info += "\n";
            }
        }


        private void OnUpdated()
        {
            foreach(var tile in _worldChanger.Tiles)
            {
                if(tile is RoadTile roadTile)
                {
                    roadTile.InspectCount = 0;
                }
            }
        }
    }
}