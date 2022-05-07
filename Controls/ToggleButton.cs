using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EditorUI_DX.Brushes;
using EditorUI_DX.Utils;



namespace EditorUI_DX.Controls
{
	public class Toggle_Button : Control
    {
        public event Action<object, bool> OnValueChanged;
        private Rectangle _selectRect;
        private Solid_Brush _selectBrush;

        public bool Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                Set_Select_Rect();
            }
        }
        private bool _value = false;


        public Color SelectedColor{get;set;} = new Color(39, 126, 242);
        public Color DeselectedColor{get;set;} = new Color(225, 231, 240);




        public Toggle_Button(Desktop _desktop) : base(_desktop)
        {
            _selectBrush = new Solid_Brush(_desktop);
            _selectRect = new Rectangle();

            this.BackgroundColor = new Color(57, 60, 64);
            this.Size = new Vector2_Int(100, 40);
        }
        protected override void After_Invalidated()
        {
            Set_Select_Rect();
        }
        private void Set_Select_Rect()
        {
            int _hW = this.Size.X / 2;
            int x = 0;

            if(_value)
            {
                x = this.Position.X;
                _selectBrush.DrawColor = SelectedColor;
            }
            else
            {
                x = this.Position.X + _hW;
                _selectBrush.DrawColor = DeselectedColor;
            }

            _selectRect.X = x + 2;
            _selectRect.Y = this.Position.Y + 2;
            _selectRect.Width = _hW - 4;
            _selectRect.Height = this.Size.Y - 4;

            OnValueChanged?.Invoke(this, _value);
        }
        protected override void After_Process()
        {
            bool _contains = _selectRect.Contains(Input.Instance.MousePosition);
            bool _lmb = Input.Instance.GetMouseButtonDown(0);

            if(_contains && _lmb)
            {
                _value = !_value;
                Set_Select_Rect();
            }
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            _spritebatch.Draw(_selectBrush.Texture, _selectRect, _selectBrush.DrawColor);
        }
    }
}


