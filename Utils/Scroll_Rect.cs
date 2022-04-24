using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using EditorUI_DX.Controls;

namespace EditorUI_DX.Utils
{
    public class Scroll_Rect
    {
        protected Rectangle _parentSource;
        public Rectangle Rectangle
        {
            get
            {
                return _rect;
            }
        }
        protected Rectangle _rect;
        public float _scrollRate = 7f;

        public void Set(Rectangle _parentRect)
        {
            _parentSource = _parentRect;
            _rect = _parentRect;
        }
        public void Scroll(float _dir, IList<Element> _elements, Padding _padding)
        {
            Calc_Height(_elements, _padding);

            if(_rect.Height < _parentSource.Height){return;}



            /*if(_rect.Y > _parentSource.Y + _padding.Top)//---top limit
            {
                _rect.Y = _parentSource.Y + _padding.Top;
            }
            if(_rect.Bottom < _parentSource.Bottom)//---bottom limit
            {
                _rect.Y = (int)(_parentSource.Y + _parentSource.Height) - _rect.Height;
            }*/

            int _topLimit = _parentSource.Y + _padding.Top;
            int _bottomLimit = (int)(_parentSource.Y + _parentSource.Height) - _rect.Height;

            _rect.Y += (int)(_dir * _scrollRate);

            _rect.Y = Math.Clamp(_rect.Y, _bottomLimit, _topLimit);
        }
        protected virtual void Calc_Height(IList<Element> _elements, Padding _padding)
        {
            int _height = 0;

            foreach (var item in _elements)
            {
                _height += item.Size.Y + _padding.Top;
            }

            _rect.Height = _height;
        }
    }

    public class TreeView_ScrollRect
    {
        protected Rectangle _parentSource;
        public Rectangle Rectangle
        {
            get
            {
                return _rect;
            }
        }
        protected Rectangle _rect;
        public float _scrollRate = 7f;

        public void Set(Rectangle _parentRect)
        {
            _parentSource = _parentRect;
            _rect = _parentRect;
        }

        public void Scroll(float _dir, IList<TreeNode> _elements, Padding _padding)
        {
            Calc_Height(_elements, _padding);

            if (_rect.Height < _parentSource.Height) { return; }

            int _topLimit = _parentSource.Y + _padding.Top;
            int _bottomLimit = (int)(_parentSource.Y + _parentSource.Height) - _rect.Height;

            _rect.Y += (int)(_dir * _scrollRate);

            _rect.Y = Math.Clamp(_rect.Y, _bottomLimit, _topLimit);
        }
        protected virtual void Calc_Height(IList<TreeNode> _elements, Padding _padding)
        {
            int _height = 0;

            foreach (var Parent in _elements)
            {
                _height += Parent.Size.Y + _padding.Top;

                if (Parent.ShowChildren)
                {
                    foreach (var Child in Parent.Controls.Collecton)
                    {
                        _height += Child.Size.Y + _padding.Top
;
                    }
                }
            }

            _rect.Height = _height;
        }
    }


}
