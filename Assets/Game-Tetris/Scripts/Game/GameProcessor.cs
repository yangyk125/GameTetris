using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;

namespace GameTetris
{
    public class GameProcessor
    {
        private int _gameScore = 0;

        private Vector2Int _gameTileCount;
        private int[][] _gameTileMask = null;

        private Vector2Int _nextTileCount;
        private int[][] _nextTileMask = null;

        private TileBase _previewTile = null;

        private TileBase _fallingTile = null;
        private float _fallingSpeed = 1.0f;
        private Vector2Int _fallingOffset = Vector2Int.zero;
        private float _fallingLastTime = 0;
        

        private Vector2Int[] _tempTiles = new Vector2Int[4];
        private Vector2Int[] _tempVec2s = new Vector2Int[4];

        public const int STATIC_TILE_MASK_VALUE = 1;
        public const int FALLING_TILE_MASK_VALUE = 2;

        public GameProcessor(int[][] gameTileMask, Vector2Int gameTileCount, int[][] nextTileMask, Vector2Int nextTileCount)
        {
            _gameTileCount = gameTileCount;
            _gameTileMask = gameTileMask;

            _nextTileCount = nextTileCount;
            _nextTileMask = nextTileMask;
        }

        public int GetGameScore()
        {
            return _gameScore;
        }

        public bool FixedUpdate()
        {
            if (Time.time - _fallingLastTime > _fallingSpeed)
            {
                if (!ProcessFixedUpdateOnce())
                    return false;

                _fallingLastTime = Time.time;
            }

            return true;
        }

        public void RestartGame(float speed)
        {
            _gameScore = 0;
            _fallingSpeed = speed;

            _fallingLastTime = Time.time;

            _fallingTile = null;
            _previewTile = null;

            for (int idxV = 0; idxV < _gameTileCount.y; idxV++)
            {
                for (int idxH = 0; idxH < _gameTileCount.x; idxH++)
                {
                    _gameTileMask[idxV][idxH] = 0;
                }
            }

            CreateOrPushFallingTiles();
        }

        public void SetFallingSpeed(float speed)
        {
            _fallingSpeed = speed;
        }

        private bool CreateOrPushFallingTiles()
        {
            if (_previewTile != null)
                _fallingTile = _previewTile;
            else
                _fallingTile = CreateTileShapeRandom();

            _previewTile = CreateTileShapeRandom();
            ResetPreviewTilesStaticMask(_previewTile);

            _fallingOffset.Set(3, _fallingTile.GetBornOffsetVertical());
            _fallingTile.GetTilesWithOffset(_tempTiles, _fallingOffset);

            if (!CheckFallingTilesSafeAndFree(_tempTiles))
                return false; //game over

            SetGameTilesFallingMask(_fallingTile, _fallingOffset, true);
            return true;
        }

        private bool ProcessFixedUpdateOnce()
        {
            if (_fallingTile == null)
            {
                CreateOrPushFallingTiles();
            }

            //check game over
            _fallingTile.GetTilesWithOffset(_tempTiles, _fallingOffset);
            if (!CheckFallingTilesSafeAndFree(_tempTiles))
                return false; //game over

            //try fall down
            Vector2Int temp = _fallingOffset + new Vector2Int(0, 1);
            if (CheckCanMoveFalling(_fallingOffset, temp))
            {
                SetGameTilesFallingMask(_fallingTile, _fallingOffset, false);
                _fallingOffset = temp;
                SetGameTilesFallingMask(_fallingTile, _fallingOffset, true);
            }
            else
            {
                _fallingTile.GetTilesWithOffset(_tempTiles, _fallingOffset);

                SetGameTilesStaticMask(_tempTiles, true);
                ScanTilesAndComputeScore();

                if (!CreateOrPushFallingTiles())
                    return false; //game over
            }

            return true;
        }

        private TileBase CreateTileShapeRandom()
        {
            TileOrient orient = (TileOrient)Random.Range((int)TileOrient.TileToLeft, (int)TileOrient.TileToBottom);

            int index = Random.Range(1, 7);
            TileBase tileBase = index switch
            {
                1 => new TileShape1(orient),
                2 => new TileShape2(orient),
                3 => new TileShape3A(orient),
                4 => new TileShape3B(orient),
                5 => new TileShape4(orient),
                6 => new TileShape5A(orient),
                7 => new TileShape5B(orient),
                _ => new TileShape1(orient),
            };

            return tileBase;
        }

        public void DoInputActionUp()
        {
            if (_fallingTile == null)
                return;

            Vector2Int temp = _fallingOffset + new Vector2Int(0, -1);
            if (!CheckCanMoveFalling(_fallingOffset, temp))
                return;

            SetGameTilesFallingMask(_fallingTile, _fallingOffset, false);
            _fallingOffset = temp;
            SetGameTilesFallingMask(_fallingTile, _fallingOffset, true);
        }

        public void DoInputActionDown()
        {
            if (_fallingTile == null)
                return;

            Vector2Int temp = _fallingOffset + new Vector2Int(0, 1);
            if (!CheckCanMoveFalling(_fallingOffset, temp))
                return;

            SetGameTilesFallingMask(_fallingTile, _fallingOffset, false);
            _fallingOffset = temp;
            SetGameTilesFallingMask(_fallingTile, _fallingOffset, true);
        }

        public void DoInputActionLeft()
        {
            if (_fallingTile == null)
                return;

            Vector2Int temp = _fallingOffset + new Vector2Int(-1, 0);
            if (!CheckCanMoveFalling(_fallingOffset, temp))
                return;

            SetGameTilesFallingMask(_fallingTile, _fallingOffset, false);
            _fallingOffset = temp;
            SetGameTilesFallingMask(_fallingTile, _fallingOffset, true);
        }

        public void DoInputActionRight()
        {
            if (_fallingTile == null)
                return;

            Vector2Int temp = _fallingOffset + new Vector2Int(1, 0);
            if (!CheckCanMoveFalling(_fallingOffset, temp))
                return;

            SetGameTilesFallingMask(_fallingTile, _fallingOffset, false);
            _fallingOffset = temp;
            SetGameTilesFallingMask(_fallingTile, _fallingOffset, true);
        }

        public void DoInputActionRotateCCW()
        {
            if (_fallingTile == null)
                return;

            TileOrient oldOrient = _fallingTile.GetCurrentOrient();
            TileOrient newOrient = TileBase.RotateOrientationCCW(oldOrient);

            Vector2Int adjust = Vector2Int.zero;
            if (!CheckCanRotateFalling(oldOrient, newOrient, ref adjust))
                return;

            SetGameTilesFallingMask(_fallingTile, _fallingOffset, false);
            _fallingOffset += adjust;
            _fallingTile.SetCurrentOrient(newOrient);
            SetGameTilesFallingMask(_fallingTile, _fallingOffset, true);
        }

        public void DoInputActionRotateCW()
        {
            if (_fallingTile == null)
                return;

            TileOrient oldOrient = _fallingTile.GetCurrentOrient();
            TileOrient newOrient = TileBase.RotateOrientationCW(oldOrient);

            Vector2Int adjust = Vector2Int.zero;
            if (!CheckCanRotateFalling(oldOrient, newOrient, ref adjust))
                return;

            SetGameTilesFallingMask(_fallingTile, _fallingOffset, false);
            _fallingOffset += adjust;
            _fallingTile.SetCurrentOrient(newOrient);
            SetGameTilesFallingMask(_fallingTile, _fallingOffset, true);
        }

        public void DoInputActionFallToBottom()
        {
            Vector2Int adjust = Vector2Int.zero;
            for (int idx = 1; ; idx++)
            {
                if (!CheckCanMoveFalling(_fallingOffset, _fallingOffset + new Vector2Int(0, idx)))
                    break;

                adjust.Set(0, idx);
            }

            SetGameTilesFallingMask(_fallingTile, _fallingOffset, false);

            _fallingOffset += adjust;
            _fallingTile.GetTilesWithOffset(_tempTiles, _fallingOffset);
            SetGameTilesStaticMask(_tempTiles, true);
            ScanTilesAndComputeScore();

            CreateOrPushFallingTiles();

            _fallingLastTime = Time.time;
        }

        private void SetGameTilesFallingMask(TileBase tiles, Vector2Int offset, bool on)
        {
            if (tiles == null)
                return;

            tiles.GetTilesWithOffset(_tempTiles, offset);

            for (int idx = 0; idx < _tempTiles.Length; idx++)
            {
                if (0 <= _tempTiles[idx].y && _tempTiles[idx].y < _gameTileCount.y && 0 <= _tempTiles[idx].x && _tempTiles[idx].x < _gameTileCount.x)
                    _gameTileMask[_tempTiles[idx].y][_tempTiles[idx].x] = on ? FALLING_TILE_MASK_VALUE : 0;
            }
        }

        private void SetGameTilesStaticMask(Vector2Int[] tiles, bool on)
        {
            for (int idx = 0; idx < tiles.Length; idx++)
            {
                if (0 <= tiles[idx].y && tiles[idx].y < _gameTileCount.y && 0 <= tiles[idx].x && tiles[idx].x < _gameTileCount.x)
                    _gameTileMask[tiles[idx].y][tiles[idx].x] = on ? STATIC_TILE_MASK_VALUE : 0;
            }
        }

        private void ResetPreviewTilesStaticMask(TileBase tiles)
        {
            if (tiles == null)
                return;

            for (int idx = 0; idx < _nextTileCount.x; idx++)
            {
                for (int idy = 0; idy < _nextTileCount.y; idy++)
                {
                    _nextTileMask[idy][idx] = 0;
                }
            }

            tiles.GetTilesWithOffset(_tempTiles, Vector2Int.zero);

            for (int idx = 0; idx < _tempTiles.Length; idx++)
            {
                if (0 <= _tempTiles[idx].y && _tempTiles[idx].y < _nextTileCount.y && 0 <= _tempTiles[idx].x && _tempTiles[idx].x < _nextTileCount.x)
                    _nextTileMask[_tempTiles[idx].y][_tempTiles[idx].x] = STATIC_TILE_MASK_VALUE;
            }
        }


        private bool CheckFallingTilesSafeAndFree(Vector2Int[] tiles)
        {
            for (int idx = 0; idx < tiles.Length; idx++)
            {
                if (tiles[idx].x < 0 || tiles[idx].y < 0)
                    return false;

                if (tiles[idx].x >= _gameTileCount.x || tiles[idx].y >= _gameTileCount.y)
                    return false;

                if (0 <= tiles[idx].x && tiles[idx].x < _gameTileCount.x && 0 <= tiles[idx].y && tiles[idx].y < _gameTileCount.y)
                {
                    if (_gameTileMask[tiles[idx].y][tiles[idx].x] == STATIC_TILE_MASK_VALUE)
                        return false;
                }
            }

            return true;
        }

        private bool CheckCanMoveFalling(Vector2Int oldOffset, Vector2Int newOffset)
        {
            if (_fallingTile == null)
                return false;

            _fallingTile.GetTilesWithOffset(_tempTiles, newOffset);

            if (!CheckFallingTilesSafeAndFree(_tempTiles))
            {
                return false;
            }

            return true;
        }

        private bool CheckCanRotateFalling(TileOrient oldOrient, TileOrient newOrient, ref Vector2Int adjust)
        {
            if (_fallingTile == null)
                return false;

            _fallingTile.GetTilesWithOffset(_tempTiles, _fallingOffset, newOrient);

            bool baseCheck = CheckFallingTilesSafeAndFree(_tempTiles);
            if (!baseCheck)
            {
                const int adjustMax = 2;

                for (int idx = 1; idx <= adjustMax; idx++)
                {
                    _tempVec2s[0].Set(-idx, 0);
                    _tempVec2s[1].Set(idx, 0);
                    _tempVec2s[2].Set(0, -idx);
                    _tempVec2s[3].Set(0, idx);

                    for (int at = 0; at < _tempVec2s.Length; at++)
                    {
                        _fallingTile.GetTilesWithOffset(_tempTiles, _fallingOffset + _tempVec2s[at], newOrient);
                        if (CheckFallingTilesSafeAndFree(_tempTiles))
                        {
                            adjust = _tempVec2s[at];
                            return true;
                        }
                    }
                }
            }

            adjust = Vector2Int.zero;
            return true;
        }

        private void ScanTilesAndComputeScore()
        {
            int ScoreLines = 0;

            for (int idxV = _gameTileCount.y-1; idxV >=0;)
            {
                bool allOn = true;

                for (int idxH = 0; idxH < _gameTileCount.x; idxH++)
                {
                    if (_gameTileMask[idxV][idxH] == 0)
                    {
                        allOn = false;
                        break;
                    }
                }

                if (allOn)
                {
                    for (int v2 = idxV - 1; v2 >= 0; v2--)
                    {
                        for (int h2 = 0; h2 < _gameTileCount.x; h2++)
                        {
                            _gameTileMask[v2+1][h2] = _gameTileMask[v2][h2];
                        }
                    }

                    ScoreLines++;
                }
                else
                {
                    idxV--;
                }
            }

            _gameScore += ScoreLines switch
            {
                1 => 100,
                2 => 300,
                3 => 600,
                4 => 1000,
                0 => 0,
                _ => 0,
            };
        }
    }
}

