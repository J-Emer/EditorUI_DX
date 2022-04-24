using System;
using EditorUI_DX.Brushes;
using EditorUI_DX.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EditorUI_DX.Controls
{
    public abstract class Element
    {
        public int ZOrder { get; set; } = 5;
        public string Name { get; set; }
        public object Tag { get; set; }
        public bool HasFocus
        {
            get
            {
                return _hasFocus;
            }
        }
        protected bool _hasFocus;
        public bool IsActive { get; set; } = true;
        public Vector2_Int Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                Invalidate();
            }
        }
        private Vector2_Int _position;
        public Vector2_Int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
                Invalidate();
            }
        }
        private Vector2_Int _size;
        public Vector2_Int Center
        {
            get
            {
                return Position + (Size / 2);
            }
        }
        public Rectangle SourceRectangle
        {
            get
            {
                return _sourceRectangle;
            }
        }
        private Rectangle _sourceRectangle = new Rectangle(0, 0, 0, 0);
        public Color BackgroundColor
        {
            get
            {
                return _brush.DrawColor;
            }
            set
            {
                _brush.DrawColor = value;
            }
        }
        private Solid_Brush _brush;
        protected Desktop _desktop;

        public event Action<Element, DockStyle> OnDockStyleChanged;
        public DockStyle DockStyle
        {
            get
            {
                return _dockStyle;
            }
            set
            {
                if (value != _dockStyle)
                {
                    _dockStyle = value;
                    OnDockStyleChanged?.Invoke(this, _dockStyle);
                }
            }
        }
        private DockStyle _dockStyle = DockStyle.None;


        private Border _border;
        public int BorderThickness
        {
            get
            {
                return _border.Thickness;
            }
            set
            {
                _border.Thickness = value;
            }
        }
        public Color BorderColor
        {
            get
            {
                return _border.DrawColor;
            }
            set
            {
                _border.DrawColor = value;
            }
        }




        internal Element(Desktop _desktop)
        {
            this._desktop = _desktop;
            _brush = new Solid_Brush(_desktop);
            _border = new Border(_desktop.Graphics, 0, this);
        }


        private void Invalidate()
        {
            _sourceRectangle.X = Position.X;
            _sourceRectangle.Y = Position.Y;
            _sourceRectangle.Width = Size.X;
            _sourceRectangle.Height = Size.Y;
            After_Invalidated();
        }
        protected virtual void After_Invalidated() { }


        public void Render(SpriteBatch _spritebatch)
        {
            if (!IsActive) { return; }

            _spritebatch.Draw(_brush.Texture, SourceRectangle, _brush.DrawColor);
            _border.Render(_spritebatch);
            After_Render(_spritebatch);
        }

        protected virtual void After_Render(SpriteBatch _spritebatch) { }
    }
}
