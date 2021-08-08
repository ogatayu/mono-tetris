using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetris
{
    class NextManager
    {
        private uint rnd;
        private List<Tetrimino> _next_list = new List<Tetrimino>();

        /// <summary>
        /// Constructor
        /// </summary>
        public NextManager(int seed) {
            if (seed >= 0)
            {
                rnd = (uint)seed;
            }
            else
            {
                rnd = (uint)DateTime.Now.Ticks;
            }
        }

        /// <summary>
        /// 現在のテトリミノに、新しいセットを追加する
        /// </summary>
        public void Reload()
        {
            var temp = new List<Tetrimino>();

            for (int i = 0; i < Tetrimino.TypeNum; i++)
            {
                temp.Add(new Tetrimino((Tetrimino.TetriminoType)i));
            }

            temp = temp.OrderBy(a => Rand.NextRandInt(ref rnd)).ToList();
            
            foreach (var t in temp)
            {
                _next_list.Add(t);
            }
        }

        /// <summary>
        /// 全部削除する
        /// </summary>
        public void Clear()
        {
            _next_list.Clear();
        }

        /// <summary>
        /// 先頭のテトリミノを取得し、リストから削除する
        /// </summary>
        /// <returns></returns>
        public Tetrimino Pull()
        {
            Tetrimino retobj = _next_list[0];
            _next_list.RemoveAt(0);
            return retobj;
        }

        /// <summary>
        /// テトリミノを取得する
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Tetrimino Get(int index)
        {
            return _next_list[index];
        }

        /// <summary>
        /// 残りの数を取得する
        /// </summary>
        /// <returns></returns>
        public int GetRemain()
        {
            return _next_list.Count();
        }
    }
}
