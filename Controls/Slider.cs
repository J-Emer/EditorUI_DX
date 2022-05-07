using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EditorUI_DX.Brushes;
using EditorUI_DX.Utils;



namespace EditorUI_DX.Controls
{
    public class Slider : Control 
    {
        private int _leftLimit
            {
                get
                {
                    return Position.X + 30;
                }
            }
        private int _rightLimit
            {
                get
                {
                    return (this.Position.X + this.Size.X) - 60;
                }
            }
        public float Value
            {
                get
                {
                    return _value;
                }
            }
        private float _value = 0;
        private Rectangle sliderHandle;
        private Solid_Brush sliderBrush;
        private bool dragSlider = false;
        public Color SliderHandleColor
            {
                get
                {
                    return sliderBrush.DrawColor;
                }
                set
                {
                    sliderBrush.DrawColor = value;
                }
            }

        /// <summary>
        /// Fires when the slider handle is moved (value is changed)
        /// </summary>
        public event Action<object, float> OnValueChanged;


        public Slider(Desktop _desktop) : base(_desktop)
        {
            sliderBrush = new Solid_Brush(_desktop);
            sliderBrush.DrawColor = Color.Black;

            sliderHandle = new Rectangle(0,0,20,20);
            
            this.BackgroundColor = new Color(57, 60, 64);
            this.Size = new Vector2_Int(300,40);
        }
        protected override void After_Invalidated()
        {
            //todo: slider handle needs to be positioned relative to the position of this control. When this control is on a Widget the SliderHandle stays in place as the widget moves around it
            sliderHandle.X = (int)Math.Clamp(sliderHandle.X, _leftLimit, _rightLimit);
            sliderHandle.Y = (int)(Center.Y - (sliderHandle.Height / 2f));
        }
        protected override void After_Process()
        {
            bool _contains = sliderHandle.Contains(Input.Instance.MousePosition);
            bool _lmb = Input.Instance.GetMouseButtonDown(0);
            
            if(_contains && _lmb)
            {
                dragSlider = true;
            }

            if(dragSlider)
            {
                sliderHandle.X = (int)Input.Instance.MousePosition.X;
                sliderHandle.X = (int)Math.Clamp(sliderHandle.X, _leftLimit, _rightLimit);
                Handle_Value();
            }

            if(Input.Instance.GetMouseButtonUp(0))
            {
                dragSlider = false;
            }
        }
        protected override void After_Render(SpriteBatch _spritebatch)
        {
            _spritebatch.Draw(sliderBrush.Texture, sliderHandle, sliderBrush.DrawColor);
        }
        private void Handle_Value()
        {
            float _rawValue = sliderHandle.X;
            _value = (_rawValue - _leftLimit) / (_rightLimit - _leftLimit);
            OnValueChanged?.Invoke(this, _value);
        }
    }
}


