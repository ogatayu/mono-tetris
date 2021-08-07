using System;
using Microsoft.Xna.Framework;

namespace Tetris
{
    public class Block
    {
        public bool Filled { get; private set; }
        public Color Color { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="f">true: ブロック有効</param>
        /// <param name="c">Color</param>
        public Block(bool f, Color c)
        {
            Filled = f;
            Color = c;
        }

        /// <summary>
        /// ブロックの状態を更新する
        /// </summary>
        /// <param name="f">true: ブロック有効</param>
        /// <param name="c">Color</param>
        public void Update(bool f, Color c)
        {
            Filled = f;
            Color = c;
        }

        /// <summary>
        /// ブロックを空の状態にする
        /// </summary>
        public void DoEmpty()
        {
            Filled = false;
        }
    }
}
