
namespace EditorUI_DX.Utils
{
    public struct Padding
    {
        public int Left;
        public int Right;
        public int Top;
        public int Bottom;

        public Padding(int _all)
        {
            Left = _all;
            Right = _all;
            Top = _all;
            Bottom = _all;
        }
        public Padding(int _left, int _right, int _top, int _bottom)
        {
            Left = _left;
            Right = _right;
            Bottom = _bottom;
            Top = _top;
        }
    }
}
