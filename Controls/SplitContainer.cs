using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EditorUI_DX.Brushes;
using EditorUI_DX.Controls;
using EditorUI_DX.Interfaces;
using EditorUI_DX.Utils;


namespace EditorUI_DX.Controls
{
    public class Split_Container : Control
    {

        public int SplitterWidth
        {
            get
            {
                return _splitterWidth;
            }
            set
            {
                if(_splitterWidth != value)
                {
                    _splitterWidth = value;
                    After_Invalidated();
                }
            }
        }
        private int _splitterWidth;

        private Rectangle _splitterHandle;
        private bool _dragHandle = false;
        private Solid_Brush _handleBrush;


        public Panel LeftPanel
        {
            get
            {
                return _leftPanel;
            }
        }
        public Panel RightPanel
        {
            get
            {
                return _rightPanel;
            }
        }

        public Color LeftPanelColor
        {
            get
            {
                return _leftPanel.BackgroundColor;
            }
            set
            {
                _leftPanel.BackgroundColor = value;
            }
        }
        public Color RightPanelColor
        {
            get
            {
                return _rightPanel.BackgroundColor;
            }
            set
            {
                _rightPanel.BackgroundColor = value;
            }
        }

        public Layout LeftPanelLayout
        {
            get
            {
                return _leftPanel.Layout;
            }
            set
            {
                _leftPanel.Layout = value;
            }
        }
        public Layout RightPanelLayout
        {
            get
            {
                return _rightPanel.Layout;
            }
            set
            {
                _rightPanel.Layout = value;
            }
        }

        public Padding LeftPanelPadding
        {
            get
            {
                return _leftPanel.Padding;
            }
            set
            {
                _leftPanel.Padding = value;
                After_Invalidated();
            }
        }
        public Padding RightPanelPadding
        {
            get
            {
                return _rightPanel.Padding;
            }
            set
            {
                _rightPanel.Padding = value;
                After_Invalidated();
            }
        }



        public Vector2_Int SplitterLocation
        {
            get
            {
                return new Vector2_Int(_splitterHandle.X, _splitterHandle.Y);
            }
            set
            {
                _splitterHandle.X = value.X;
            }
        }
        public Color SplitterColor{get;set;} = Color.Black;
        public Color SplitterSelectColor{get;set;} = Color.Yellow;

        private Panel _leftPanel;
        private Panel _rightPanel;

        private bool _firstMove;


        public Split_Container(Desktop _desktop) : base(_desktop)
        {
            _firstMove = true;

            _leftPanel = new Panel(_desktop)
            {
                Name = this.Name +"_Left_Panel",
                Position = Vector2_Int.Zero,
                Size = Vector2_Int.Zero,
                BackgroundColor = Color.Red,
                Layout = new Vertical_Layout()
            };

            _rightPanel = new Panel(_desktop)
            {
                Name = this.Name +"_Righ_Panel",
                Position = Vector2_Int.Zero,
                Size = Vector2_Int.Zero,
                BackgroundColor = Color.Green,
                Layout = new Vertical_Layout()
            };
            

            _handleBrush = new Solid_Brush(_desktop);
            _handleBrush.DrawColor = Color.Black;

            //_splitterHandle = new Rectangle(this.Position.X + 100, this.Position.Y, SplitterWidth, this.Size.Y);

            this.OnMouseDown += Container_MouseDown;
            this.OnMouseUp += Container_MouseUp;
        }

        protected override void After_Invalidated()
        {
            if(_firstMove && Position != Vector2_Int.Zero && Size != Vector2_Int.Zero)
            {
                _firstMove = false;
                _splitterHandle = new Rectangle(this.Position.X + 200, this.Position.Y, _splitterWidth, this.Size.Y);
            }
            _splitterHandle.Y = this.Position.Y;
            _splitterHandle.Width = _splitterWidth;
            _splitterHandle.Height = this.Size.Y;

            Handle_Left();
            Handle_Right();
        }

        private void Container_MouseDown(EditorUI_DX.Utils.MouseEventArgs e)
        {
            _dragHandle = _splitterHandle.Contains(this._desktop.Input.GetMouseRect());
            if(_dragHandle)
            {
                _handleBrush.DrawColor = SplitterSelectColor;
            }
        }
        private void Container_MouseUp(EditorUI_DX.Utils.MouseEventArgs e)
        {
            _dragHandle = false;
            _handleBrush.DrawColor = SplitterColor;
        }

        protected override void After_Process()
        {
            if(_dragHandle)
            {
                _splitterHandle.X = Vector2_Int.FromVec2(this._desktop.Input.MousePosition).X;
                _splitterHandle.X = Math.Clamp(_splitterHandle.X, this.SourceRectangle.Left, this.SourceRectangle.Right - SplitterWidth);
                After_Invalidated();
            }

            _leftPanel.Process();
            _rightPanel.Process();
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            _leftPanel.Render(_spritebatch);
            _rightPanel.Render(_spritebatch);
            _spritebatch.Draw(_handleBrush.Texture, _splitterHandle, _handleBrush.DrawColor);
        }


        private void Handle_Left()
        {
            _leftPanel.Position = this.Position;
            int width = _splitterHandle.X - this.Position.X;
            _leftPanel.Size = new Vector2_Int(width, this.Size.Y);
        }

        private void Handle_Right()
        {
            int x = _splitterHandle.X + _splitterHandle.Width;
            int width = this.Size.X - ((_splitterHandle.X - Position.X) + _splitterHandle.Width);
            _rightPanel.Position = new Vector2_Int(x, this.Position.Y);
            _rightPanel.Size = new Vector2_Int(width, this.Size.Y);
        }

    }
}


