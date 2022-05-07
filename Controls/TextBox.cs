using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EditorUI_DX.Utils;




namespace EditorUI_DX.Controls
{
    public class TextBox : Control
    {
        /// <summary>
        /// Fires after the user has pressed the Enter key
        /// </summary>
        public event Action<object, string> OnTextSubmitted;
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

        public Color FontColor
        {
            get
            {
                return _label.FontColor;
            }
            set
            {
                _label.FontColor = value;
            }
        }

        public Color SelectedBorderColor{get;set;} = Color.Black;
        public Color NotSelectedBorderColor{get;set;} = Color.Transparent;

        private string _buffer = "";

        public bool IsReadOnly{get;set;} = false;

        private Dictionary<string,string> _parse = new Dictionary<string, string>()
                                                                                    {
                                                                                        {"NumPad1", "1"},
                                                                                        {"NumPad2", "2"},
                                                                                        {"NumPad3", "3"},
                                                                                        {"NumPad4", "4"},
                                                                                        {"NumPad5", "5"},
                                                                                        {"NumPad6", "6"},
                                                                                        {"NumPad7", "7"},
                                                                                        {"NumPad8", "8"},
                                                                                        {"NumPad9", "9"},
                                                                                        {"NumPad0", "0"},
                                                                                        {"D1", "1"},
                                                                                        {"D2", "2"},
                                                                                        {"D3", "3"},
                                                                                        {"D4", "4"},
                                                                                        {"D5", "5"},
                                                                                        {"D6", "6"},
                                                                                        {"D7", "7"},
                                                                                        {"D8", "8"},
                                                                                        {"D9", "9"},
                                                                                        {"Decimal", "."},
                                                                                        {"Add", "+"},
                                                                                        {"Subtract", "-"},
                                                                                        {"Multiply", "*"},
                                                                                        {"Divide", "/"},
                                                                                        {"Space", " "},
                                                                                        {"TAB", ""},
                                                                                        {"LeftShift", ""},
                                                                                        {"RightShift", ""},
                                                                                        {"Right", ""},
                                                                                        {"Left", ""},
                                                                                        {"Up", ""},
                                                                                        {"Down", ""},
                                                                                    };
        private Dictionary<string,Action> _specialParse = new Dictionary<string, Action>();

        public TextBox(Desktop _desktop) : base(_desktop)
        {
            _specialParse.Add("Back", Back);
            _specialParse.Add("Enter", Enter);
            _specialParse.Add(@"\n", Enter);



            _label = new Label(_desktop)
            {
                Text = "TextBox",
                FontColor = Color.Black
            };


            this.BorderThickness = 2;
            this.BorderColor = NotSelectedBorderColor;
            this.Size = new Vector2_Int(150,30);
            this.OnFocusChanged += FocusChanged;
        }

        private void FocusChanged()
        {
            BorderColor = HasFocus? SelectedBorderColor : NotSelectedBorderColor;
        }
        protected override void After_Invalidated()
        {
            Vector2_Int _center = this.Center - Vector2_Int.FromVec2(_label.FontBrush.HalfFontSize);

            _label.Position = new Vector2_Int((this.Position.X + 10), _center.Y);
        }
        protected override void After_Process()
        {
            Handle_Text_Input();

            _label.Process();

        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            _label.Render(_spritebatch);
        }


        private void Handle_Text_Input()
        {
            if(!HasFocus){return;}
            if(IsReadOnly){return;}

            _buffer = Input.Instance.InputString;

            foreach (var item in _parse)
            {
                if(_buffer.Contains(item.Key))
                {
                    _buffer = item.Value;
                }
            }

            foreach (var item in _specialParse)
            {
                if(_buffer.Contains(item.Key))
                {
                    item.Value.Invoke();
                }
            }

            Text += _buffer;
        }


        public void Back()
        {
            _buffer = "";

            if(Text.Length > 0)
            {
                Text = Text.Substring(0, Text.Length - 1);
            }
        }

        private void Enter()
        {
            _buffer = "";
            this._hasFocus = false;
            OnTextSubmitted?.Invoke(this, Text);
        }
    }

    internal class Cursor
    {
        //---currently this is not being used. Was intended to give the user visual feedback that they are editing the text in the textbox

        private int _count = 32;
        private bool _showCursor = true;
        public string CursorIcon{get; private set;} = "|";

        public void Update()
        {
            _count -= 1;

            if(_count <= 0)
            {
                _showCursor = !_showCursor;
                _count = 32;
            }

            if(_showCursor)
            {
                CursorIcon = "|";
            }
            else
            {
                CursorIcon = " ";
            }
        }
    }
}


