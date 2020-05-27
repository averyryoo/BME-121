using System;
using System.Collections;
using System.Collections.Generic;
using static System.Console;

namespace DataStructures
{
    class LinkedList< TData > : ICollection< TData >
    {
        private class Node
        {
            public Node Next { get; set; }
            public TData Data { get; private set; }

            public Node( TData newData ) { Next = null; Data = newData; }
        }

        private Node Tail { get; set; }
        private Node Head { get; set; }
        public  int Count { get; private set; }

        public LinkedList( ) { Tail = null; Head = null; Count = 0; }

        // This is the simplest way to make the linked list implement the IEnumerable< T > interface
        // so the list itself may be the target of a foreach loop or use Linq.Enumerable extension methods.
        // We need this because the ICollection< T > interface requires the IEnumerable< T > interface.
        // Further, the generic IEnumerable< T > interface requires the nongeneric IEnumerable interface.
        // Both IEnumerable and IEnumerable< T > specify a 'GetEnumerator' method which returns
        // an IEnumerator or IEnumerator< T > object containing methods needed to access the current
        // element in a collection or move to the next element in a collection.
        // The 'yield return' statement returns such an enumerator object. The value it returns is the
        // current element and the code up to the next 'yield return' is the move to the next element.
        // You don't need to understand this material for BME 121.

        IEnumerator IEnumerable.GetEnumerator( ) { return ( ( IEnumerable< TData > ) this ).GetEnumerator( ); }
        IEnumerator< TData > IEnumerable< TData >.GetEnumerator( )
        {
            Node currentNode = Head;
            while( currentNode != null )
            {
                yield return currentNode.Data;
                currentNode = currentNode.Next;
            }
        }

        // Implement the ICollection< T > interface. // TO DO
		
		// establish properties
		int ICollection<TData>.Count {get;}
		bool ICollection<TData>.IsReadOnly {get;}
		
		
		// establish methods
		void ICollection<TData>.Add(TData item) 
        {
			AddLast(item);
		}
		
        void ICollection<TData>.Clear() 
        {
			Head = null;
			Tail = null;
			Count = 0;
		}
		
		
		bool ICollection<TData>.Contains(TData item) 
        {
			bool IsTarget(TData currentItem) 
			{
				if(currentItem.Equals(item)) 
				{
					return true;
				}
				return false; 
			}
			//return boolean value
			return Contains(IsTarget);
		}
		
		
        void ICollection<TData>.CopyTo(TData[] arr, int arrIndex) 
        {
			// initialize array
			TData[] newArr = ToArray(); 
			int counter = 0; 
			// copy values to new array
			for (int i = arrIndex; i < arr.Length - 1; i++)
			{
				arr [i] = newArr [counter]; 
				counter ++; 
			}
		}
        bool ICollection<TData>.Remove(TData item) 
        {
			bool IsTarget(TData currentItem) //IsTarget method
			{
				//return true if value is itemToBeRemoved 
				if(currentItem.Equals(item))
				{
					return true;
				}
				return false;
			}
			// either return true and remove item, or return false
			return RemoveFirst(IsTarget, out item); 
		}
        // Add a new TData object to the end of the linked list.

        public void AddLast( TData newData )
        {
            if( newData == null ) throw new ArgumentNullException( );

            Node newNode = new Node( newData );
            Node oldTail = Tail;

            if( Head == null ) Head = newNode;
            else oldTail.Next = newNode;
            Tail = newNode;

            Count ++;
        }

        // Determine whether the linked list contains a target TData object.

        public bool Contains( Predicate< TData > IsTarget )
        {
            Node currentNode = Head;
            while( currentNode != null )
            {
                if( IsTarget( currentNode.Data ) ) return true;

                currentNode = currentNode.Next;
            }

            return false;
        }

        // Remove from the linked list the first instance of a target TData object.

        public bool RemoveFirst( Predicate< TData > IsTarget, out TData removed )
        {
            Node previousNode = null;
            Node currentNode = Head;
            while( currentNode != null )
            {
                if( IsTarget( currentNode.Data ) )
                {
                    Node nextNode = currentNode.Next;
                    removed = currentNode.Data;

                    if( previousNode == null ) // delete at head
                    {
                        currentNode.Next = null;
                        Head = nextNode;
                    }
                    else if( nextNode == null ) // delete at tail
                    {
                        previousNode.Next = nextNode;
                        Tail = previousNode;
                    }
                    else if( Count == 1 ) // delete only element
                    {
                        Head = null;
                        Tail = null;
                    }
                    else // delete between two nodes
                    {
                        currentNode.Next = null;
                        previousNode.Next = nextNode;
                    }

                    Count --;
                    return true;
                }

                previousNode = currentNode;
                currentNode = currentNode.Next;
            }

            removed = default( TData );
            return false;
        }

        // Make an array from TData objects on the linked list.

        public TData[ ] ToArray( )
        {
            TData[ ] result = new TData[ Count ] ;

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

namespace Bme121
{
    static class Program
    {
        static void Main( )
        {
            DataStructures.LinkedList< int > intList = new DataStructures.LinkedList< int >( );

            // View the linked list of integers as a collection.

            ICollection< int > c = intList;

            Write( "New c:" );
            foreach( int i in c ) Write( " {0}", i );
            WriteLine( );

            // Test collection 'Add' method.

            c.Add( 3 );
            c.Add( 5 );
            c.Add( 1 );
            c.Add( 4 );

            Write( "After 'Add' c:" );
            foreach( int i in c ) Write( " {0}", i );
            WriteLine( );

            // Test collection 'Clear' method.

            c.Clear( );

            Write( "After 'Clear' c:" );
            foreach( int i in c ) Write( " {0}", i );
            WriteLine( );

            // Test collection 'Contains' method.

            c.Add( 3 );
            c.Add( 5 );
            c.Add( 1 );
            c.Add( 4 );

            Write( "After 'Add' c:" );
            foreach( int i in c ) Write( " {0}", i );
            WriteLine( );

            WriteLine( "c.Contains( 5 ) = {0}", c.Contains( 5 ) );
            WriteLine( "c.Contains( 6 ) = {0}", c.Contains( 6 ) );

            // Test collection 'CopyTo' method.

            int[ ] a = new int[ 8 ];
            c.CopyTo( a, 3 );

            Write( "After 'CopyTo' a:" );
            foreach( int i in a ) Write( " {0}", i );
            WriteLine( );

            // Test collection 'Remove' method.

            WriteLine( "c.Remove( 5 ) = {0}", c.Remove( 5 ) );
            Write( "After 'c.Remove( 5 )' c:" );
            foreach( int i in c ) Write( " {0}", i );
            WriteLine( );
            WriteLine( "c.Remove( 6 ) = {0}", c.Remove( 6 ) );
            Write( "After 'c.Remove( 6 )' c:" );
            foreach( int i in c ) Write( " {0}", i );
            WriteLine( );

            // Text collection 'Count' property.

            WriteLine( "c.Count = {0}", c.Count );

            // Text collection 'IsReadOnly' property.

            WriteLine( "c.IsReadOnly = {0}", c.IsReadOnly );
        }
    }
}

