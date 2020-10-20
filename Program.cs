using System;

namespace XYZtoSawSimTsConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            Convert logConversion = new Convert();
            string sourcePath = getXYZPath();
            string destPath = getSawSimPath();
            logConversion.readXYZ(sourcePath);
            logConversion.writeSawSimFile(destPath, 1);

        }


        //Get path to folder containing xyz files
        static string getXYZPath()
        {
            Console.WriteLine("Enter the path to the .XYZ file: ");
            string xyzPath = Console.ReadLine();
            return xyzPath;
        }

        //Get path to target folder where converted SawSimTs files will be placed
        static string getSawSimPath()
        {
            Console.WriteLine("Enter the path to the sawsim file: ");
            string ssPath = Console.ReadLine();
            return ssPath;
        }



        //Convert XYZ to SawSimTs
    }
}
