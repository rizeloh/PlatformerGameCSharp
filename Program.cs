using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformerGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _playerTexture;
        private Vector2 _playerPosition;
        private Vector2 _playerVelocity;
        private bool _isOnGround;

        private Texture2D _platformTexture;
        private Rectangle[] _platforms;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Set the window size
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load player texture
            _playerTexture = new Texture2D(GraphicsDevice, 50, 60);
            Color[] playerData = new Color[50 * 60];
            for (int i = 0; i < playerData.Length; ++i) playerData[i] = Color.Blue;
            _playerTexture.SetData(playerData);
            _playerPosition = new Vector2(100, 500);

            // Load platform texture
            _platformTexture = new Texture2D(GraphicsDevice, 200, 10);
            Color[] platformData = new Color[200 * 10];
            for (int i = 0; i < platformData.Length; ++i) platformData[i] = Color.Black;
            _platformTexture.SetData(platformData);

            // Define platforms
            _platforms = new Rectangle[]
            {
                new Rectangle(100, 550, 200, 10),
                new Rectangle(400, 450, 200, 10),
                new Rectangle(200, 350, 200, 10),
            };
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Vector2 previousPosition = _playerPosition;

            // Player movement
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                _playerPosition.X -= 5;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                _playerPosition.X += 5;
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && _isOnGround)
            {
                _playerVelocity.Y = -10;
                _isOnGround = false;
            }

            _playerVelocity.Y += 0.5f; // gravity
            _playerPosition += _playerVelocity;

            // Collision detection
            _isOnGround = false;
            foreach (var platform in _platforms)
            {
                if (_playerPosition.X < platform.Right && _playerPosition.X + _playerTexture.Width > platform.Left &&
                    _playerPosition.Y < platform.Bottom && _playerPosition.Y + _playerTexture.Height > platform.Top)
                {
                    if (previousPosition.Y + _playerTexture.Height <= platform.Top)
                    {
                        _playerPosition.Y = platform.Top - _playerTexture.Height;
                        _playerVelocity.Y = 0;
                        _isOnGround = true;
                    }
                }
            }

            if (_playerPosition.Y > _graphics.PreferredBackBufferHeight - _playerTexture.Height)
            {
                _playerPosition.Y = _graphics.PreferredBackBufferHeight - _playerTexture.Height;
                _playerVelocity.Y = 0;
                _isOnGround = true;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_playerTexture, _playerPosition, Color.White);
            foreach (var platform in _platforms)
                _spriteBatch.Draw(_platformTexture, platform, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
}
