using System;
using System.IO;
using static System.Console;

using MediCal;
using DataStructures;

namespace DataStructures
{
	delegate bool Comparison(Drug d);
    partial class LinkedList
    {
        // Method forms a sub-list of Drug objects satisfying a target predicate.
        public LinkedList SubList(Comparison compare)
        {
			// establish new Linked List
            LinkedList drugList = new LinkedList();
            
            // establish head
            Node currentNode = Head;
            
            // add to linked List
            while(currentNode.Next != null)
            {
				if(compare(currentNode.Data))
				{
					drugList.AddLast(currentNode.Data);
				}
				currentNode = currentNode.Next;
			}
			// return linked list
			return drugList;
        }
    }
}

namespace Bme121
{
    static class Program
    {
        // Test a drug name for Lorazepam tablet.
        public static bool IsLorazepamTablet( Drug d )
        {
            return d.Name.Contains( "LORAZEPAM" ) && d.Name.Contains( "TABLET" );
        }

        // Test a drug name for Temazipam capsule.
        public static bool IsTemazipamCapusule( Drug d )
        {
            return d.Name.Contains( "TEMAZEPAM" ) && d.Name.Contains( "CAPSULE" );
        }

        // Test the 'LinkedList.SubList' method.
        static void Main( )
        {
            // Parameters for reading the drug file.
            const string path = "RXQT1503.txt";
            const FileMode mode = FileMode.Open;
            const FileAccess access = FileAccess.Read;

            // Load all drugs into a linked list.
            LinkedList allDrugs = new LinkedList( );
            using( FileStream file = new FileStream( path, mode, access ) )
            using( StreamReader reader = new StreamReader( file ) )
            {
                while( ! reader.EndOfStream )
                {
                    string line = reader.ReadLine( );
                    Drug d = Drug.ParseFileLine( line );
                    allDrugs.AddLast( d );
                }
            }
            WriteLine( "allDrugs.Count = {0}", allDrugs.Count );

            // Form a sub-list of Temazipam capsules.
            LinkedList subDrugs1 = allDrugs.SubList(IsTemazipamCapusule);
            WriteLine( "subDrugs1.Count = {0}", subDrugs1.Count );
            foreach( Drug d in subDrugs1.ToArray( ) ) WriteLine( d );

            // Form a sub-list of Lorazepam tablets.
            LinkedList subDrugs2 = allDrugs.SubList(IsLorazepamTablet);
            WriteLine( "subDrugs2.Count = {0}", subDrugs2.Count );
            foreach( Drug d in subDrugs2.ToArray( ) ) WriteLine( d );
        }
    }
}
