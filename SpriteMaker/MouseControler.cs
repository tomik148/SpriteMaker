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
            Holding
        }
          
        State previous = State.Up;
        State current = State.Up;
        static MouseControler _instance;

        public event MouseEventHandler pressed;
        public event MouseEventHandler released;
        public event MouseEventHandler holding;

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
                case State.Up:
                    if(previous == State.Down || previous == State.Holding)
                    {
                        buttonReleased(sender, e);
                    }
                    current = State.Up;
                    break;
                default:
                    break;
            }
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

    }
}
