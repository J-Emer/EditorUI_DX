using System;
using System.Collections.Generic;
using System.Text;

using EditorUI_DX;
using EditorUI_DX.Utils;

namespace EditorUI_DX.Controls
{
    public abstract class Control : Element
    {

        public event Action OnFocusChanged;
        public event Action OnMouseEnter;
        public event Action OnMouseHover;
        public event Action OnMouseExit;
        public event Action<MouseEventArgs> OnMouseDown;
        public event Action<MouseEventArgs> OnMouseUp;
        public event Action<object, float> OnScrollWheel;

        public ContextMenu ContextMenu;

        private bool _pCursor;
        private bool _cCursor;


        public Control(Desktop _desktop) : base(_desktop) { }

        public void Process()
        {
            if (!IsActive) { return; }

            _pCursor = _cCursor;
            _cCursor = SourceRectangle.Contains(Input.Instance.MousePosition);

            bool _lmb_Down = Input.Instance.GetMouseButtonDown(0);
            bool _lmb_UP = Input.Instance.GetMouseButtonUp(0);

            bool _rmb_Down = Input.Instance.GetMouseButtonDown(1);
            bool _rmb_Up = Input.Instance.GetMouseButtonUp(1);

            //---------Enter/Exit/Hover----------//
            if (_pCursor && _cCursor) { OnMouseHover?.Invoke(); }
            if (!_pCursor && _cCursor) { OnMouseEnter?.Invoke(); }
            if (_pCursor && !_cCursor) { OnMouseExit?.Invoke(); }


            //---------Mouse Down / Up----------// (Left Mouse Button Only)
            if (_cCursor)
            {
                if (_lmb_Down && !_lmb_UP)
                {
                    _hasFocus = true;
                    OnFocusChanged?.Invoke();
                    OnMouseDown?.Invoke(new MouseEventArgs
                                                            {
                                                                MouseButton = MouseButtons.Left,
                                                                MouseButtonState = MouseButtonState.Down,
                                                                MousePosition = Input.Instance.MousePosition
                                                            });
                }
                if (_lmb_UP)
                {
                    OnMouseUp?.Invoke(new MouseEventArgs
                                                        {
                                                            MouseButton = MouseButtons.Left,
                                                            MouseButtonState = MouseButtonState.Up,
                                                            MousePosition = Input.Instance.MousePosition
                                                        });
                }
                if(_rmb_Down && !_rmb_Up)
                {
                    _hasFocus = true;
                    OnFocusChanged?.Invoke();
                    OnMouseDown?.Invoke(new MouseEventArgs
                                                            {
                                                                MouseButton = MouseButtons.Right,
                                                                MouseButtonState = MouseButtonState.Down,
                                                                MousePosition = Input.Instance.MousePosition
                                                            });

                    if(ContextMenu != null)
                    {
                        ContextMenu.IsActive = true;
                        ContextMenu.Position = Utils.Vector2_Int.FromVec2(Input.Instance.MousePosition);
                    }
                }
                if(_rmb_Up)
                {
                    OnMouseUp?.Invoke(new MouseEventArgs
                                                        {
                                                            MouseButton = MouseButtons.Right,
                                                            MouseButtonState = MouseButtonState.Up,
                                                            MousePosition = Input.Instance.MousePosition
                                                        });
                }
            }

            if (!_cCursor && _lmb_Down)
            {
                _hasFocus = false;
                OnFocusChanged?.Invoke();
            }

            //---------Scroll----------//
            if (Input.Instance.ScrollWheel() != 0)
            {
                if (_cCursor)
                {
                    OnScrollWheel?.Invoke(this, Input.Instance.ScrollWheel());
                }
            }

            After_Process();

        }
        protected virtual void After_Process() { }

    }
}
