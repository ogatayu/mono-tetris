using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    using LayerBit = Byte;

    /// <summary>
    /// Screen class
    /// </summary>
    public class Screen
    {
        // constance value
        public const int Width  = 640;
        public const int Height = 600;

        public static Texture2D Pixel { get; private set; }  // a single white pixel

        public Screen(Game game)
        {
            Pixel = new Texture2D(game.GraphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });
        }

    }
}
