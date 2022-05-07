using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EditorUI_DX.Utils;


namespace EditorUI_DX.Controls
{
    public class Widget : Control
    {
        public string Title
        {
            get
            {
                return _header.Title;
            }
            set
            {
                _header.Title = value;
            }
        }

        private Right_Grabber _rightGrabber;
        private Bottom_Grabber _bottomGrabber;
        public int GrabHandle_Size{get;set;} = 20;


        public Control_Collection<Control> Controls
        {
            get
            {
                return _panel.Controls;
            }
        }
        public Padding Padding
        {
            get
            {
                return _panel.Padding;
            }
            set
            {
                _panel.Padding = value;
            }
        }
        public Layout Layout
        {
            get
            {
                return _panel.Layout;
            }
            set
            {
                _panel.Layout = value;
            }
        }
        private Widget_Header _header;
        private Panel _panel;



        public Widget(Desktop _desktop, string _title) : base(_desktop)
        {
            _header = new Widget_Header(_desktop, this, _title);
            _panel = new Panel(_desktop)
            {
                BackgroundColor = new Color(57, 60, 64)
            };

            _rightGrabber = new Right_Grabber();
            _bottomGrabber = new Bottom_Grabber();

            _desktop.Controls.Add(this);
        }
        protected override void After_Invalidated()
        {
            _header.Position = this.Position;
            _header.Size = new Vector2_Int(this.Size.X, 40);

            _panel.Position = this.Position + new Vector2_Int(0, 40);
            _panel.Size = this.Size - new Vector2_Int(0, 40);

            _rightGrabber._rect = new Rectangle(SourceRectangle.Right, SourceRectangle.Top, GrabHandle_Size, SourceRectangle.Height);
            _bottomGrabber._rect = new Rectangle(SourceRectangle.X, SourceRectangle.Bottom, SourceRectangle.Width, GrabHandle_Size);
        }
        protected override void After_Process()
        {
            _header.Process();
            _panel.Process();
            
            _rightGrabber.Update(this);
            _bottomGrabber.Update(this);

        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            _header.Render(_spritebatch);
            _panel.Render(_spritebatch);
        }
        protected virtual void Before_Close(){}

        public void Close()
        {
            Before_Close();
            this._desktop.Controls.Remove(this);
        }
    }

    internal class Widget_Header : Control
    {
        public string Title
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
        private Label _label;
        private Button _button;

        private bool _canMove = false;
        private Widget _parentWidget;
        private Vector2_Int _offset;

        public Widget_Header(Desktop _desktop, Widget _widget, string _title) : base(_desktop)
        {
            this._parentWidget = _widget;

            _label = new Label(_desktop)
            {
                Text = _title,
                FontColor = Color.White
            };

            _button = new Button(_desktop)
            {
                Position = Vector2_Int.Zero,
                Size = new Vector2_Int(40,40),
                BackgroundColor = Color.Transparent,
                Text = "X",
                FontColor = Color.White,
                NormalColor = Color.Transparent,
                HighlightColor = Color.Red
            };

            _button.OnMouseDown += CloseButton_MouseDown;

            this.BackgroundColor = new Color(79, 84, 89);


            this.OnMouseDown += Header_MouseDown;
            this.OnMouseUp += Header_OnMouseUp;
        }
        protected override void After_Invalidated()
        {
            _label.Position = this.Center - Vector2_Int.FromVec2(_label.FontBrush.HalfFontSize);
            _label.IsActive = _label.FontBrush.FontSize.X < this.Size.X && _label.FontBrush.FontSize.Y < this.Size.Y;//---toggles Label.IsActive if the Lable.FontSize is greater than this.SourceRectangle
        
        
            _button.Position = new Vector2_Int(_parentWidget.SourceRectangle.Right - 40, _parentWidget.SourceRectangle.Top);
        }
        protected override void After_Process()
        {
            if(_canMove)
            {
                _parentWidget.Position = Vector2_Int.FromVec2(Input.Instance.MousePosition) + _offset;
            }

            _label.Process();
            _button.Process();
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            _label.Render(_spritebatch);
            _button.Render(_spritebatch);
        }
        private void Header_MouseDown(EventArgs e)
        {
            _offset = this.Position - Vector2_Int.FromVec2(Input.Instance.MousePosition);
            _canMove = true;
        }
        private void Header_OnMouseUp(EventArgs e)
        {
            _canMove = false;
        }
        private void CloseButton_MouseDown(EventArgs e)
        {
            _desktop.Controls.Remove(_parentWidget);
        }
    }
}


