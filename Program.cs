using System;
using System.Collections.Generic;

namespace XYZtoSawSimTsConverter
{
    class Program
    {
        static void Main(string[] args)
        {
      
            string sourcePath = getXYZPath();
            //check if the path is to a folder or file 
            //if to a folder then generate an enumerable containing all of the .xyz files in the folder
            string destPath = getSawSimPath();

            if (isPathFolder(sourcePath))
            {
                //enumerate through the folder and get all .xyz
                List<string> listOfXYZ = getAllXYZinFolder(sourcePath);
                //iterate through the list of XYZ files, get the numbers, and convert to sawsim
                batchConvert(listOfXYZ, destPath);
            }
            else
            {
                //check file
                Convert logConversion = new Convert();
                //check if the destination is a folder, if not revise to be a folder and request verification...
                logConversion.readXYZ(sourcePath);
                logConversion.writeSawSimFile(destPath, 1, 1);
            }


            Console.WriteLine("Done");


        }

        static void batchConvert(List<string> listXYZ, string destFolder)
        {
            int count = 1;
            foreach(string xyzFile in listXYZ)
            {
                //get the number from the file
                System.IO.FileInfo fInfo = new System.IO.FileInfo(xyzFile);
                string fileName = fInfo.Name;
                string[] nameComponents = fileName.Split("_");
                int nameNumber = int.Parse(nameComponents[nameComponents.Length - 1].Split(".")[0]);
                Convert logConversion = new Convert();
                logConversion.readXYZ(xyzFile);
                logConversion.writeSawSimFile(destFolder, nameNumber, count);
                count++;
                Single percentDone = 100 * count / listXYZ.Count;
                Console.Clear();
                Console.Write(percentDone + "%");

            }
        }



        //get all xyz files in a folder location
        static List<string> getAllXYZinFolder(string folderPath)
        {
            List<string> xyzList = new List<string>();
            string[] filesInDir = System.IO.Directory.GetFiles(folderPath);
            foreach (string file in filesInDir)
            {
                //if the file contains the xyz extension then add to the out list
                if(file.Contains(".xyz"))
                {
                    xyzList.Add(file);
                }
            }

            return xyzList;

        }

        //Get path to folder containing xyz files
        static string getXYZPath()
        {
            Console.WriteLine("Enter the path to the .XYZ file: ");
            string xyzPath = Console.ReadLine();


            return xyzPath;
        }

        /// <summary>
        /// Check whether the input path is a folder
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static Boolean isPathFolder(string path)
        {
            Boolean isFolder = false;

            if (System.IO.File.Exists(path))
            {
                //the path is to a file
                isFolder = false;
            }
            else if (System.IO.Directory.Exists(path))
            {
                //the path is a folder
                isFolder = true;
            }

            return isFolder;
        }


        //Get path to target folder where converted SawSimTs files will be placed
        static string getSawSimPath()
        {
            Console.WriteLine("Enter the path to the folder where you want to save the converted SawSim files: ");
            string ssPath = Console.ReadLine();
            return ssPath;
        }



        //Convert XYZ to SawSimTs
    }
}
