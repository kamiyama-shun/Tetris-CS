using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    static class BlockManager
    {

        static Block[] blocks;

        static BlockManager()
        {
            BlockManager.blocks = new Block[] {

                // 縦棒
                new Block(2, 7, new Point[] {
                    new Point(-2, 0),
                    new Point(-1, 0),
                    new Point(0, 0), 
                    new Point(1, 0),
                    new Point(2, 0)
                }),

                // 四角
                new Block(1, 6, new Point[] {
                    new Point(0, -1),
                    new Point(1, -1), 
                    new Point(0, 0),
                    new Point(1, 0)
                }),

                // L
                new Block(4, 5, new Point[] {
                    new Point(-1, 0),
                    new Point(0, 0),
                    new Point(1, 0),
                    new Point(1, -1) 
                }),

                // カギ
                new Block(2, 4, new Point[] {
                    new Point(-1, 0),
                    new Point(0, 0),
                    new Point(0, 1),
                    new Point(1, 1) 
                }),

                // 逆L
                new Block(4, 3, new Point[] {
                    new Point(-1, -1),
                    new Point(-1, 0),
                    new Point(0, 0),
                    new Point(1, 0) 
                }),

                // 逆カギ
                new Block(2, 2, new Point[] {
                    new Point(-1, 1),
                    new Point(0, 1),
                    new Point(0, 0),
                    new Point(1, 0) 
                }),

                // 凸
                new Block(4, 1, new Point[] {
                    new Point(-1, 0),
                    new Point(0, 0),
                    new Point(1, 0),
                    new Point(0, -1) 
                })
            };
        }

        /// <summary>
        /// ブロックを取得
        /// </summary>
        /// <returns>ブロック</returns>
        public static Block getBlock()
        {
            return blocks[(new Random()).Next(0, blocks.Length)];
        }
    }
}
