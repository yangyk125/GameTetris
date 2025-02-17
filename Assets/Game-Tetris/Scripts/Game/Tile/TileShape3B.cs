using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameTetris
{

    /**
     * ÓÒ³úÍ·
     */
    public class TileShape3B : TileBase
    {
        public TileShape3B(TileOrient orient)
        {
            _currentOrient = orient;

            //top
            /**
             * 1 0 0 0
             * 1 1 1 0
             * 0 0 0 0
             * 0 0 0 0
             */
            Vector2Int[] tiles4x4 = _orientToTiles4x4[TileOrient.TileToTop];
            tiles4x4[0].Set(0, 0);
            tiles4x4[1].Set(0, 1);
            tiles4x4[2].Set(1, 1);
            tiles4x4[3].Set(2, 1);

            //left
            /**
             * 0 1 0 0
             * 0 1 0 0
             * 1 1 0 0
             * 0 0 0 0
             */
            tiles4x4 = _orientToTiles4x4[TileOrient.TileToLeft];
            tiles4x4[0].Set(1, 0);
            tiles4x4[1].Set(1, 1);
            tiles4x4[2].Set(1, 2);
            tiles4x4[3].Set(0, 2);

            //bottom
            /**
             * 0 0 0 0
             * 1 1 1 0
             * 0 0 1 0
             * 0 0 0 0
             */
            tiles4x4 = _orientToTiles4x4[TileOrient.TileToBottom];
            tiles4x4[0].Set(0, 1);
            tiles4x4[1].Set(1, 1);
            tiles4x4[2].Set(2, 1);
            tiles4x4[3].Set(2, 2);

            //right
            /**
             * 0 1 1 0
             * 0 1 0 0
             * 0 1 0 0
             * 0 0 0 0
             */
            tiles4x4 = _orientToTiles4x4[TileOrient.TileToRight];
            tiles4x4[0].Set(1, 0);
            tiles4x4[1].Set(1, 1);
            tiles4x4[2].Set(1, 2);
            tiles4x4[3].Set(2, 0);
        }

        public override int GetBornOffsetVertical()
        {
            return _currentOrient switch
            {
                TileOrient.TileToTop => 0,
                TileOrient.TileToLeft => 0,
                TileOrient.TileToBottom => -1,
                TileOrient.TileToRight => 0,
                _ => 0
            };
        }
    }

}
