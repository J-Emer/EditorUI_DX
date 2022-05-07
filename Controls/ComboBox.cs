using System;
using Microsoft.Xna.Framework.Graphics;
using EditorUI_DX.Utils;



namespace EditorUI_DX.Controls
{
    public class ComboBox : Control
    {
        /// <summary>
        /// Fires when an item has been selected from the ComboBox drop down 
        /// </summary>
        public event Action<object, ListBoxItem> OnItemSelected;
        public ListBoxItem SelectedItem{get; private set;}


        private string _fontName;
        
        private TextBox _textBox;
        private Button _button;
        private ListBox _listBox;
        private bool _isExpanded = true;
        
        
        public ComboBox(Desktop _desktop, string _fontName) : base(_desktop)
        {
            this._fontName = _fontName;

            _textBox = new TextBox(_desktop)
            {
                Size = new Vector2_Int(150,25),
                IsReadOnly = false,
                Text = ""
            };

            _button = new Button(_desktop)
            {
                Text = "^"
            };

            _button.OnMouseDown += Button_MouseDown;

            _listBox = new ListBox(_desktop, this._desktop.DefaultFontName)
            {
                Position = this.Position,
            };
            _listBox.OnListBoxItemSelected += ListBoxItemSelected;
            _listBox.OnMouseExit += ListBoxMouseExit;

            this.Size = new Vector2_Int(200,150);

            Button_MouseDown(new EditorUI.Utils.MouseEventArgs());
        }

        /// <summary>
        /// Adds item to the Combobox
        /// </summary>
        /// <param name="_text">The text that will added to the Combobox dropdown</param>
        public void Add(string _text)
        {
            _listBox.Add(_text);
        }
        
        /// <summary>
        /// Adds item to the Combobox
        /// </summary>
        /// <param name="_text">The text that will added to the Combobox dropdown</param>
        /// <param name="_tag">A user defined piece of data</param>
        /// <returns>TreeNode</returns>
        public void Add(string _text, object _tag)
        {
            _listBox.Add(_text, _tag);
        }
        public void Remove(ListBoxItem _item)
        {
            _listBox.Remove(_item);
        }

        protected override void After_Invalidated()
        {
            int _rightMost = this.Position.X + this.Size.X;
            _button.Position = new Vector2_Int(_rightMost - 25, this.Position.Y);

            _button.Size = new Vector2_Int(25, 25);


            _textBox.Position = this.Position;
            int _textBoxSpaceonX = (_button.Position.X - this.Position.X) - 5;
            _textBox.Size = new Vector2_Int(_textBoxSpaceonX, 25);
            
            _listBox.Position = this.Position + new Vector2_Int(0, 25);
            _listBox.Size = this.Size - new Vector2_Int(0, 25);
        }
        protected override void After_Process()
        {
            _textBox.Process();
            _button.Process();
            _listBox.Process();
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            _textBox.Render(_spritebatch);
            _button.Render(_spritebatch);
            _listBox.Render(_spritebatch);
        }

        private void Button_MouseDown(EditorUI.Utils.EventArgs e)
        {
            Toggle();
        }
        private void ListBoxItemSelected(object sender, ListBoxItem item)
        {
            SelectedItem = item;
            _textBox.Text = item.Text;
            OnItemSelected?.Invoke(this, SelectedItem);
            Toggle();
        }
        private void Toggle()
        {
            _isExpanded = !_isExpanded;
            _listBox.IsActive = _isExpanded;

            if(_isExpanded)
            {
                this.Size = new Vector2_Int(this.Size.X, 225);
                _button.Text = "^";
            }
            else
            {
                this.Size = new Vector2_Int(this.Size.X, 25);
                _button.Text = "v";
            }
        }
    
        private void ListBoxMouseExit()
        {
            if(_listBox.IsActive)
            {
                Toggle();
            }
        }
    }
}


