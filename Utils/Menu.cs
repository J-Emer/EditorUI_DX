using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EditorUI_DX.Brushes;
using EditorUI_DX.Controls;
using EditorUI_DX.Interfaces;
using EditorUI_DX.Utils;



namespace EditorUI_DX.Utils
{
    public class Menu : Control
    {
        public Control_Collection<Control> Controls = new Control_Collection<Control>();
        private Layout _layout;
        private Padding _padding;



        public Menu(Desktop _desktop) : base(_desktop)
        {
            _layout = new Horizontal_Layout();
            _padding = new Padding(0,5,0,0);
            Controls.OnControlsChanged += After_Invalidated;

            this.Size = new Vector2_Int(100, 30);
            this.BackgroundColor =  new Color(57, 60, 64);
        }
        protected override void After_Invalidated()
        {
            _layout.Handle_Layout(Controls.Controls, this.SourceRectangle, this._padding);
        }
        protected override void After_Process()
        {
            for (int i = 0; i < Controls.Collecton.Count; i++)
            {
                Controls.Collecton[i].Process();
            }
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            for (int i = 0; i < Controls.Collecton.Count; i++)
            {
                Controls.Collecton[i].Render(_spritebatch);
            }
        }

        /// <summary>
        /// Adds a Button to the Menu
        /// </summary>
        /// <param name="_text">Text the Button will display</param>
        /// <param name="_name">The Name of the Button (optional)</param>
        /// <returns></returns>
        public Button Add_Button(string _text, string _name = "")
        {
            Button _b = new Button(this._desktop)
            {
                Name = _name,
                Text = _text,
                Size = new Vector2_Int(150,30),
                BackgroundColor = Color.Transparent,
                NormalColor = Color.Transparent
            };
            Controls.Add(_b);
            return _b;
        }

        /// <summary>
        /// Adds a DropDownButton to the Menu. 
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_name"></param>
        /// <returns></returns>
        public Drop_Down_Button AddDropDown(string _text, string _name = "")
        {
            Drop_Down_Button _dropDown = new Drop_Down_Button(this._desktop)
            {
                Name = _name,
                Text = _text,
                Size = new Vector2_Int(150,30),
                BackgroundColor = Color.Transparent
            };
            Controls.Add(_dropDown);
            return _dropDown;
        }
    }

    public class Drop_Down_Button : Control
    {
        private Label _label;
        public string Text
        {
            get
            {
                return _label.Text;
            }
            set
            {
                _label.Text = value;
            }
        }
        private Panel _panel;
        public Control_Collection<Control> Buttons
        {
            get
            {
                return _panel.Controls;
            }
        }
        private Layout _layout;
        private Padding _padding;
        private string _fontName;


        public Color NormalColor{get;set;} = Color.Transparent;
        public Color HighlightColor{get;set;} = new Color(39, 126, 242);





        public Drop_Down_Button(Desktop _desktop) : base(_desktop)
        {

            _label = new Label(_desktop);
            _label.Text = "Button";

            _padding = new Padding(5);
            _layout = new Vertical_Stretch_Layout();

            _panel = new Panel(_desktop)
            {
                Size = new Vector2_Int(200, 200),
                BackgroundColor =  new Color(57, 60, 64),
                IsActive = false
            };
            _panel.OnMouseExit += Panel_MouseExit;

            Buttons.OnControlsChanged += After_Invalidated;

            this.OnMouseEnter += Control_MouseEnter;
            this.OnMouseExit += Control_MouseExit;
            this.OnMouseDown += Control_MouseDown;

            this.Size = new Vector2_Int(300,30);
        }
        protected override void After_Invalidated()
        {
            _label.Position = this.Center - Vector2_Int.FromVec2(_label.FontBrush.HalfFontSize);

            int _height = _padding.Top;

            for (int i = 0; i < Buttons.Collecton.Count; i++)
            {
                _height += Buttons.Collecton[i].Size.Y + _padding.Bottom;
            }

            int y = this.Position.Y + this.Size.Y;
            _panel.Position = new Vector2_Int(this.Position.X, y);
            _panel.Size = new Vector2_Int(200, _height);

            _layout.Handle_Layout(Buttons.Collecton, _panel.SourceRectangle, _padding);
        }
        protected override void After_Process()
        {
            _label.Process();
            _panel.Process();
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            _label.Render(_spritebatch);
            _panel.Render(_spritebatch);
        }


        public Button AddButton(string _text, string _name = "")
        {
            Button _b = new Button(this._desktop)
            {
                Name = _name,
                Text = _text,
                Size = new Vector2_Int(150,30),
                BackgroundColor = Color.Transparent,
                NormalColor = Color.Transparent
            };
            Buttons.Add(_b);
            return _b;
        }




        private void Control_MouseEnter()
        {
            BackgroundColor = HighlightColor;
        }
        private void Control_MouseExit()
        {
            BackgroundColor = NormalColor;
        }
        private void Control_MouseDown(EditorUI_DX.Utils.EventArgs e)
        {
            _panel.IsActive = !_panel.IsActive;
        }
        private void Panel_MouseExit()
        {
            if(_panel.IsActive)
            {
                _panel.IsActive = false;
            }
        }
    }
}


