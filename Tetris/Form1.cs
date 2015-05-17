using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

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
        readonly Point mStartPos = new Point(4, 1);

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
            Point[] nextLoc;    // 次のブロック位置

            // 現在のブロック位置を取得
            Point[] currLoc = GetBlockLocation(currBlock, pos, rot);

            // 現在のブロック位置を黒で初期化
            PutBlock(currLoc, 0);

            // ゆっくりブロックを落下させるための考慮（10回に1度だけ落ちる）
            if ((fallCnt = (fallCnt + 1) % 5) == 0)
            {
                // ブロック落下
                nextPos.Y += 1;
            }
            else if (Keyboard.IsKeyDown(Key.Right))
            {
                // 右へブロックの座標を移動
                nextPos.X += 1;
                nextLoc = GetBlockLocation(currBlock, nextPos, nextRot);

                // 壁もしくは他のブロックと干渉しているか判定
                if (IsWallCollision(nextLoc) == true ||
                    IsBlockCollision(nextLoc) == true )
                {
                    // 元にに戻す
                    nextPos.X -= 1;
                }
            }
            else if (Keyboard.IsKeyDown(Key.Left))
            {
                // 左へブロックの座標を移動
                nextPos.X -= 1;
                nextLoc = GetBlockLocation(currBlock, nextPos, nextRot);

                // 壁もしくは他のブロックと干渉しているか判定
                if (IsWallCollision(nextLoc) == true ||
                    IsBlockCollision(nextLoc) == true )
                {
                    // 元にに戻す
                    nextPos.X += 1;
                }
            }
            else if (Keyboard.IsKeyDown(Key.Down))
            {
                // ブロック落下
                nextPos.Y += 1;
            }
            else if (Keyboard.IsKeyDown(Key.Up))
            {
                // ブロックの回転 ※パターン数を超えないよう考慮
                nextRot = (nextRot + 1) % currBlock.point.GetLength(0);
            }

            // 次のブロック位置を取得
            nextLoc = GetBlockLocation(currBlock, nextPos, nextRot);

            // 次の位置にブロックが配置不可か判定
            if (IsCollision(nextLoc) == true)
            {
                // 次の位置に置けないので元の位置に置く
                PutBlock(currLoc, currBlock.color);

                PutStartBlock();

                currLoc = GetBlockLocation(currBlock, pos, rot);
                if (IsCollision(currLoc))
                {
                    PutBlock(currLoc, currBlock.color);
                    GameOver();
                }
            }
            else
            {
                // 次の位置に置く
                PutBlock(nextLoc, currBlock.color);

                // 座標位置の更新
                pos = nextPos;
                rot = nextRot;
            }

            // 再描画イベント
            Invalidate();
        }

        /// <summary>
        /// ブロックが床、もしくは他のブロックと干渉するか判定
        /// </summary>
        /// <param name="loc">ブロック座標</param>
        /// <returns>干渉結果</returns>
        private bool IsCollision(Point[] loc)
        {
            int h = screen.GetLength(1);

            for (int i = 0; i < loc.Length; ++i)
            {
                if (loc[i].Y < 0 || h <= loc[i].Y || screen[loc[i].X, loc[i].Y] != 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ブロックが壁（画面外）と衝突しているか判定
        /// </summary>
        /// <param name="loc">ブロック座標リスト</param>
        /// <returns>衝突フラグ</returns>
        private bool IsWallCollision(Point[] loc)
        {
            int w = screen.GetLength(0);

            for (int i = 0; i < loc.Length; ++i)
            {
                if (loc[i].X < 0 || w <= loc[i].X)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ブロックが他のブロックと衝突しているか判定
        /// </summary>
        /// <param name="loc">ブロック座標リスト</param>
        /// <returns>衝突フラグ</returns>
        private bool IsBlockCollision(Point[] loc)
        {
            for (int i = 0; i < loc.Length; ++i)
            {
                if (screen[loc[i].X, loc[i].Y] != 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ブロックを画面に配置したときの位置を取得
        /// </summary>
        /// <param name="block">ブロック情報</param>
        /// <param name="pt">XY座標</param>
        /// <param name="rot">回転</param>
        /// <returns>ブロック情報リスト</returns>
        private Point[] GetBlockLocation(Block block, Point pt, int rot)
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

        /// <summary>
        /// ゲームオーバー
        /// </summary>
        private void GameOver()
        {
            // ブロックを赤で塗りつぶす
            for (int i = 0; i < Const.Width; ++i)
            {
                for (int j = 0; j < Const.Height; ++j)
                {
                    if (screen[i, j] != 0)
                    {
                        screen[i, j] = 5;
                    }
                }
            }

            // タイマー停止
            timer1.Enabled = false;
        }
    }
}
