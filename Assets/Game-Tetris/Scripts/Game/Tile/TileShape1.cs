using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameTetris
{

    /**
     * ¹÷¹÷:
     * 0 1 0 0
     * 0 1 0 0
     * 0 1 0 0
     * 0 1 0 0
     */
    public class TileShape1 : TileBase
    {
        public TileShape1(TileOrient orient)
        {
            _currentOrient = orient;

            //top
            Vector2Int[] tiles4x4 = _orientToTiles4x4[TileOrient.TileToTop];
            tiles4x4[0].Set(1, 0);
            tiles4x4[1].Set(1, 1);
            tiles4x4[2].Set(1, 2);
            tiles4x4[3].Set(1, 3);

            //left
            tiles4x4 = _orientToTiles4x4[TileOrient.TileToLeft];
            tiles4x4[0].Set(0, 1);
            tiles4x4[1].Set(1, 1);
            tiles4x4[2].Set(2, 1);
            tiles4x4[3].Set(3, 1);

            //bottom
            tiles4x4 = _orientToTiles4x4[TileOrient.TileToBottom];
            tiles4x4[0].Set(1, 0);
            tiles4x4[1].Set(1, 1);
            tiles4x4[2].Set(1, 2);
            tiles4x4[3].Set(1, 3);

            //right
            tiles4x4 = _orientToTiles4x4[TileOrient.TileToRight];
            tiles4x4[0].Set(0, 1);
            tiles4x4[1].Set(1, 1);
            tiles4x4[2].Set(2, 1);
            tiles4x4[3].Set(3, 1);
        }

        public override int GetBornOffsetVertical()
        {
            return _currentOrient switch
            {
                TileOrient.TileToTop => 0,
                TileOrient.TileToLeft => -1,
                TileOrient.TileToBottom => 0,
                TileOrient.TileToRight => -1,
                _ => 0
            };
        }
    }

}
