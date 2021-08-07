using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class Block
    {
        public const int TETRIMINO_TYPE_NUM = (int)TetriminoType.Num;

        // public property
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Size {
            get { return _mino_now.size; }
            private set {}
        }
        public TetriminoType Type { get; private set; }

        private struct Tetrimino
        {
            public string name;
            public Color color;
            public int size;
            public int[,] shape;

            public Tetrimino(string n, Color c, int sz, int[,] sp)
            {
                name = n;
                color = c;
                size = sz;
                shape = sp;
            }
        }

        public enum TetriminoType { I, O, S, Z, J, L, T, Num, None }  // テトリミノの種類
        #region テトリミノの定義
        private Tetrimino[] Mino = {
            new Tetrimino(
                "I",
                new Color(0x80,0x80,0xF0),
                4,
                new int[,]
                {
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 }
                }
            ),
            new Tetrimino(
                "O",
                new Color( 0xF0, 0xF0, 0x00 ),
                2,
                new int[,]
                {
                    { 1, 1 },
                    { 1, 1 },
                }
            ),
            new Tetrimino(
                "S",
                new Color( 0x00, 0x80, 0x00 ),
                3,
                new int[,]
                {
                    { 1, 0, 0 },
                    { 1, 1, 0 },
                    { 0, 1, 0 }
                }
            ),
            new Tetrimino(
                "Z",
                new Color( 0xF0, 0x00, 0x00 ),
                3,
                new int[,]
                {
                    { 0, 1, 0 },
                    { 1, 1, 0 },
                    { 1, 0, 0 }
                }
            ),
            new Tetrimino(
                "J",
                new Color( 0x00, 0x00, 0xF0 ),
                3,
                new int[,]
                {
                    { 0, 1, 0 },
                    { 0, 1, 0 },
                    { 1, 1, 0 }
                }
            ),
            new Tetrimino(
                "L",
                new Color( 0xF0, 0x80, 0x00 ),
                3,
                new int[,]
                {
                    { 1, 1, 0 },
                    { 0, 1, 0 },
                    { 0, 1, 0 }
                }
            ),
            new Tetrimino(
                "T",
                new Color( 0x80, 0x00, 0x80 ),
                3,
                new int[,]
                {
                    { 0, 1, 0 },
                    { 1, 1, 0 },
                    { 0, 1, 0 }
                }
            ),
        };
        #endregion

        private Tetrimino _mino_now;

        /// <summary>
        /// Constructor
        /// </summary>
        public Block( TetriminoType type )
        {
            Type = type;

            if(type == TetriminoType.None)
            {
                return;
            }

            _mino_now = Mino[(int)type];
        }

        /// <summary>
        /// ブロックの位置を指定
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// 指定されたぶんだけ移動する
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public void Move(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }

        /// <summary>
        /// 指定した座標にブロックが存在するか
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsBlockExist(int x, int y)
        {
            if( x >= Size || y >= Size )
            {
                throw new Exception();
            }

            return (_mino_now.shape[x,y] == 1);
        }

        /// <summary>
        /// ブロックを時計回りに回転する
        /// </summary>
        public void RotateClockwise()
        {
            int rows = _mino_now.shape.GetLength(0);
            int cols = _mino_now.shape.GetLength(1);
            var t = new int[cols, rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    t[j, rows - i - 1] = _mino_now.shape[i, j];
                }
            }
            _mino_now.shape = t;
        }

        /// <summary>
        /// ブロックを反時計回りに回転する
        /// </summary>
        public void RotateAnticlockwise()
        {
            int rows = _mino_now.shape.GetLength(0);
            int cols = _mino_now.shape.GetLength(1);
            var t = new int[cols, rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    t[cols - j - 1, i] = _mino_now.shape[i, j];
                }
            }

            _mino_now.shape = t;
        }

        /// <summary>
        /// オブジェクト複製
        /// </summary>
        public Block Duplicate()
        {
            return (Block)MemberwiseClone();
        }

        /// <summary>
        /// オブジェクト破棄
        /// </summary>
        public void Destroy()
        {
            Type = TetriminoType.None;
        }
    }
}
