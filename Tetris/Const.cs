using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public static class Const
    {
        private static int blockSize = 20;  // ブロックサイズ
        private static int nColor = 8;      // ブロックの色数
        private static int width = 10;      // 横に配置できるブロック数
        private static int height = 20;     // 縦に配置できるブロック数

        public static int BlockSize
        {
            get { return Const.blockSize; }
        }

        public static int NColor
        {
            get { return Const.nColor; }
        }

        public static int Width
        {
            get { return Const.width; }
        }

        public static int Height
        {
            get { return Const.height; }
        }

    }
}
