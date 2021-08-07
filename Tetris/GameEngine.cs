using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class GameEngine
    {
        private uint rnd;

        private const int FIELD_WIDTH = 12;
        private const int FIELD_HEIGHT = 21;
        private int[,] _field = new int[FIELD_WIDTH, FIELD_HEIGHT + 1];
        private int[,] _field_fixed = new int[FIELD_WIDTH, FIELD_HEIGHT + 1];

        private Block _block_now;  // 操作中のブロック
        private int _fall_speed = 2;
        private int _fall_count = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public GameEngine()
        {
            _block_now = new Block( Block.TetriminoType.None );
         }

        /// <summary>
        /// フィールドのクリア処理
        /// </summary>
        /// <param name="fill">true: 内側もすべて削除</param>
        private void FieldClear(bool fill)
        {
            if (fill)
            {
                Array.Clear(_field_fixed, 0, _field_fixed.Length);
            }

            for (int i = 0; i < FIELD_WIDTH; i++)
            {
                for (int j = 0; j < FIELD_HEIGHT; j++)
                {
                    // 左右の壁
                    if (i == 0 || i == FIELD_WIDTH - 1)
                    {
                        _field_fixed[i, j] = 255;
                    }

                    // 一番下
                    if (j == FIELD_HEIGHT - 1)
                    {
                        _field_fixed[i, j] = 255;
                    }
                }
            }
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

            FieldClear(true);
        }

        /// <summary>
        /// 周囲の要素との衝突判定
        /// </summary>
        private bool BlockCollision()
        {
            for (int i = 0; i < _block_now.Size; i++)
            {
                for (int j = 0; j < _block_now.Size; j++)
                {
                    if (_block_now.IsBlockExist(i,j))
                    {
                        int x = _block_now.X + i;
                        int y = _block_now.Y + j;

                        if(y < 0)
                        {
                            continue;
                        }

                        // 左右の壁
                        if (x < 0)
                        {
                            _block_now.Move(1, 0);
                            return true;
                        }
                        else if (x >= _field.GetLength(0))
                        {
                            _block_now.Move(-1, 0);
                            return true;
                        }

                        // 他のブロックとの衝突
                        if (_field_fixed[x, y] != 0)
                        {
                            return true;
#if false
                            if (i < 3)
                            {
                                // ブロックの左半分が衝突
                                _block_x++;
                                return true;
                            }
                            else if (i > 3)
                            {
                                // 右半分
                                _block_x--;
                                return true;
                            }

                            if (j < 3)
                            {
                                // ブロックの下半分が衝突
                                _block_now.Move(0, 1);
                                return true;
                            }
                            else if (j > 3)
                            {
                                // 上半分
                                _block_now.Move(0, -1);
                                return true;
                            }
#endif
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
            for (int i = 0; i < _block_now.Size; i++)
            {
                for (int j = 0; j < _block_now.Size; j++)
                {
                    if (_block_now.IsBlockExist(i, j))
                    {
                        int x = _block_now.X + i;
                        int y = _block_now.Y + j;

                        if (y < 0)
                        {
                            continue;
                        }
                        else if(y >= FIELD_HEIGHT)
                        {
                            return true;
                        }

                        if (_field_fixed[x, y] != 0)
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
            int[,] temp = new int[FIELD_WIDTH, FIELD_HEIGHT + 1];
            Array.Clear(temp, 0, temp.Length);

            int y = _field_fixed.GetLength(1) - 1;
            int copy_line = y;
            while(y >= 0 && copy_line >= 0)
            {
                if (y == line)
                {
                    y--;
                    continue;
                }

                for (int x = 0; x < _field_fixed.GetLength(0); x++)
                {
                    temp[x, copy_line] = _field_fixed[x, y];
                }
                y--;
                copy_line--;
            }

            Array.Copy(temp, _field_fixed, temp.Length);
            FieldClear(false); // 壁の再定義
        }

        /// <summary>
        /// ブロックの回転処理
        /// </summary>
        /// <param name="direction">0 = 左回転, 1 = 右回転</param>
        private void BlockRotate(int direction)
        {
            if (_block_now.Type == Block.TetriminoType.O)
            {
                return;
            }

            // とっておく
            Block org_block = _block_now.Duplicate();

            // まわす
            if (direction == 0)
            {
                // 左回転
                _block_now.RotateAnticlockwise();

            }
            else if(direction == 1)
            {
                // 右回転
                _block_now.RotateClockwise();
            }

            // 回転していいか判定
            if (BlockCollision())
            {
                if (_block_now.Type == Block.TetriminoType.I)
                {
                    // TODO: I は衝突していたら無条件に回転不可能なので、もとに戻す
                    _block_now = org_block;
                }
                else
                {
                    // 右に移動して再判定
                    _block_now.Move(1, 0);
                    if (BlockCollision())
                    {
                        // だめだったら左に移動して再判定
                        _block_now.Move(2, 0);
                        if (BlockCollision())
                        {
                            // 駄目だったら回転不可能
                            _block_now = org_block;
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
                    _block_now.Move(-1, 0);

                    // 衝突してたら戻す
                    if(BlockCollision())
                    {
                    _block_now.Move(1, 0);
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
                    _block_now.Move(1, 0);

                    // 衝突してたら戻す
                    if (BlockCollision())
                    {
                        _block_now.Move(-1, 0);
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
                        _block_now.Move(0, 1);
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
                    _block_now.Move(0, 1);
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
            if(_block_now.Type == Block.TetriminoType.None)
            {
                uint t = Rand.NextRandInt(ref rnd) % Block.TETRIMINO_TYPE_NUM;
                // t = 0;

                _block_now = new Block((Block.TetriminoType)t);
                _block_now.SetPosition((FIELD_WIDTH/2) - (_block_now.Size/2), -1);
            }

            // ブロック操作
            Control();

            // ブロックを一つ下に落とす
            _fall_count += _fall_speed;
            if(_fall_count >= 60)
            {
                _block_now.Move(0, 1);
                _fall_count = 0;
            }

            if(IsCollisionBlockAndFloor())
            {
                // 他ブロックまたは床と衝突してたら位置を戻してブロック確定処理
                _block_now.Move(0, -1);
                _block_now.Destroy();

                for (int i = 0; i < _block_now.Size; i++)
                {
                    for (int j = 0; j < _block_now.Size; j++)
                    {
                        if (_block_now.IsBlockExist(i, j))
                        {
                            int x = _block_now.X + i;
                            int y = _block_now.Y + j;

                            if (x >= 0 && x < _field_fixed.GetLength(0) &&
                                y >= 0 && y < _field_fixed.GetLength(1))
                            {
                                _field_fixed[x, y] = 1;
                            }
                        }
                    }
                }

                // 列が揃ってたら消す
                for (int y = 0; y < FIELD_HEIGHT; y++)
                {
                    int line_count = 0;

                    // 両脇の1マスを省いた判定
                    for (int x = 1; x < FIELD_WIDTH - 1; x++)
                    {
                        if (_field_fixed[x, y] == 1)
                        {
                            line_count++;
                        }
                    }
                    if(line_count >= FIELD_WIDTH - 2 )
                    {
                        LineRemove(y);
                    }
                }

                // 最上部列にブロックがあればゲームオーバー
                for (int x = 1; x < FIELD_WIDTH - 1; x++)
                {
                    if (_field_fixed[x, 0] == 1)
                    {
                        // TODO: とりあえず全クリア
                        FieldClear(true);
                    }
                }
            }

            // 確定したフィールドを作業用にコピー
            Array.Copy(_field_fixed, _field, _field_fixed.Length);

            // 描画用にブロックをフィールドにコピー
            for (int i = 0; i < _block_now.Size; i++)
            {
                for (int j = 0; j < _block_now.Size; j++)
                {
                    if (_block_now.IsBlockExist(i,j))
                    {
                        int x = _block_now.X + i;
                        int y = _block_now.Y + j;

                        if (x >= 0 && x < _field.GetLength(0) &&
                            y >= 0 && y < _field.GetLength(1))
                        {
                            _field[x, y] = 1;
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
#if false
            Color background_color = new Color(0xFF, 0xFF, 0xFF);
            spriteBatch.DrawRectangle(
                BLOCK_X_BASE,
                BLOCK_Y_BASE,
                BLOCK_SIZE * FIELD_WIDTH, BLOCK_SIZE * (FIELD_HEIGHT - 1),
                background_color, background_color
                );
#endif

            Color border_color;
            Color fill_color;
            for (int i = 0; i < FIELD_WIDTH; i++)
            {
                // 一番上の行は不可視とするので j は 1 始まり
                for (int j = 1; j < FIELD_HEIGHT; j++)
                {
                    switch (_field[i, j])
                    {
                        case 0:
                            border_color = new Color(0xCC, 0xCC, 0xCC);
                            fill_color = new Color(0xFF, 0xFF, 0xFF);
                            break;
                        case 1:
                            border_color = new Color(0xFF, 0x88, 0x00);
                            fill_color = new Color(0xFF, 0xCC, 0x00);
                            break;
                        case 255:
                            border_color = new Color(0x88, 0x88, 0x88);
                            fill_color = new Color(0x88, 0x88, 0x88);
                            break;
                        default:
                            border_color = new Color(0x00, 0x00, 0x00);
                            fill_color = new Color(0x00, 0x00, 0x00);
                            break;
                    }

                    spriteBatch.DrawRectangle(
                        BLOCK_X_BASE + (i * BLOCK_SIZE),
                        BLOCK_Y_BASE + (j * BLOCK_SIZE),
                        BLOCK_SIZE, BLOCK_SIZE,
                        fill_color, border_color
                        );
                }
            }
        }
    }
}
