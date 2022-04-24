using EditorUI_DX.Controls;
using EditorUI_DX.Utils;


namespace EditorUI_DX.Interfaces
{
    public interface IControl_Container<T> where T : Control
    {
        Control_Collection<T> Controls { get; set; }
    }
}
