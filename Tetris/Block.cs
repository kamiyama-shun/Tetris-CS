using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Block
    {
        public Point[,] point;
        public int color;   // [0：黒][1：水][2：黄][3：青][4：赤][5：橙][6：緑][7：紫]

        public Block(int pattern, int color, Point[] loc)
        {
            this.point = new Point[pattern, loc.Length];

            for (int ptnNo = 0; ptnNo < pattern; ++ptnNo)
            {
                // 座標の登録
                for (int blkNo = 0; blkNo < loc.Length; ++blkNo)
                {
                    this.point[ptnNo, blkNo] = loc[blkNo];
                }

                // 次に設定する座標の算出
                for (int blkNo = 0; blkNo < loc.Length; ++blkNo)
                {
                    loc[blkNo] = this.RotatePoint(loc[blkNo]);
                }
            }

            // 色の設定
            this.color = color;
        }

        /// <summary>
        /// 座標変換
        /// </summary>
        /// <param name="point">現在の座標</param>
        /// <returns>変換後の座標</returns>
        private Point RotatePoint(Point point)
        {
            return new Point(point.Y, -point.X);
        }
    }
}
