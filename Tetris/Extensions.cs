using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    static class Extensions
    {
        public static void DrawLine(
            this SpriteBatch spriteBatch,
            int x1, int y1,
            int x2, int y2,
            Color color,
            float thickness = 1.0f)
        {
            Vector2 start = new Vector2(x1, y1);
            Vector2 end = new Vector2(x2, y2);
            Vector2 delta = end - start;

            float angle = (float)Math.Atan2(delta.Y, delta.X);

            spriteBatch.Draw(
                Screen.Pixel,
                start,
                null,
                color,
                angle,
                new Vector2(0, 0),
                new Vector2(delta.Length(), thickness),
                SpriteEffects.None, 0f);
        }

        /// color1 = fill color, color2 = outline color
        public static void DrawRectangle(
            this SpriteBatch spriteBatch,
            int x, int y,
            int width, int height,
            Color color1, Color color2
            )
        {
            // fill
            Rectangle rect = new Rectangle(x, y, width, height);
            spriteBatch.Draw(Screen.Pixel, rect, null, color1);

            // outline
            spriteBatch.DrawLine(x, y, x + width, y, color2);
            spriteBatch.DrawLine(x, y, x, y + height, color2);
            spriteBatch.DrawLine(x + width, y, x + width, y + height, color2);
            spriteBatch.DrawLine(x, y + height, x + width, y + height, color2);
        }
	}
}
