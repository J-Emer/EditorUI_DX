using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorUI_DX
{
    public class OnScreenLog
    {
        public static OnScreenLog Instance;

        public Vector2 StartPosition { get; set; } = new Vector2(10, 10);
        public float YOffset { get; set; } = 20f;
        public Color DrawColor { get; set; } = Color.Black;
        public SpriteFont Font { get; private set; }

        private List<string> _messages = new List<string>();

        public OnScreenLog(SpriteFont _font)
        {
            this.Font = _font;
            Instance = this;
        }
        public void Log(string _message)
        {
            _messages.Add(_message);
        }
        public void Clear()
        {
            _messages.Clear();
        }
        public void Draw(SpriteBatch _spritebatch)
        {
            for (int i = 0; i < _messages.Count; i++)
            {
                float yPos = (i * YOffset) + StartPosition.Y;

                _spritebatch.DrawString(Font, _messages[i], new Vector2(StartPosition.X, yPos), DrawColor);
            }
        }
    }
}
