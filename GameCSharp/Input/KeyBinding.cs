using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Input
{
    public class KeyBinding : InputBinding
    {


        private KeyType type;

        private KeyCommand command;

        protected KeyBinding(int keyCode, KeyCommand command) : base(keyCode)
        {
            this.command = command;
            type = new SingleKeyMatch(keyCode);
        }

        protected KeyBinding(int modCode, int keyCode, KeyCommand command) : base(keyCode)
        {

            this.command = command;
            type = new ModdedKeyMatch(keyCode,modCode);
        }

        public void onPressed() { command.onPressed(); }  //POSSIBLE 
        public void onReleased() { command.onReleased(); }
        public void onHeld() { command.onHeld(); }

        internal bool keyMatch(KeyEvent e)
        {
            return (type.keyCodeMatches(e) && type.modCodeMatches(e));
        }

        public int getModCode()
        {
            return type.getModCode();
        }

        public int getKeyCode()
        {
            return this.inputCode;
        }

        internal bool modMatch(KeyEvent e)
        {
            return type.modCodeMatches(e);
        }

        internal abstract class KeyType
        {
            protected int ownerInputCode;

            internal abstract bool keyCodeMatches(KeyEvent e);
            internal abstract bool modCodeMatches(KeyEvent e);
            internal abstract int getModCode();
        }

        private class SingleKeyMatch : KeyType
        {

            internal SingleKeyMatch( int inputCode )
            {
                this.ownerInputCode = inputCode;
            }

            internal override bool keyCodeMatches(KeyEvent e)
            { 

                if (e.getKeyCode() == ownerInputCode)
                    return true;
                else
                    return false;
            }

            internal override bool modCodeMatches(KeyEvent e)
            {
                if (e.getModifiers() == 0)
                    return true;
                else
                    return false;
            }

            public String toString()
            {
                return "Key " + ownerInputCode;
            }

            internal override int getModCode()
            {
                return 0;
            }

    }

        private class ModdedKeyMatch : KeyType
        {

            private int modKeyCode;

            internal ModdedKeyMatch(int ownerInputCode , int modKeyCode)
            {
                this.ownerInputCode = ownerInputCode;
                this.modKeyCode = modKeyCode;
            }

            internal override bool keyCodeMatches(KeyEvent e)
            { //class

                if (e.getKeyCode() == ownerInputCode)
                    return true;
                else
                    return false;

            }

            internal override bool modCodeMatches(KeyEvent e)
            {
                if (e.getModifiers() == modKeyCode)
                    return true;
                else
                    return false;
            }

            public String toString()
            {
                return modKeyCode + " + Key " + ownerInputCode;
            }


            internal override int getModCode()
            {
                return modKeyCode;
            }
	
	    }
	
        public String toString()
        {
            return this.type.toString();
        }

        internal KeyCommand extractCommand()
        {
            return this.command;
        }
	
    }
}
