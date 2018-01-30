using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Utilities
{
    public class DoubleLinkedList<T>
    {

        private LinkedHead head;
        private LinkedTail tail;
        private LinkedNodeElement currentItteration;
        private int listSize;

        public DoubleLinkedList()
        {
            head = new LinkedHead(this);
            tail = new LinkedTail(this);
            currentItteration = tail;
            this.listSize = 0;
        }

        public ListNodeTicket<T> add(T element)
        {

            try
            {
                LinkedNodeElement newElement = this.head.addElement(element,this);
                currentItteration = newElement;
                listSize++;
                return new ListNodeTicket<T>(newElement);
            }

            catch (InvalidCastException e)
            {
                //System.err.println("Attempted to add entity without Composite DoubleLinkedList.");
                return null;
            }

        }

        public void executeThrough()
        {

        }

        public T get()
        {
            LinkedNodeElement currentNode = this.currentItteration;

            this.currentItteration = this.currentItteration.getNextNode();

            return currentNode.getElement();
        }

        public bool hasNext()
        {
            return currentItteration.isNext();
        }

        public void getRemove()
        {

            LinkedNodeElement currentNode = this.currentItteration;

            this.currentItteration = this.currentItteration.getNextNode();

            currentNode.removeSelf();

        }

        private void setSize( int size)
        {
            this.listSize = size;
        }

        public int size()
        {
            return this.listSize;
        }

        internal class LinkedHead : LinkedNode
        {

            private LinkedTail tail; //useful having pointer to tail for end adding?

            public LinkedHead( DoubleLinkedList<T> list)
            {
                this.tail = new LinkedTail(list);
                this.setNextNode(tail);
                tail.setPreviousNode(this);
            }


            public LinkedNodeElement addElement(T element, DoubleLinkedList<T> list)
            {
                LinkedNodeElement newNode = new LinkedNodeElement(element, list);

                newNode.setNextNode(this.nextNode);     // Head     Add --> Tail^      order of these is important

                newNode.setPreviousNode(this);          // Head <-- Add     Tail

                this.nextNode.setPreviousNode(newNode); // Head     Add <-- Tail^

                this.nextNode = newNode;                // Head --> Add     Tail**   change this.nextNode from tail to add LAST

                return newNode;
            }
            
        }

        internal class LinkedTail : LinkedNodeElement{

            public LinkedTail(DoubleLinkedList<T> list) : base(list)
            {
                this.prevNode = list.head;
            }


            public override bool isNext()
            {
                ownerList.currentItteration = ownerList.head.getNextNode(); //RESET ITTERATOR BACK TO HEAD
                return false; //TERMINATE WHILE LOOP ITTERATING
            }

        }


        public abstract class LinkedNode
        { // make interface?

            protected LinkedNodeElement nextNode;
            protected LinkedNode prevNode;

            public void setPreviousNode(LinkedNode prevNode) { this.prevNode = prevNode; }
            public void setNextNode(LinkedNodeElement nextNode) { this.nextNode = nextNode; }
            public LinkedNode getPreviousNode() { return this.prevNode; }
            public LinkedNodeElement getNextNode() { return this.nextNode; }

        }

        public class LinkedNodeElement : LinkedNode{

            private T element;
            protected DoubleLinkedList<T> ownerList;

            public LinkedNodeElement( DoubleLinkedList<T> list)
            {
                this.ownerList = list;
            }

            public LinkedNodeElement(T element , DoubleLinkedList<T> list )
            {
                this.ownerList = list;
                this.element = element;
            }


            public T getElement()
            {
                return this.element;
            }

            virtual public bool isNext()
            {
                return true;
            }

            public void removeSelf()
            {
                this.prevNode.setNextNode(this.nextNode);
                this.nextNode.setPreviousNode(this.prevNode);
                --this.ownerList.listSize;
            }
            
        }

	    public void clear()
        {

            while (this.hasNext())
            {
                this.getRemove();
            }

            //this.head.nextNode = this.tail;
        }
	
	
    }
}
