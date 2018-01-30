using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Input
{
    public abstract class InputBinding
    {

        protected int inputCode;

        protected int indexHeld;
        protected int indexListened;

        public InputBinding(int inputCode)
        {

            this.inputCode = inputCode;
        }

        //protected abstract boolean codeMatch();
        internal int getCode()
        {
            return this.inputCode;
        }

        internal int getIndexHeld() { return indexHeld; }
        internal int getIndexListened() { return indexListened; }

        internal void setIndexHeld(int i) { indexHeld = i; }
        internal void setIndexListened(int i) { indexListened = i; }

        internal void shiftHeldIndex() { indexHeld--; }
        internal void shiftListenedIndex() { indexListened--; }

    }
}
