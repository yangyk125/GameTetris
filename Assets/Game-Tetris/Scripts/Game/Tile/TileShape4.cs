using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameTetris
{

    /**
     * ·½ÐÎ
     */
    public class TileShape4 : TileBase
    {
        public TileShape4(TileOrient orient)
        {
            _currentOrient = orient;

            //top
            /**
             * 0 0 0 0
             * 0 1 1 0
             * 0 1 1 0
             * 0 0 0 0
             */
            Vector2Int[] tiles4x4 = _orientToTiles4x4[TileOrient.TileToTop];
            tiles4x4[0].Set(1, 1);
            tiles4x4[1].Set(2, 1);
            tiles4x4[2].Set(1, 2);
            tiles4x4[3].Set(2, 2);

            //left
            /**
             * 0 0 0 0
             * 0 1 1 0
             * 0 1 1 0
             * 0 0 0 0
             */
            tiles4x4 = _orientToTiles4x4[TileOrient.TileToLeft];
            tiles4x4[0].Set(1, 1);
            tiles4x4[1].Set(2, 1);
            tiles4x4[2].Set(1, 2);
            tiles4x4[3].Set(2, 2);

            //bottom
            /**
             * 0 0 0 0
             * 0 1 1 0
             * 0 1 1 0
             * 0 0 0 0
             */
            tiles4x4 = _orientToTiles4x4[TileOrient.TileToBottom];
            tiles4x4[0].Set(1, 1);
            tiles4x4[1].Set(2, 1);
            tiles4x4[2].Set(1, 2);
            tiles4x4[3].Set(2, 2);

            //right
            /**
             * 0 0 0 0
             * 0 1 1 0
             * 0 1 1 0
             * 0 0 0 0
             */
            tiles4x4 = _orientToTiles4x4[TileOrient.TileToRight];
            tiles4x4[0].Set(1, 1);
            tiles4x4[1].Set(2, 1);
            tiles4x4[2].Set(1, 2);
            tiles4x4[3].Set(2, 2);
        }

        public override int GetBornOffsetVertical()
        {
            return _currentOrient switch
            {
                TileOrient.TileToTop => -1,
                TileOrient.TileToLeft => -1,
                TileOrient.TileToBottom => -1,
                TileOrient.TileToRight => -1,
                _ => 0
            };
        }
    }

}
