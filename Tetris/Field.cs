using System;
using Microsoft.Xna.Framework;

namespace Tetris
{
    public class Field
    {
        public const int Width = 12;
        public const int Height = 21;

        private Block[,] _field = new Block[Width, Height + 1];

        /// <summary>
        /// Constructor
        /// </summary>
        public Field()
        {
            // initial object
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height + 1; j++)
                {
                    _field[i, j] = new Block(false, Color.Transparent);
                }
            }
        }

        /// <summary>
        /// クリア処理
        /// </summary>
        /// <param name="fill"></param>
        public void Clear(bool fill)
        {
            if (fill)
            {
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        _field[i, j].DoEmpty();
                    }
                }
            }

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    // 左右の壁
                    if (i == 0 || i == Width - 1)
                    {
                        _field[i, j].Update(true, Color.DarkGray);
                    }

                    // 一番下
                    if (j == Height - 1)
                    {
                        _field[i, j].Update(true, Color.DarkGray);
                    }
                }
            }
        }

        /// <summary>
        /// 指定した座標のブロックが有効かどうかを返す
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsFilled(int x, int y)
        {
            return _field[x, y].Filled;
        }

        /// <summary>
        /// 指定座標のブロックオブジェクト取得
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Block GetBlock(int x, int y)
        {
            return _field[x, y];
        }

        /// <summary>
        /// 指定座標のブロックオブジェクト設定
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetBlock(int x, int y, Block b)
        {
            _field[x, y] = b;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="f"></param>
        /// <param name="c"></param>
        public void UpdateBlock(int x, int y, bool f, Color c)
        {
            _field[x, y].Update(true, c);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Color GetBlockColor(int x, int y)
        {
            return _field[x, y].Color;
        }

        /// <summary>
        /// オブジェクト複製
        /// </summary>
        public Field Duplicate()
        {
            Field temp = new Field();

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height + 1; j++)
                {
                    temp.SetBlock(i, j, new Block(this.IsFilled(i, j), this.GetBlockColor(i, j)));
                }
            }
            return temp;
        }
    }
}
