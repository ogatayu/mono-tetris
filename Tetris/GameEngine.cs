using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class GameEngine
    {
        private uint rnd;

        private Field _field_draw = new Field();
        private Field _field_fixed = new Field();

        private Tetrimino _tetrimino_now;  // 操作中のブロック
        private int _fall_speed = 2;
        private int _fall_count = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public GameEngine()
        {
            _tetrimino_now = new Tetrimino( Tetrimino.TetriminoType.None );
         }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="seed"></param>
        public void Initialize(int seed)
        {
            if (seed >= 0)
            {
                rnd = (uint)seed;
            }
            else
            {
                rnd = (uint)DateTime.Now.Ticks;
            }

            _field_fixed.Clear(true);
        }

        /// <summary>
        /// 周囲の要素との衝突判定
        /// </summary>
        private bool BlockCollision()
        {
            for (int i = 0; i < _tetrimino_now.Size; i++)
            {
                for (int j = 0; j < _tetrimino_now.Size; j++)
                {
                    if (_tetrimino_now.IsBlockExist(i,j))
                    {
                        int x = _tetrimino_now.X + i;
                        int y = _tetrimino_now.Y + j;

                        if(y < 0)
                        {
                            continue;
                        }

                        // 左右の壁
                        if (x < 0)
                        {
                            _tetrimino_now.Move(1, 0);
                            return true;
                        }
                        else if (x >= Field.Width)
                        {
                            _tetrimino_now.Move(-1, 0);
                            return true;
                        }

                        // 他のブロックと衝突
                        if (_field_fixed.IsFilled(x,y))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// ブロックを確定すべきか判定する
        /// </summary>
        /// <returns></returns>
        private bool IsCollisionBlockAndFloor()
        {
            for (int i = 0; i < _tetrimino_now.Size; i++)
            {
                for (int j = 0; j < _tetrimino_now.Size; j++)
                {
                    if (_tetrimino_now.IsBlockExist(i, j))
                    {
                        int x = _tetrimino_now.X + i;
                        int y = _tetrimino_now.Y + j;

                        if (y < 0)
                        {
                            continue;
                        }
                        else if(y >= Field.Height)
                        {
                            return true;
                        }

                        if (_field_fixed.IsFilled(x, y))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 1行消して下に詰める
        /// </summary>
        /// <param name="line"></param>
        private void LineRemove(int line)
        {
            Field temp = new Field();
            temp.Clear(false);

            int y = Field.Height;  // 下から処理する
            int copy_line = y;
            while(y >= 0 && copy_line >= 0)
            {
                if (y == line)
                {
                    y--;
                    continue;
                }

                for (int x = 0; x < Field.Width; x++)
                {
                    temp.SetBlock(x, copy_line, _field_fixed.GetBlock(x, y));
                }
                y--;
                copy_line--;
            }

            _field_fixed = temp;

            _field_fixed.Clear(false);
        }

        /// <summary>
        /// ブロックの回転処理
        /// </summary>
        /// <param name="direction">0 = 左回転, 1 = 右回転</param>
        private void BlockRotate(int direction)
        {
            if (_tetrimino_now.Type == Tetrimino.TetriminoType.O)
            {
                return;
            }

            // とっておく
            Tetrimino org_block = _tetrimino_now.Duplicate();

            // まわす
            if (direction == 0)
            {
                // 左回転
                _tetrimino_now.RotateAnticlockwise();

            }
            else if(direction == 1)
            {
                // 右回転
                _tetrimino_now.RotateClockwise();
            }

            // 回転していいか判定
            if (BlockCollision())
            {
                if (_tetrimino_now.Type == Tetrimino.TetriminoType.I)
                {
                    // TODO: I は衝突していたら無条件に回転不可能なので、もとに戻す
                    _tetrimino_now = org_block;
                }
                else
                {
                    // 右に移動して再判定
                    _tetrimino_now.Move(1, 0);
                    if (BlockCollision())
                    {
                        // だめだったら左に移動して再判定
                        _tetrimino_now.Move(2, 0);
                        if (BlockCollision())
                        {
                            // 駄目だったら回転不可能
                            _tetrimino_now = org_block;
                        }
                    }
                }
            }
        }

        private int _key_down_count_Left = 0;
        private int _key_down_count_Right = 0;
        private int _key_down_count_Up = 0;
        private int _key_down_count_Down = 0;
        private int _key_down_count_Z = 0;
        private int _key_down_count_X = 0;

        /// <summary>
        /// ブロック操作
        /// </summary>
        private void Control()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                if (_key_down_count_Z == 0)
                {
                    // 左回転
                    BlockRotate(0);
                }

                _key_down_count_Z++;
            }
            else
            {
                _key_down_count_Z = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                if (_key_down_count_X == 0)
                {
                    // 右回転
                    BlockRotate(1);
                }

                _key_down_count_X++;
            }
            else
            {
                _key_down_count_X = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if(_key_down_count_Left == 0)
                {
                    // 左へ移動
                    _tetrimino_now.Move(-1, 0);

                    // 衝突してたら戻す
                    if(BlockCollision())
                    {
                    _tetrimino_now.Move(1, 0);
                    }
                }

                _key_down_count_Left++;
            }
            else
            {
                _key_down_count_Left = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if(_key_down_count_Right == 0)
                {
                    // 右へ移動
                    _tetrimino_now.Move(1, 0);

                    // 衝突してたら戻す
                    if (BlockCollision())
                    {
                        _tetrimino_now.Move(-1, 0);
                    }
                }

                _key_down_count_Right++;
            }
            else
            {
                _key_down_count_Right = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (_key_down_count_Up == 0)
                {
                    while (!BlockCollision())
                    {
                        // 衝突するまで下に下ろす
                        _tetrimino_now.Move(0, 1);
                    }
                    _fall_count = 0;
                }

                _key_down_count_Up++;
            }
            else
            {
                _key_down_count_Up = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                if(_key_down_count_Down == 0)
                {
                    _tetrimino_now.Move(0, 1);
                }

                _key_down_count_Down++;
            }
            else
            {
                _key_down_count_Down = 0;
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // ブロック生成
            if(_tetrimino_now.Type == Tetrimino.TetriminoType.None)
            {
                uint t = Rand.NextRandInt(ref rnd) % Tetrimino.TETRIMINO_TYPE_NUM;
                //t = 0;

                _tetrimino_now = new Tetrimino((Tetrimino.TetriminoType)t);
                _tetrimino_now.SetPosition((Field.Width/2) - (_tetrimino_now.Size/2), 0);
            }

            // ブロック操作
            Control();

            // ブロックを一つ下に落とす
            _fall_count += _fall_speed;
            if(_fall_count >= 60)
            {
                _tetrimino_now.Move(0, 1);
                _fall_count = 0;
            }

            if(IsCollisionBlockAndFloor())
            {
                // 他ブロックまたは床と衝突してたら位置を戻してブロック確定処理
                _tetrimino_now.Move(0, -1);
                _tetrimino_now.Destroy();

                for (int i = 0; i < _tetrimino_now.Size; i++)
                {
                    for (int j = 0; j < _tetrimino_now.Size; j++)
                    {
                        if (_tetrimino_now.IsBlockExist(i, j))
                        {
                            int x = _tetrimino_now.X + i;
                            int y = _tetrimino_now.Y + j;

                            if (x >= 0 && x < Field.Width &&
                                y >= 0 && y < Field.Height)
                            {
                                _field_fixed.UpdateBlock(x, y, true, _tetrimino_now.GetColor());
                            }
                        }
                    }
                }

                // 列が揃ってたら消す
                for (int y = 0; y < Field.Height - 1; y++)
                {
                    int line_count = 0;

                    // 両脇の1マスを省いた判定
                    for (int x = 1; x < Field.Width - 1; x++)
                    {
                        if (_field_fixed.IsFilled(x, y))
                        {
                            line_count++;
                        }
                    }
                    if(line_count >= Field.Width - 2 )
                    {
                        LineRemove(y);
                    }
                }

                // 最上部列にブロックがあればゲームオーバー
                for (int x = 1; x < Field.Width - 1; x++)
                {
                        if (_field_fixed.IsFilled(x, 0))
                    {
                        // TODO: とりあえず全クリア
                        _field_fixed.Clear(true);
                    }
                }
            }

            // 確定したフィールドを描画用にコピー
            _field_draw = (Field)_field_fixed.Duplicate();
            //Array.Copy(_field_fixed, _field_draw, _field_fixed.Length);

            // 描画用にブロックをフィールドにコピー
            for (int i = 0; i < _tetrimino_now.Size; i++)
            {
                for (int j = 0; j < _tetrimino_now.Size; j++)
                {
                    if (_tetrimino_now.IsBlockExist(i,j))
                    {
                        int x = _tetrimino_now.X + i;
                        int y = _tetrimino_now.Y + j;

                        if (x >= 0 && x < Field.Width &&
                            y >= 0 && y < Field.Height)
                        {
                            _field_draw.UpdateBlock(x, y, true, _tetrimino_now.GetColor());
                        }
                    }
                }
            }
        }


        private const int BLOCK_SIZE = 24;
        private const int BLOCK_X_BASE = BLOCK_SIZE;
        private const int BLOCK_Y_BASE = 0;

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(
                BLOCK_X_BASE,
                BLOCK_Y_BASE + BLOCK_SIZE,
                BLOCK_SIZE * Field.Width, BLOCK_SIZE * (Field.Height - 1),
                Color.Black,
                Color.Transparent
                );

            for (int i = 0; i < Field.Width; i++)
            {
                // 一番上の行は不可視とするので j は 1 始まり
                for (int j = 1; j < Field.Height; j++)
                {
                    if( _field_draw.IsFilled(i, j) )
                    {
                        spriteBatch.DrawRectangle(
                            BLOCK_X_BASE + (i * BLOCK_SIZE),
                            BLOCK_Y_BASE + (j * BLOCK_SIZE),
                            BLOCK_SIZE, BLOCK_SIZE,
                            _field_draw.GetBlockColor(i, j),
                            Color.Transparent
                            );
                    }
                }
            }
        }
    }
}
