using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        // テトリスブロック画像
        Image mImg;

        // テトリスブロック色別リスト
        Rectangle[] mBlock = new Rectangle[Const.NColor];

        // スクリーン上のブロック色リスト
        int[,] screen = new int[Const.Width, Const.Height];

        // ブロック出現位置
        readonly Point mStartPos = new Point(4, 0);

        Block currBlock;
        Point pos;
        int rot;

        int fallCnt = 0;

        public Form1()
        {
            InitializeComponent();

            // ブロック画像設定
            mImg = Properties.Resources.tetris;

            // ブロック画像を色別に分割して設定
            for (int i = 0; i < Const.NColor; ++i)
            {
                // テトリスブロック画像の切り抜き（始点座標X, 始点座標Y, 幅, 高さ) 
                mBlock[i] = new Rectangle(i * Const.BlockSize, 0, Const.BlockSize, Const.BlockSize);
            }

            // 初期ブロック配置
            PutStartBlock();

            int x, y;
            for (int i = 0; i < currBlock.point.GetLength(1); ++i)
            {
                x = pos.X + currBlock.point[rot, i].X;
                y = pos.Y + currBlock.point[rot, i].Y;
                if (0 <= x && 0 <= y)
                {
                    screen[x, y] = currBlock.color;
                }
            }
        }

        /// <summary>
        /// 初期ブロック配置
        /// </summary>
        private void PutStartBlock()
        {
            pos = this.mStartPos;
            currBlock = BlockManager.getBlock();
            rot = (new Random()).Next(0, currBlock.point.GetLength(0));
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            const int width = 10;
            const int height = 20;
            Rectangle[,] destR = new Rectangle[width, height];

            for (int i = 0; i < width; ++i)
            {
                for (int k = 0; k < height; ++k)
                {
                    destR[i, k] = new Rectangle(i * Const.BlockSize, k * Const.BlockSize, Const.BlockSize, Const.BlockSize);

                    e.Graphics.DrawImage(mImg, destR[i, k], mBlock[screen[i, k]], GraphicsUnit.Pixel);
                }
            }
        }

        /// <summary>
        /// タイマー処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            Point nextPos = new Point(pos.X, pos.Y);    // 次の位置
            int nextRot = rot;                          // 次の回転

            // ゆっくりブロックを落下させるための考慮（10回に1度だけ落ちる）
            if ((fallCnt = (fallCnt + 1) % 10) == 0)
            {
                pos.Y += 1;
            }

            // 現在のブロック位置を取得
            Point[] currLoc = GetLoc(currBlock, pos, rot);

            // 現在のブロック位置を黒で初期化
            PutBlock(currLoc, 0);

            // 次の位置にブロックが配置不可か判定
            if (IsCollision() == true)
            {
            }
            else
            {

            }

            for (int i = 0; i < currBlock.point.GetLength(1); ++i)
            {
                screen[pos.X + currBlock.point[rot, i].X, pos.Y + currBlock.point[rot, i].Y] = 0;
            }


            for (int i = 0; i < currBlock.point.GetLength(1); ++i)
            {
                screen[pos.X + currBlock.point[rot, i].X, pos.Y + currBlock.point[rot, i].Y] = currBlock.color;
            }

            Invalidate();
        }


        private bool IsCollision()
        {
            return false;
        }

        /// <summary>
        /// ブロックを画面に配置したときの位置を取得
        /// </summary>
        /// <param name="block"></param>
        /// <param name="pt"></param>
        /// <param name="rot"></param>
        /// <returns></returns>
        private Point[] GetLoc(Block block, Point pt, int rot)
        {
            Point[] retLoc = new Point[block.point.GetLength(1)];
            for (int i = 0; i < retLoc.Length; ++i)
            {
                retLoc[i] = new Point(pt.X + block.point[rot, i].X, pt.Y + block.point[rot, i].Y);
            }
            return retLoc;
        }

        /// <summary>
        /// ブロックを配置
        /// </summary>
        /// <param name="points">ブロック座標</param>
        /// <param name="color">色</param>
        private void PutBlock(Point[] points, int color)
        {
            foreach (Point pt in points)
            {
                screen[pt.X, pt.Y] = color;
            }
        }
    }
}
