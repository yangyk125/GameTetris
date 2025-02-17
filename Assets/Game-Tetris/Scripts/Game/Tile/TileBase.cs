using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameTetris
{
    public enum TileOrient
    {
        TileToLeft = 0,
        TileToRight = 1,
        TileToTop = 2,
        TileToBottom = 3
    }

    public abstract class TileBase
    {
        protected Dictionary<TileOrient, Vector2Int[]> _orientToTiles4x4 = new();
        protected TileOrient _currentOrient = TileOrient.TileToLeft;

        public static TileOrient RotateOrientationCW(TileOrient oldOrient)
        {
            TileOrient _newOrient = TileOrient.TileToLeft;

            switch (oldOrient)
            {
                case TileOrient.TileToTop:
                    _newOrient = TileOrient.TileToRight;
                    break;
                case TileOrient.TileToLeft:
                    _newOrient = TileOrient.TileToTop;
                    break;
                case TileOrient.TileToBottom:
                    _newOrient = TileOrient.TileToLeft;
                    break;
                case TileOrient.TileToRight:
                    _newOrient = TileOrient.TileToBottom;
                    break;
            }

            return _newOrient;
        }

        public static TileOrient RotateOrientationCCW(TileOrient oldOrient)
        {
            TileOrient _newOrient = TileOrient.TileToLeft;

            switch (oldOrient)
            {
                case TileOrient.TileToTop:
                    _newOrient = TileOrient.TileToLeft;
                    break;
                case TileOrient.TileToLeft:
                    _newOrient = TileOrient.TileToBottom;
                    break;
                case TileOrient.TileToBottom:
                    _newOrient = TileOrient.TileToRight;
                    break;
                case TileOrient.TileToRight:
                    _newOrient = TileOrient.TileToTop;
                    break;
            }

            return _newOrient;
        }

        public TileBase()
        {
            _orientToTiles4x4.Add(TileOrient.TileToLeft, new Vector2Int[4]);
            _orientToTiles4x4.Add(TileOrient.TileToRight, new Vector2Int[4]);
            _orientToTiles4x4.Add(TileOrient.TileToTop, new Vector2Int[4]);
            _orientToTiles4x4.Add(TileOrient.TileToBottom, new Vector2Int[4]);
        }

        public void GetTilesWithOffset(Vector2Int[] tiles, Vector2Int offset)
        {
            Vector2Int[] tiles4x4 = _orientToTiles4x4[_currentOrient];
            tiles[0] = tiles4x4[0] + offset;
            tiles[1] = tiles4x4[1] + offset;
            tiles[2] = tiles4x4[2] + offset;
            tiles[3] = tiles4x4[3] + offset;
        }

        public void GetTilesWithOffset(Vector2Int[] tiles, Vector2Int offset, TileOrient orient)
        {
            Vector2Int[] tiles4x4 = _orientToTiles4x4[orient];
            tiles[0] = tiles4x4[0] + offset;
            tiles[1] = tiles4x4[1] + offset;
            tiles[2] = tiles4x4[2] + offset;
            tiles[3] = tiles4x4[3] + offset;
        }

        public Vector2Int[] GetTilesBase4X4()
        {
            Vector2Int[] tiles4x4 = _orientToTiles4x4[_currentOrient];
            return tiles4x4;
        }

        public Vector2Int[] GetTilesBase4X4(TileOrient orient)
        {
            Vector2Int[] tiles4x4 = _orientToTiles4x4[orient];
            return tiles4x4;
        }

        public TileOrient GetCurrentOrient()
        {
            return _currentOrient;
        }

        public void SetCurrentOrient(TileOrient orient)
        {
            _currentOrient = orient;
        }

        public abstract int GetBornOffsetVertical();

    }
}
    
