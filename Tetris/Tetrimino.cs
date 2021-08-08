using System;
using Microsoft.Xna.Framework;

namespace Tetris
{
    public class Tetrimino
    {
        public const int TypeNum = (int)TetriminoType.Num;

        // public property
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Size {
            get { return _mino_now.size; }
            private set { }
        }
        public TetriminoType Type { get; private set; }

        private struct MinoTypeFormat
        {
            public string name;
            public Color color;
            public int size;
            public int[,] shape;

            public MinoTypeFormat(string n, Color c, int sz, int[,] sp)
            {
                name = n;
                color = c;
                size = sz;
                shape = sp;
            }
        }

        public enum TetriminoType { I, O, S, Z, J, L, T, Num, None }  // テトリミノの種類
        #region テトリミノの定義
        private MinoTypeFormat[] Mino = {
            new MinoTypeFormat(
                "I",
                Color.Cyan,
                4,
                new int[,]
                {
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 }
                }
            ),
            new MinoTypeFormat(
                "O",
                Color.Yellow,
                2,
                new int[,]
                {
                    { 1, 1 },
                    { 1, 1 },
                }
            ),
            new MinoTypeFormat(
                "S",
                Color.Green,
                3,
                new int[,]
                {
                    { 1, 0, 0 },
                    { 1, 1, 0 },
                    { 0, 1, 0 }
                }
            ),
            new MinoTypeFormat(
                "Z",
                Color.Red,
                3,
                new int[,]
                {
                    { 0, 1, 0 },
                    { 1, 1, 0 },
                    { 1, 0, 0 }
                }
            ),
            new MinoTypeFormat(
                "J",
                Color.Blue,
                3,
                new int[,]
                {
                    { 0, 1, 0 },
                    { 0, 1, 0 },
                    { 1, 1, 0 }
                }
            ),
            new MinoTypeFormat(
                "L",
                Color.Orange,
                3,
                new int[,]
                {
                    { 1, 1, 0 },
                    { 0, 1, 0 },
                    { 0, 1, 0 }
                }
            ),
            new MinoTypeFormat(
                "T",
                Color.Violet,
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

        private MinoTypeFormat _mino_now;

        /// <summary>
        /// Constructor
        /// </summary>
        public Tetrimino( TetriminoType type )
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

        public Color GetColor()
        {
            return _mino_now.color;
        }

        /// <summary>
        /// オブジェクト複製
        /// </summary>
        public Tetrimino Duplicate()
        {
            return (Tetrimino)MemberwiseClone();
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
