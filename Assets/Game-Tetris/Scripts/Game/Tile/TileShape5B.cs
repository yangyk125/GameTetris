using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameTetris
{

    /**
     * 平行四边形B
     */
    public class TileShape5B : TileBase
    {
        public TileShape5B(TileOrient orient)
        {
            _currentOrient = orient;

            //top
            /**
             * 1 0 0 0
             * 1 1 0 0
             * 0 1 0 0
             * 0 0 0 0
             */
            Vector2Int[] tiles4x4 = _orientToTiles4x4[TileOrient.TileToTop];
            tiles4x4[0].Set(0, 0);
            tiles4x4[1].Set(0, 1);
            tiles4x4[2].Set(1, 1);
            tiles4x4[3].Set(1, 2);

            //left
            /**
             * 0 1 1 0
             * 1 1 0 0
             * 0 0 0 0
             * 0 0 0 0
             */
            tiles4x4 = _orientToTiles4x4[TileOrient.TileToLeft];
            tiles4x4[0].Set(0, 1);
            tiles4x4[1].Set(1, 1);
            tiles4x4[2].Set(1, 0);
            tiles4x4[3].Set(2, 0);

            //bottom
            /**
             * 1 0 0 0
             * 1 1 0 0
             * 0 1 0 0
             * 0 0 0 0
             */
            tiles4x4 = _orientToTiles4x4[TileOrient.TileToBottom];
            tiles4x4[0].Set(0, 0);
            tiles4x4[1].Set(0, 1);
            tiles4x4[2].Set(1, 1);
            tiles4x4[3].Set(1, 2);

            //right
            /**
             * 0 1 1 0
             * 1 1 0 0
             * 0 0 0 0
             * 0 0 0 0
             */
            tiles4x4 = _orientToTiles4x4[TileOrient.TileToRight];
            tiles4x4[0].Set(0, 1);
            tiles4x4[1].Set(1, 1);
            tiles4x4[2].Set(1, 0);
            tiles4x4[3].Set(2, 0);
        }

        public override int GetBornOffsetVertical()
        {
            return _currentOrient switch
            {
                TileOrient.TileToTop => 0,
                TileOrient.TileToLeft => 0,
                TileOrient.TileToBottom => 0,
                TileOrient.TileToRight => 0,
                _ => 0
            };
        }
    }

}
