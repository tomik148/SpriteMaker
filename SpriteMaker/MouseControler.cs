using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpriteMaker
{
    class MouseControler
    {
        public enum State
        {
            Down,
            Up,
            Holding,
            Hovering
        }

        public enum SubState
        {
            Drawing,
            Selecting,
            Moving,
            Scaling
        }

        State previous = State.Up;
        State current = State.Up;

        SubState currentSubState = SubState.Drawing;

        Cursor currentCursor = Cursors.Cross;

        static MouseControler _instance;

        public event MouseEventHandler pressed;
        public event MouseEventHandler released;
        public event MouseEventHandler holding;
        public event MouseEventHandler hover;

        private MouseControler() { }
        static public MouseControler getInstance()
        {
            if(_instance == null)
            {
                _instance = new MouseControler();
            }
            return _instance;
        }

        public void Update(State s, object sender, MouseEventArgs e)
        {
            previous = current;
            switch(s)
            {
                case State.Holding:
                case State.Down:
                    if(previous == State.Down || previous == State.Holding)
                    {
                        current = State.Holding;
                        buttonHolding(sender, e);
                    }
                    else
                    {
                        current = State.Down;
                        buttonPressed(sender, e);
                    }
                    break;
                case State.Hovering:
                case State.Up:
                    if(previous == State.Down || previous == State.Holding)
                    {
                        current = State.Up;
                        buttonReleased(sender, e);
                    }
                    else
                    {
                        buttonHovering(sender, e);
                    }

                    break;
                default:
                    break;
            }
            Cursor.Current = currentCursor;
        }

        public void setSubState(SubState s)
        {
            if(currentSubState != s)
            {
                currentSubState = s;
                Console.WriteLine("setSubState -- changed to " + s.ToString());
            }
        }
        public SubState getSubState()
        {
            return currentSubState;
        }

        public void setCursor(Cursor c)
        {
            if(currentCursor != c)
            {
                currentCursor = c;
                //Console.WriteLine("setCursor -- changed to " + c.ToString());
            }
        }
        public Cursor getCursor()
        {
            return currentCursor;
        }

        private void buttonPressed(object sender, MouseEventArgs e)
        {
            pressed?.Invoke(sender, e);
            //Console.WriteLine("buttonPressed -- Invoked");
        }

        private void buttonReleased(object sender, MouseEventArgs e)
        {
            released?.Invoke(sender, e);
            //Console.WriteLine("buttonReleased -- Invoked");
        }

        private void buttonHolding(object sender, MouseEventArgs e)
        {
            holding?.Invoke(sender, e);
            //Console.WriteLine("buttonHolding -- Invoked");
        }

        private void buttonHovering(object sender, MouseEventArgs e)
        {
            hover?.Invoke(sender, e);
            //Console.WriteLine("buttonHolding -- Invoked");
        }

    }
}
