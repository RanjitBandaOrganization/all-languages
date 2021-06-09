using System;

namespace ArrayAndCollections
{
    class Program
    {
        static void Main(string[] args)
        {
            double ldblSingleValue = 12.4;
            //// OR    -   C# can figure out the TYPE of the variable
            ///            NOTE - still the below statement is strong typing
            //var ldblSingleValue = 12.4;
            double[] larrdblValuesForDisplay;

            //Array - 0 - basic version - array
            double[] larrdblValues_0 = new double[2];
            larrdblValues_0[0] = 12.2;
            larrdblValues_0[1] = 29.5;
            larrdblValuesForDisplay = larrdblValues_0;

            //Array - 1 - array initialization syntax
            double[] larrdblValues_1 = new double[2] { 12.2222, 29.555555 };
            ///////////////   OR   - C# compiler can figure out the SIZE of the array
            //double[] larrdblValues_1 = new double[] { 12.2, 29.5 };
            ///////////////   OR   - C# compiler can figure out the TYPE of the array
            //double[] larrdblValues_1 = new[] { 12.2, 29.5 };
            ///////////////   OR   - Enhancing further to use IMPLICIT TYPING
            //var larrdblValues_1 = new[] { 12.2, 29.5 };
            larrdblValuesForDisplay = larrdblValues_1;


            foreach (var item in larrdblValuesForDisplay )
            {   
                //String Interpolation
                Console.WriteLine($"Item value is - {item}");
                //Among all formatting options the following is the base and all boils down to this format only
                Console.WriteLine($"Item value with 1 decimal format - {item:N1}");
            }
        }
    }
}
