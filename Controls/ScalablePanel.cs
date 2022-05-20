using System;
using System.Windows.Forms;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using EditorUI_DX.Brushes;
using EditorUI_DX.Interfaces;
using EditorUI_DX.Utils;



namespace EditorUI_DX.Controls
{
    public class ScalablePanel : Control, IControl_Container<Control>, IDragDrop_Container
    {
        public bool ShowHandles { get; set; } = false;
        public Control_Collection<Control> Controls { get; set; } = new Control_Collection<Control>();
        public Layout Layout;
        public EditorUI_DX.Utils.Padding Padding;
        private Right_Grabber _rightGrabber;
        private Bottom_Grabber _bottomGrabber;
        private Left_Grabber _leftGrabber;
        private Top_Grabber _topGrabber;



        public int GrabHandle_Size { get; set; } = 20;

        private Solid_Brush _sharedGrabberBrush;


        public event Action<object, DragEventArgs> OnDragDrop;




        public ScalablePanel(Desktop _desktop) : base(_desktop)
        {
            _sharedGrabberBrush = new Solid_Brush(_desktop);

            Padding = new EditorUI_DX.Utils.Padding(0);
            Layout = new Layout();

            this.BackgroundColor = new Color(57, 60, 64);

            _leftGrabber = new Left_Grabber(this._desktop);
            _rightGrabber = new Right_Grabber(this._desktop);
            _bottomGrabber = new Bottom_Grabber(this._desktop);
            _topGrabber = new Top_Grabber(this._desktop);

            _rightGrabber.AfterRelease += After_Release;
            _bottomGrabber.AfterRelease += After_Release;


            this.Controls.OnControlsChanged += After_Invalidated;

            this.OnDockStyleChanged += DockStyleChanged;

            _desktop.OnDragDrop += Internal_DragDrop;
        }

        ~ScalablePanel()
        {
            _desktop.OnDragDrop -= Internal_DragDrop;
        }

        private void Internal_DragDrop(object sender, DragEventArgs e)
        {
            if(SourceRectangle.Contains(this._desktop.Input.MousePosition))
            {
                OnDragDrop?.Invoke(sender, e);
            }
        }

        private void DockStyleChanged(Element _element, EditorUI_DX.Utils.DockStyle _dockStyle)
        {
            if (this.DockStyle == EditorUI_DX.Utils.DockStyle.Left)
            {
                _leftGrabber.IsActive = false;
                _topGrabber.IsActive = false;
                _bottomGrabber.IsActive = false;
                _rightGrabber.IsActive = true;
            }
            if (this.DockStyle == EditorUI_DX.Utils.DockStyle.Right)
            {
                _leftGrabber.IsActive = true;
                _topGrabber.IsActive = false;
                _bottomGrabber.IsActive = false;
                _rightGrabber.IsActive = false;
            }
            if (this.DockStyle == EditorUI_DX.Utils.DockStyle.Top)
            {
                _leftGrabber.IsActive = false;
                _topGrabber.IsActive = false;
                _bottomGrabber.IsActive = true;
                _rightGrabber.IsActive = false;
            }
            if (this.DockStyle == EditorUI_DX.Utils.DockStyle.Bottom)
            {
                _leftGrabber.IsActive = false;
                _topGrabber.IsActive = true;
                _bottomGrabber.IsActive = false;
                _rightGrabber.IsActive = false;
            }
            if (this.DockStyle == EditorUI_DX.Utils.DockStyle.Center)
            {
                _leftGrabber.IsActive = false;
                _topGrabber.IsActive = false;
                _bottomGrabber.IsActive = false;
                _rightGrabber.IsActive = false;
            }
            if (this.DockStyle == EditorUI_DX.Utils.DockStyle.None)
            {
                _leftGrabber.IsActive = true;
                _topGrabber.IsActive = true;
                _bottomGrabber.IsActive = true;
                _rightGrabber.IsActive = true;
            }
        }

        protected override void After_Process()
        {
            _leftGrabber.Update(this);
            _rightGrabber.Update(this);
            _bottomGrabber.Update(this);
            _topGrabber.Update(this);

            for (int i = 0; i < Controls.Collecton.Count; i++)
            {
                Controls.Collecton[i].IsActive = SourceRectangle.Contains(Controls.Collecton[i].SourceRectangle);
                Controls.Collecton[i].Process();
            }
        }

        protected override void After_Invalidated()
        {
            _leftGrabber._rect = new Rectangle((SourceRectangle.X - GrabHandle_Size), SourceRectangle.Y, 20, SourceRectangle.Height);
            _rightGrabber._rect = new Rectangle(SourceRectangle.Right, SourceRectangle.Top, GrabHandle_Size, SourceRectangle.Height);
            _bottomGrabber._rect = new Rectangle(SourceRectangle.X, SourceRectangle.Bottom, SourceRectangle.Width, GrabHandle_Size);
            _topGrabber._rect = new Rectangle(this.Position.X, (this.Position.Y - GrabHandle_Size), this.Size.X, 20);

            Layout.Handle_Layout(Controls.Controls, this.SourceRectangle, Padding);
        }

        protected override void After_Render(SpriteBatch _spritebatch)
        {
            for (int i = 0; i < Controls.Collecton.Count; i++)
            {
                Controls.Collecton[i].Render(_spritebatch);
            }

            if (ShowHandles)
            {
                _spritebatch.Draw(_sharedGrabberBrush.Texture, _rightGrabber._rect, _rightGrabber.DrawColor);
                _spritebatch.Draw(_sharedGrabberBrush.Texture, _bottomGrabber._rect, _bottomGrabber.DrawColor);
                _spritebatch.Draw(_sharedGrabberBrush.Texture, _leftGrabber._rect, _leftGrabber.DrawColor);
                _spritebatch.Draw(_sharedGrabberBrush.Texture, _topGrabber._rect, _topGrabber.DrawColor);
            }
        }

        private void After_Release()
        {
            _desktop.Resize(this, null);
        }
    }

    internal abstract class Grabber
    {
        public bool IsActive { get; set; } = true;
        public Rectangle _rect;
        protected Vector2_Int _offset;
        private bool _isGrabbed = false;

        public Color DrawColor = Color.White;

        public event Action AfterRelease;

        private bool _lastContains = false;
        private bool _contains = false;

        protected Desktop _desktop;

        public Grabber(Desktop _desktop)
        {
            this._desktop = _desktop;
        }

        public void Update(Element _element)
        {
            if (!IsActive) { return; }

            _lastContains = _contains;
            _contains = _rect.Contains(this._desktop.Input.MousePosition);

            if (_contains)
            {
                Mouse.SetCursor(MouseCursor.Hand);
            }

            if (this._desktop.Input.GetMouseButtonDown(0) && _contains)
            {
                Start_Grab(_element);
            }

            if (_isGrabbed)
            {
                Handle_Grab(_element);
            }

            if (this._desktop.Input.GetMouseButtonUp(0))
            {
                _isGrabbed = false;
                DrawColor = Color.White;
                Mouse.SetCursor(MouseCursor.Arrow);
                AfterRelease?.Invoke();
            }

            if (!_contains && _lastContains)
            {
                Mouse.SetCursor(MouseCursor.Arrow);
            }
        }

        protected virtual void Start_Grab(Element _element)
        {
            _isGrabbed = true;
            //_offset = Vector2_Int.FromVec2(Input.MousePosition) - new Vector2_Int(_element.SourceRectangle.Right, _element.SourceRectangle.Top);
            _offset = Vector2_Int.FromVec2(this._desktop.Input.MousePosition) - new Vector2_Int(_rect.Center.X, _rect.Center.Y);
            DrawColor = Color.Yellow;
        }
        public virtual void Handle_Grab(Element _element) { }
    }
    internal class Right_Grabber : Grabber
    {
        public Right_Grabber(Desktop _desktop) : base(_desktop){}
        public override void Handle_Grab(Element _element)
        {
            int _x = (Vector2_Int.FromVec2(this._desktop.Input.MousePosition).X + _offset.X) - _element.Position.X;
            _element.Size = new Vector2_Int(_x, _element.Size.Y);
        }
    }
    internal class Bottom_Grabber : Grabber
    {
        public Bottom_Grabber(Desktop _desktop) : base(_desktop){}

        public override void Handle_Grab(Element _element)
        {
            int _y = (Vector2_Int.FromVec2(this._desktop.Input.MousePosition).Y + _offset.Y) - _element.Position.Y;
            _element.Size = new Vector2_Int(_element.Size.X, _y);
        }
    }
    internal class Left_Grabber : Grabber
    {
        //https://medium.com/the-z/making-a-resizable-div-in-js-is-not-easy-as-you-think-bda19a1bc53d
        //modified this code for my own needs

        int originalWidth;
        int originalMouseX;
        int originalX;

        public Left_Grabber(Desktop _desktop) : base(_desktop){}


        protected override void Start_Grab(Element _element)
        {
            base.Start_Grab(_element);

            originalWidth = _element.Size.X;
            originalMouseX = (int)this._desktop.Input.MousePosition.X;
            originalX = _element.Position.X;
        }
        public override void Handle_Grab(Element _element)
        {
            Vector2_Int _mousePos = Vector2_Int.FromVec2(this._desktop.Input.MousePosition);

            int _newWidth = originalWidth - (_mousePos.X - originalMouseX);
            int _newX = originalX + (_mousePos.X - originalMouseX);

            _element.Position = new Vector2_Int(_newX, _element.Position.Y);
            _element.Size = new Vector2_Int(_newWidth, _element.Size.Y);
        }
    }
    internal class Top_Grabber : Grabber
    {
        //https://medium.com/the-z/making-a-resizable-div-in-js-is-not-easy-as-you-think-bda19a1bc53d
        //modified this code for my own needs

        int originalHeight;
        int originalMouseY;
        int originalY;

        public Top_Grabber(Desktop _desktop) : base(_desktop){}


        protected override void Start_Grab(Element _element)
        {
            base.Start_Grab(_element);

            originalHeight = _element.Size.Y;
            originalMouseY = (int)this._desktop.Input.MousePosition.Y;
            originalY = _element.Position.Y;
        }
        public override void Handle_Grab(Element _element)
        {
            Vector2_Int _mousePos = Vector2_Int.FromVec2(this._desktop.Input.MousePosition);
            int newHeight = originalHeight - (_mousePos.Y - originalMouseY);
            int newY = originalY + (_mousePos.Y - originalMouseY);

            _element.Position = new Vector2_Int(_element.Position.X, newY);
            _element.Size = new Vector2_Int(_element.Size.X, newHeight);
        }
    }
}


