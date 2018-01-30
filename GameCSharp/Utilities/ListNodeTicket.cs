using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Utilities
{
    public class ListNodeTicket<T>
    {

        DoubleLinkedList<T>.LinkedNodeElement node;

        //protected ListNodeTicket( LinkedNodeElement<T> node ){
        //	this.node = node;
        //}
        protected ListNodeTicket()
        {

        }

        public ListNodeTicket(DoubleLinkedList<T>.LinkedNodeElement newElement)
        {
            this.node = newElement;
        }

        public void removeSelfFromList()
        {
            node.removeSelf();
        }

        public bool isActive()
        {
            return true;
        }

    }
}
