using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class Tetris : Game
    {
        #region Public valiable
        public Screen screen;

        public enum GameState
        {
            Title,
            InGame,
            Gameover,
            StageClear,
            Pause
        }
        public GameState CurrentState = GameState.InGame; // @@@

        public GameEngine gameengine;
        #endregion

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Tetris()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = Screen.Width;
            _graphics.PreferredBackBufferHeight = Screen.Height;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            screen = new Screen(this);

            gameengine = new GameEngine();
            gameengine.Initialize(-1);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            switch (CurrentState)
            {
                case GameState.Title:
                    break;
                case GameState.InGame:
                    gameengine.Update(gameTime);
                    break;
                case GameState.Gameover:
                    break;
                case GameState.StageClear:
                    break;
                case GameState.Pause:
                    break;
                default:
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred);

            switch (CurrentState)
            {
                case GameState.Title:
                    break;
                case GameState.InGame:
                    gameengine.Draw(gameTime, _spriteBatch);
                    break;
                case GameState.Gameover:
                    break;
                case GameState.StageClear:
                    break;
                case GameState.Pause:
                    break;
                default:
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
