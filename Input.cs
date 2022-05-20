using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EditorUI_DX
{
    public  class Input
    {

        private GameWindow Window;
        private  KeyboardState pKeys;
        private  KeyboardState cKeys;

        private  MouseState pMouse;
        private  MouseState cMouse;

        public  string InputString;

        public  Vector2 MousePosition
        {
            get
            {
                return new Vector2(cMouse.Position.X, cMouse.Position.Y);
            }
        }

        private  int pScroll;
        private  int cScroll;


        /// <summary>
        /// Returns a float between -1, 1 for the x Axis (A,D keys)
        /// </summary>
        public  float xAxis;

        /// <summary>
        /// Returns a float between -1, 1 for the y Axis (W,S keys)
        /// </summary>
        public  float yAxis;


        public Input(GameWindow _window)
        {
            this.Window = _window;
        }


        public  void Update()
        {
            pKeys = cKeys;
            cKeys = Keyboard.GetState();

            //InputString = cKeys.GetPressedKeys().ToString();

            pMouse = cMouse;
            cMouse = Mouse.GetState(this.Window);

            pScroll = cScroll;
            cScroll = cMouse.ScrollWheelValue;

            InputString = "";

            foreach (var item in cKeys.GetPressedKeys())
            {
                if (GetKeyDown(item))
                {
                    InputString += item.ToString();
                }
            }

            HandleAxis();
        }

        private  void HandleAxis()
        {
            if (GetKey(Keys.W) || GetKey(Keys.Up))
            {
                yAxis -= 1;
            }
            else if (GetKey(Keys.S) || GetKey(Keys.Down))
            {
                yAxis += 1;
            }
            else
            {
                yAxis = 0f;
            }



            if (GetKey(Keys.A) || GetKey(Keys.Left))
            {
                xAxis -= 1;
            }
            else if (GetKey(Keys.D) || GetKey(Keys.Right))
            {
                xAxis += 1;
            }
            else
            {
                xAxis = 0f;
            }

            xAxis = MathHelper.Clamp(xAxis, -1f, 1f);
            yAxis = MathHelper.Clamp(yAxis, -1f, 1f);
        }

        /// <summary>
        /// Returns true/false if a key is currently being pressed down
        /// </summary>
        /// <param name="_key"></param>
        /// <returns></returns>
        public  bool GetKey(Keys _key)
        {
            if (cKeys.IsKeyDown(_key))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true/false if key is currently being pressed this frame
        /// </summary>
        /// <param name="_key"></param>
        /// <returns></returns>
        public  bool GetKeyDown(Keys _key)
        {
            if (cKeys.IsKeyDown(_key) && (!pKeys.IsKeyDown(_key)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true/false if a key is currently being released this frame
        /// </summary>
        /// <param name="_key"></param>
        /// <returns></returns>
        public  bool GetKeyUp(Keys _key)
        {
            if (cKeys.IsKeyUp(_key) && (!pKeys.IsKeyUp(_key)))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Returns true/false if a mouse button is being held down 
        /// LMB = 0, RMB = 1, MMB = 2
        /// </summary>
        /// <param name="_index"></param>
        /// <returns></returns>
        public  bool GetMouseButton(int _index)
        {
            if (cMouse.LeftButton == ButtonState.Pressed && pMouse.LeftButton == ButtonState.Pressed && _index == 0)
            {
                return true;
            }

            if (cMouse.RightButton == ButtonState.Pressed && pMouse.RightButton == ButtonState.Pressed && _index == 1)
            {
                return true;
            }

            if (cMouse.MiddleButton == ButtonState.Pressed && pMouse.MiddleButton == ButtonState.Pressed && _index == 2)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true/false if a mouse button is being pressed this frame 
        /// LMB = 0, RMB = 1, MMB = 2
        /// </summary>
        /// <param name="_index"></param>
        /// <returns></returns>
        public  bool GetMouseButtonDown(int _index)
        {
            if (cMouse.LeftButton == ButtonState.Pressed && pMouse.LeftButton != ButtonState.Pressed && _index == 0)
            {
                return true;
            }

            if (cMouse.RightButton == ButtonState.Pressed && pMouse.RightButton != ButtonState.Pressed && _index == 1)
            {
                return true;
            }

            if (cMouse.MiddleButton == ButtonState.Pressed && pMouse.MiddleButton != ButtonState.Pressed && _index == 2)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true/false if a mouse button is being released this frame 
        /// LMB = 0, RMB = 1, MMB = 2
        /// </summary>
        /// <param name="_index"></param>
        /// <returns></returns>
        public  bool GetMouseButtonUp(int _index)
        {
            if (cMouse.LeftButton == ButtonState.Released && pMouse.LeftButton != ButtonState.Released && _index == 0)
            {
                return true;
            }

            if (cMouse.RightButton == ButtonState.Released && pMouse.RightButton != ButtonState.Released && _index == 1)
            {
                return true;
            }

            if (cMouse.MiddleButton == ButtonState.Released && pMouse.MiddleButton != ButtonState.Released && _index == 2)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a float between -1 and 1 based on scroll wheel scrolling
        /// </summary>
        /// <returns></returns>
        public  float ScrollWheel()
        {
            if (cScroll < pScroll)
            {
                return -1;
            }

            if (cScroll > pScroll)
            {
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Returns the mouse as a rectangle in screen space
        /// </summary>
        /// <returns></returns>
        public  Rectangle GetMouseRect()
        {
            return new Rectangle((int)MousePosition.X, (int)MousePosition.Y, 1, 1);
        }


        public  string MousePos_String()
        {
            return MousePosition.ToString();
        }

    }
}
