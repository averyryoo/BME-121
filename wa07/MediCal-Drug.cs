using System;

// Classes related to California Department of Health Care Services
// Medi-Cal (a Medicaid welfare program for low-income individuals).
// See http://medi-cal.ca.gov or http://www.dhcs.ca.gov for more information.

namespace MediCal
{
    // A Drug object holds information about one fee-for-service outpatient drug 
    // reimbursed by Medi-Cal to pharmacies during one calendar-year quarter.
    
    class Drug
    {
        // All fields are private.
        
        string code;            // old Medi-Cal drug code
        string name;            // brand name, strength, dosage form
        string id;              // national drug code number
        double size;            // package size
        string unit;            // unit of measurement
        double quantity;        // number of units dispensed
        double lowest;          // price Medi-Cal is willing to pay
        double ingredientCost;  // estimated ingredient cost
        int    numTar;          // number of claims with a 'treatment authorization request'
        double totalPaid;       // total amount paid
        double averagePaid;     // average paid per prescription
        int    daysSupply;      // total days supply
        int    claimLines;      // total number of claim lines
        
        // Properties providing read-only access to every field.
        
        public string Code           { get { return code;           } }               
        public string Name           { get { return name;           } }               
        public string Id             { get { return id;             } }                 
        public double Size           { get { return size;           } }             
        public string Unit           { get { return unit;           } }             
        public double Quantity       { get { return quantity;       } }         
        public double Lowest         { get { return lowest;         } }             
        public double IngredientCost { get { return ingredientCost; } }    
        public int    NumTar         { get { return numTar;         } }                
        public double TotalPaid      { get { return totalPaid;      } }          
        public double AveragePaid    { get { return averagePaid;    } }        
        public int    DaysSupply     { get { return daysSupply;     } }            
        public int    ClaimLines     { get { return claimLines;     } }            
        
        // Hide the default constructor by providing a do-nothing private parameterless constructor.  
        // We provide no other constructors so the user must call "ParseFileLine" to get a new "Drug" object.
        
        private Drug( ) { }
        
        // Parse a string of the form used for each line in the file of drug data.
        // Mostly there are specific columns in the file for each piece of information.
        // The exception is that 'size' and 'unit' are concatenated.  They are collected
        // together and then separated by noting that 'unit' is always the last two characters.
        // Note:  The document describing the file layout doesn't quite match the file.
        // The field widths of "id" and "averagePaid" are two characters longer than stated.
        // The field "daysSupply" seems to use an exponential notation for numbers of a million or larger.
        // This method has been fully tested on the Medi-Cal quarterly data file "RXQT1503.txt".
        
        public static Drug ParseFileLine( string line )
        {
            if( line == null ) throw new ArgumentNullException( "String is null.", nameof( line ) );
            if( line.Length != 158 ) throw new ArgumentException( "Length must be 158", nameof( line ) );
            
            Drug newDrug = new Drug( );
            
            newDrug.code = line.Substring( 0, 7 ).Trim( );
            newDrug.name = line.Substring( 7, 30 ).Trim( );
            newDrug.id = line.Substring( 37, 13 ).Trim( );
            string sizeWithUnit = line.Substring( 50, 14 ).Trim( );
            newDrug.size = double.Parse( sizeWithUnit.Substring( 0 , sizeWithUnit.Length - 2 ) );
            newDrug.unit = sizeWithUnit.Substring( sizeWithUnit.Length - 2, 2 );
            newDrug.quantity = double.Parse( line.Substring( 64, 16 ) );
            newDrug.lowest = double.Parse( line.Substring( 80, 10 ) );
            newDrug.ingredientCost = double.Parse( line.Substring( 90, 12 ) );
            newDrug.numTar = int.Parse( line.Substring( 102, 8 ) );
            newDrug.totalPaid = double.Parse( line.Substring( 110, 14 ) );
            newDrug.averagePaid = double.Parse( line.Substring( 124, 10 ) );
            newDrug.daysSupply = ( int ) double.Parse( line.Substring( 134, 14 ) );
            newDrug.claimLines = int.Parse( line.Substring( 148, 10 ) );
            
            return newDrug;
        }
        
        // Produce a string of the form used for each line in the file of drug data.
        // Note:  The document describing the file layout doesn't quite match the file.
        // The field widths of "id" and "averagePaid" are two characters longer than stated.
        // The field "daysSupply" seems to use an exponential notation for numbers of a million or larger.
        // This method has been fully tested on the Medi-Cal quarterly data file "RXQT1503.txt".
        
        public string ToFileLine( )
        {
            string sizeWithUnit = string.Concat( size.ToString( "f3" ), unit );
            string daysSupplyFormatted;
            if( daysSupply >= 1_000_000 ) daysSupplyFormatted = daysSupply.ToString( "0.#####e+000" );
            else daysSupplyFormatted = daysSupply.ToString( "f0" );
            
            return $"{code,-7}{name,-30}{id,-13}{sizeWithUnit,-14}{quantity,-16:f0}"
                + $"{lowest,-10:#.0000;-#.0000}{ingredientCost,-12:#.00;-#.00}{numTar,-8}"
                + $"{totalPaid,-14:#.00;-#.00}{averagePaid,-10:#.00;-#.00}"
                + $"{daysSupplyFormatted,-14}{claimLines,-10}";
        }
        
        // Simple string for debugging purposes, showing only selected fields.
        // We assume the combination of these selected fields is unique for each drug.
        
        public override string ToString( ) { return $"{id}, {name}, {size}"; }
    }
}
