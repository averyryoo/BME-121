using System;

using MediCal;

namespace DataStructures
{
    partial class LinkedList
    {
        private class Node
        {
            public Node Next { get; set; }
            public Drug Data { get; private set; }

            public Node( Drug newData ) { Next = null; Data = newData; }
        }

        private Node Tail { get; set; }
        private Node Head { get; set; }
        public int Count { get; private set; }

        public LinkedList( ) { Tail = null; Head = null; Count = 0; }

        public void AddFirst( Drug newData )
        {
            if( newData == null ) throw new ArgumentNullException( );

            Node newNode = new Node( newData );
            Node oldHead = Head;

            if( Tail == null ) Tail = newNode;
            else newNode.Next = oldHead;
            Head = newNode;

            Count ++;
        }

        public void AddLast( Drug newData )
        {
            if( newData == null ) throw new ArgumentNullException( );

            Node newNode = new Node( newData );
            Node oldTail = Tail;

            if( Head == null ) Head = newNode;
            else oldTail.Next = newNode;
            Tail = newNode;

            Count ++;
        }

        public Drug[ ] ToArray( )
        {
            Drug[ ] result = new Drug[ Count ] ;

            int i = 0;
            Node currentNode = Head;
            while( currentNode != null )
            {
                result[ i ] = currentNode.Data;
                i ++;
                currentNode = currentNode.Next;
            }

            return result;
        }
    }
}
