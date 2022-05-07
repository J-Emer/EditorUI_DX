using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EditorUI_DX.Brushes;
using EditorUI_DX.Controls;
using EditorUI_DX.Interfaces;
using EditorUI_DX.Utils;


namespace EditorUI_DX.Controls
{
    public class PropertyGrid : Control 
    {
        public event Action<object, PropertyInfo> OnPropertyValueChanged;
        private Control_Collection<Control> Controls = new Control_Collection<Control>();
        private Layout _layout;
        private Padding _padding;

        public object SelectedObject
        {
            get
            {
                return _selectedObject;
            }
        }
        private object _selectedObject;

        private string _fontName;

        public PropertyGrid(Desktop _desktop, string _fontName) : base(_desktop)
        {
            this._fontName = _fontName;
            _layout = new Vertical_Stretch_Layout();
            _padding = new Padding(5);

            this.Size = new Vector2_Int(400,400);
            this.BackgroundColor = new Color(57, 60, 64);

            Controls.OnControlsChanged += After_Invalidated;
        }

        public void Select_Object(object _obj)
        {
            _selectedObject = _obj;

            Controls.Clear();

            Label _label = new Label(this._desktop);
            _label.Text = _selectedObject.GetType().ToString();
            _label.Size = new Vector2_Int((int)_label.FontBrush.FontSize.X, 20);

            Controls.Add(_label);

            foreach (var item in SelectedObject.GetType().GetProperties())
            {
                if(item.PropertyType == typeof(string))
                {
                    Controls.Add(new String_Property_Grid_Item(this._desktop, _fontName, _selectedObject, item, ValueChangedCallBack));
                }
                if(item.PropertyType == typeof(int))
                {
                    Controls.Add(new Int_Property_Grid_Item(this._desktop, _fontName, _selectedObject, item, ValueChangedCallBack));
                }
                if(item.PropertyType == typeof(float))
                {
                    Controls.Add(new Float_Property_Grid_Item(this._desktop, _fontName, _selectedObject, item, ValueChangedCallBack));
                }
                if(item.PropertyType == typeof(double))
                {
                    Controls.Add(new Double_Property_Grid_Item(this._desktop, _fontName, _selectedObject, item, ValueChangedCallBack));
                }
                if(item.PropertyType == typeof(Vector2))
                {
                    Controls.Add(new Vector2_Property_Grid_Item(this._desktop, _fontName, _selectedObject, item, ValueChangedCallBack));
                }
                if(item.PropertyType == typeof(bool))
                {
                    Controls.Add(new Bool_Property_Grid_Item(this._desktop, _fontName, _selectedObject, item, ValueChangedCallBack));
                }
                if(item.PropertyType.IsGenericType && item.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    Controls.Add(new List_Property_Grid_Item(this._desktop, _fontName, _selectedObject, item, ValueChangedCallBack));
                }
                if(item.PropertyType.IsEnum)
                {
                    Controls.Add(new Enum_Property_Grid_Item(this._desktop, _fontName, _selectedObject, item, ValueChangedCallBack));                
                }
            }

            int _height = _padding.Top + _padding.Bottom;

            for (int i = 0; i < Controls.Collecton.Count; i++)
            {
                _height += Controls.Collecton[i].Size.Y + _padding.Top;
            }

            this.Size = new Vector2_Int(this.Size.X, _height);

        }
        protected override void After_Invalidated()
        {
            _layout.Handle_Layout(Controls.Controls, this.SourceRectangle, _padding);
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



        private void ValueChangedCallBack(PropertyInfo _info)
        {
            OnPropertyValueChanged?.Invoke(this, _info);
        }






    }

    public class Property_Grid_Item : Control
    {
        protected PropertyInfo _info;
        protected object _selectedObject;
        protected Action<PropertyInfo> _actionCallBack;

        protected Label _label;

        public Property_Grid_Item(Desktop _desktop, string _fontName, object _selectedObj, PropertyInfo _info, Action<PropertyInfo> _callback) : base(_desktop)
        {
            this._actionCallBack = _callback;
            this._info = _info;
            this._selectedObject = _selectedObj;

            _label = new Label(_desktop)
            {
                Text = _info.Name
            };

            BackgroundColor = Color.Transparent;
        }


    }
    public class String_Property_Grid_Item : Property_Grid_Item
    {
        private TextBox _textBox;

        public String_Property_Grid_Item(Desktop _desktop, string _fontName, object _selectedObj, PropertyInfo _info, Action<PropertyInfo> _callback) : base(_desktop, _fontName, _selectedObj, _info, _callback)
        {

            _textBox = new TextBox(_desktop)
            {
                Text = _info.GetValue(_selectedObj).ToString()
            };

            _textBox.OnTextSubmitted += Text_Submitted;

            this.Size = new Vector2_Int(200, 20);
        }
        protected override void After_Invalidated()
        {
            _label.Position = this.Position;
            _label.Size = Vector2_Int.FromVec2(_label.FontBrush.FontSize);

            _textBox.Position = this.Position + new Vector2_Int(_label.Size.X + 10, 0);

            int _sX = (this.Size.X - _label.Size.X) - 10;

            _textBox.Size = new Vector2_Int(_sX, 20);
        }
        protected override void After_Process()
        {
            _label.Process();
            _textBox.Process();
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            _label.Render(_spritebatch);
            _textBox.Render(_spritebatch);
        }
        private void Text_Submitted(object sender, string text)
        {
            _info.SetValue(_selectedObject, _textBox.Text);
            _actionCallBack?.Invoke(this._info);
        }
    }
    public class Int_Property_Grid_Item : Property_Grid_Item
    {
        private TextBox _textBox;

        public Int_Property_Grid_Item(Desktop _desktop, string _fontName, object _selectedObj, PropertyInfo _info, Action<PropertyInfo> _callback) : base(_desktop, _fontName, _selectedObj, _info, _callback)
        {

            _textBox = new TextBox(_desktop)
            {
                Text = _info.GetValue(_selectedObj).ToString()
            };

            _textBox.OnTextSubmitted += Text_Submitted;

            this.Size = new Vector2_Int(200, 20);
        }
        protected override void After_Invalidated()
        {
            _label.Position = this.Position;
            _label.Size = Vector2_Int.FromVec2(_label.FontBrush.FontSize);

            _textBox.Position = this.Position + new Vector2_Int(_label.Size.X + 10, 0);

            int _sX = (this.Size.X - _label.Size.X) - 10;

            _textBox.Size = new Vector2_Int(_sX, 20);
        }
        protected override void After_Process()
        {
            _label.Process();
            _textBox.Process();
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            _label.Render(_spritebatch);
            _textBox.Render(_spritebatch);
        }
        private void Text_Submitted(object sender, string text)
        {
            _info.SetValue(_selectedObject, int.Parse(_textBox.Text));
            _actionCallBack?.Invoke(this._info);
        }
    }
    public class Float_Property_Grid_Item : Property_Grid_Item
    {
        private TextBox _textBox;

        public Float_Property_Grid_Item(Desktop _desktop, string _fontName, object _selectedObj, PropertyInfo _info, Action<PropertyInfo> _callback) : base(_desktop, _fontName, _selectedObj, _info, _callback)
        {

            _textBox = new TextBox(_desktop)
            {
                Text = _info.GetValue(_selectedObj).ToString()
            };

            _textBox.OnTextSubmitted += Text_Submitted;

            this.Size = new Vector2_Int(200, 20);
        }
        protected override void After_Invalidated()
        {
            _label.Position = this.Position;
            _label.Size = Vector2_Int.FromVec2(_label.FontBrush.FontSize);

            _textBox.Position = this.Position + new Vector2_Int(_label.Size.X + 10, 0);

            int _sX = (this.Size.X - _label.Size.X) - 10;

            _textBox.Size = new Vector2_Int(_sX, 20);
        }
        protected override void After_Process()
        {
            _label.Process();
            _textBox.Process();
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            _label.Render(_spritebatch);
            _textBox.Render(_spritebatch);
        }
        private void Text_Submitted(object sender, string text)
        {
            _info.SetValue(_selectedObject, float.Parse(_textBox.Text));
            _actionCallBack?.Invoke(this._info);
        }
    }
    public class Double_Property_Grid_Item : Property_Grid_Item
    {
        private TextBox _textBox;

        public Double_Property_Grid_Item(Desktop _desktop, string _fontName, object _selectedObj, PropertyInfo _info, Action<PropertyInfo> _callback) : base(_desktop, _fontName, _selectedObj, _info, _callback)
        {

            _textBox = new TextBox(_desktop)
            {
                Text = _info.GetValue(_selectedObj).ToString()
            };

            _textBox.OnTextSubmitted += Text_Submitted;

            this.Size = new Vector2_Int(200, 20);
        }
        protected override void After_Invalidated()
        {
            _label.Position = this.Position;
            _label.Size = Vector2_Int.FromVec2(_label.FontBrush.FontSize);

            _textBox.Position = this.Position + new Vector2_Int(_label.Size.X + 10, 0);

            int _sX = (this.Size.X - _label.Size.X) - 10;

            _textBox.Size = new Vector2_Int(_sX, 20);
        }
        protected override void After_Process()
        {
            _label.Process();
            _textBox.Process();
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            _label.Render(_spritebatch);
            _textBox.Render(_spritebatch);
        }
        private void Text_Submitted(object sender, string text)
        {
            _info.SetValue(_selectedObject, double.Parse(_textBox.Text));
            _actionCallBack?.Invoke(this._info);
        }
    }
    public class Vector2_Property_Grid_Item : Property_Grid_Item
    {
        private TextBox _xTextbox;
        private TextBox _yTextbox;


        public Vector2_Property_Grid_Item(Desktop _desktop, string _fontName, object _selectedObj, PropertyInfo _info, Action<PropertyInfo> _callback) : base(_desktop, _fontName, _selectedObj, _info, _callback)
        {
            Vector2 _vec = (Vector2)_info.GetValue(_selectedObj);

            _xTextbox = new TextBox(_desktop)
            {
                Text = _vec.X.ToString()
            };
            _yTextbox = new TextBox(_desktop)
            {
                Text = _vec.Y.ToString()
            };

            _xTextbox.OnTextSubmitted += TextSubmitted;
            _yTextbox.OnTextSubmitted += TextSubmitted;

            this.Size  = new Vector2_Int(200, 20);
        }
        protected override void After_Invalidated()
        {
            _label.Position = this.Position;
            _label.Size = Vector2_Int.FromVec2(_label.FontBrush.FontSize);

            int _remainingWidth = this.Size.X - _label.Size.X;
            int _halfremainingWidth = _remainingWidth / 2;

            _xTextbox.Position = this.Position + new Vector2_Int(_label.Size.X + 5, 0);
            _yTextbox.Position = this.Position + new Vector2_Int(_label.Size.X + _halfremainingWidth, 0);

            _xTextbox.Size = new Vector2_Int(_halfremainingWidth - 10, 20);
            _yTextbox.Size = new Vector2_Int(_halfremainingWidth, 20);
        }
        protected override void After_Process()
        {
            _label.Process();
            _xTextbox.Process();
            _yTextbox.Process();
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            _label.Render(_spritebatch);
            _xTextbox.Render(_spritebatch);
            _yTextbox.Render(_spritebatch);
        }
        private void TextSubmitted(object sender, string text)
        {
            float x = float.Parse(_xTextbox.Text);
            float y = float.Parse(_yTextbox.Text);

            _info.SetValue(_selectedObject, new Vector2(x, y));
            _actionCallBack?.Invoke(this._info);
        }
    }
    public class Bool_Property_Grid_Item : Property_Grid_Item
    {
        private Toggle_Button _toggle;

        public Bool_Property_Grid_Item(Desktop _desktop, string _fontName, object _selectedObj, PropertyInfo _info, Action<PropertyInfo> _callback) : base(_desktop, _fontName, _selectedObj, _info, _callback)
        {
            _toggle = new Toggle_Button(_desktop);
            _toggle.BackgroundColor = new Color(112, 118, 125);
            _toggle.SelectedColor = new Color(22, 24, 26);

            _toggle.Value = (bool)_info.GetValue(_selectedObj);

            _toggle.OnValueChanged += ValueChanged;

            this.Size = new Vector2_Int(200, 30);
        }
        protected override void After_Invalidated()
        {
            int _centerY = this.Center.Y - (int)_label.FontBrush.HalfFontSize.Y;
            _label.Position = new Vector2_Int(this.Position.X, _centerY);
            _label.Size = Vector2_Int.FromVec2(_label.FontBrush.FontSize);

            _toggle.Position = this.Position + new Vector2_Int(_label.Size.X + 10, 0);
            _toggle.Size = new Vector2_Int(_toggle.Size.X, this.Size.Y);
        }
        protected override void After_Process()
        {
            _label.Process();
            _toggle.Process();
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            _label.Render(_spritebatch);
            _toggle.Render(_spritebatch);
        }
        private void ValueChanged(object sender, bool value)
        {
            _info.SetValue(_selectedObject, _toggle.Value);
            _actionCallBack?.Invoke(this._info);
        }

    }
    public class List_Property_Grid_Item : Property_Grid_Item
    {
        private List<TextBox> _textBoxs = new List<TextBox>();
        private string _fontName; 

        public List_Property_Grid_Item(Desktop _desktop, string _fontName, object _selectedObj, PropertyInfo _info, Action<PropertyInfo> _callback) : base(_desktop, _fontName, _selectedObj, _info, _callback)
        {
            this._fontName = _fontName;

            System.Collections.IList b = (System.Collections.IList)_info.GetValue(_selectedObj, null);

            for (int i = 0; i < b.Count; i++)
            {
                TextBox _tb = new TextBox(_desktop)
                {
                    Text = b[i].ToString(),
                    Tag = i
                };

                _tb.OnTextSubmitted += Text_Submitted;

                _textBoxs.Add(_tb);
            }

            int _height = 5;

            for (int i = 0; i < _textBoxs.Count; i++)
            {
                _height += _textBoxs[i].Size.Y + 5;
            }

            this.Size = new Vector2_Int(200, _height);
        }
        protected override void After_Invalidated()
        {
            int _centerY = this.Center.Y - (int)_label.FontBrush.HalfFontSize.Y;
            _label.Position = this.Position + new Vector2_Int(5, 5);
            _label.Size = Vector2_Int.FromVec2(_label.FontBrush.FontSize);


            int height = this.Position.Y + 5;

            for (int i = 0; i < _textBoxs.Count; i++)
            {
                _textBoxs[i].Position = new Vector2_Int(_label.Size.X + 10 + this.Position.X, height);
                height += _textBoxs[i].Size.Y + 5;
            }
        }
        protected override void After_Process()
        {
            _label.Process();

            for (int i = 0; i < _textBoxs.Count; i++)
            {
                _textBoxs[i].Process();
            }
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            _label.Render(_spritebatch);

            for (int i = 0; i < _textBoxs.Count; i++)
            {
                _textBoxs[i].Render(_spritebatch);
            }
        }

        private void Text_Submitted(object sender, string text)
        {
            TextBox _tb = (TextBox)sender;
            int id = (int)_tb.Tag;

            System.Collections.IList b = (System.Collections.IList)_info.GetValue(_selectedObject, null);

            b[id] = _tb.Text;

            _actionCallBack?.Invoke(this._info);
        }
    }
    public class Enum_Property_Grid_Item : Property_Grid_Item
    {
        private ComboBox _combo;

        public Enum_Property_Grid_Item(Desktop _desktop, string _fontName, object _selectedObj, PropertyInfo _info, Action<PropertyInfo> _callback) : base(_desktop, _fontName, _selectedObj, _info, _callback)
        {

            _combo = new ComboBox(_desktop);
            _combo.OnItemSelected += Combo_Item_Selected;

            foreach (FieldInfo fInfo in _info.PropertyType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                int _id = (int)fInfo.GetRawConstantValue();
                string _value = fInfo.GetValue(fInfo).ToString();

                Enum_Helper _helper = new Enum_Helper()
                {
                    ID = _id,
                    Value = _value
                };

                _combo.Add(_value, _helper);

                this.Size = new Vector2_Int(200, 30);
            }


        }
        protected override void After_Invalidated()
        {
            int _centerY = this.Center.Y - (int)_label.FontBrush.HalfFontSize.Y;
            _label.Position = new Vector2_Int(this.Position.X, _centerY);
            _label.Size = Vector2_Int.FromVec2(_label.FontBrush.FontSize);

            _combo.Position = this.Position + new Vector2_Int(_label.Size.X + 10, 0);
        }
        protected override void After_Process()
        {
            _label.Process();
            _combo.Process();
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            _label.Render(_spritebatch);
            _combo.Render(_spritebatch);
        }
        private void Combo_Item_Selected(object sender, ListBoxItem item)
        {
            Console.WriteLine(item.Tag);

            Enum_Helper _helper = (Enum_Helper)item.Tag;

            _info.SetValue(_selectedObject, _helper.ID);
            _actionCallBack.Invoke(this._info);
        }
    }

    internal class Enum_Helper
    {
        public int ID{get;set;}
        public string Value{get;set;}



        public override string ToString()
        {
            return $"ID: {ID} | Value: {Value}";
        }
    }
}


