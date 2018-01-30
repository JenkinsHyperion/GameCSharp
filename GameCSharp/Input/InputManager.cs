using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.Engine;

namespace Game1.Input
{
    public abstract class InputManager
    {

        protected String name;


        protected  void removeFromListening<T>(T inputbinding, List<T> listeningList) where T : InputBinding
        {
            listeningList.RemoveAt(inputbinding.getIndexListened());

            for (int i = inputbinding.getIndexListened(); i < listeningList.Count; i++)
            {
                listeningList.[i].shiftListenedIndex();
            }
        }

        protected void removeFromHeld<T>(T inputbinding, List<T> heldList) where T : InputBinding
        {

            heldList.RemoveAt(inputbinding.getIndexHeld());

            for (int i = inputbinding.getIndexHeld(); i < heldList.Count; i++)
            {
                heldList.[i].shiftHeldIndex();
            }

        }

        public abstract void runHeld();

        public abstract void debugPrintInputList(int x, int y, Graphics2D g2);

        /*protected <T extends InputBinding> void releaseButton( T button, ArrayList<T> listeningList, ArrayList<T> heldList ){
            button.setIndexListened( listeningList.size() ); 
            listeningList.add( button );
            removeFromHeld( button, heldList);
            button.mouseReleased();
        }

        protected <T extends InputBinding> void pressButton( T button, ArrayList<T> heldList, ArrayList<T> listeningList ){
            button.setIndexHeld( heldList.size() ); // add this key to keys being held
            heldList.add( button );
            removeFromListening( button, listeningList ); // stop listening for this key while it's being held
            button.mousePressed(); // trigger pressed event for that key

        }*/

    }
}
