using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace XYZtoSawSimTsConverter
{
    class Convert
    {


        private Dictionary<Single, List<Tuple<Single, Single>>> _profileSet;

        public Dictionary<Single, List<Tuple<Single, Single>>> ProfileSet
        {
            get { return _profileSet; }
        }

        //read the text file one line at a time
        public void readXYZ(string pathToXYZ)
        {
            //sawSim is in thousandths of inch, so multiply xyz coordinates by 1000
            //for each xyz z coordinate create another key value set for the dictionary with the single key being the z coordinate and all of the xy values fed into a list of tuples

            string line;
            int counter = 0;
            _profileSet = new Dictionary<float, List<Tuple<float, float>>>();
            System.IO.StreamReader xyzFile = new System.IO.StreamReader(pathToXYZ);

            while ((line = xyzFile.ReadLine()) != null)
            {
                //split the line into x, y, and z components
                string[] coordArray = splitLine(line);
                //save all points at the same z coordinate to a dictionary with key = z-value and value = tuple of x,y values
                Single zVal = Single.Parse(coordArray[2]);
                //multiply by 1000
                zVal = zVal * 1000;
                //check if any profiles have been added to the dictionary yet
                //  if no coordinates have been added, then add the first coordinate
                //  if a coordinate has been added the revise the dictionary
                if (_profileSet.ContainsKey(zVal))
                {
                    _profileSet[zVal].Add(xyAtZ(coordArray));
                }
                else
                {
                    //instantiate a new list of profile points and create a new z-value entry in the dictionary
                    List<Tuple<Single, Single>> newProfileSet = new List<Tuple<float, float>>();
                    newProfileSet.Add(xyAtZ(coordArray));
                    _profileSet.Add(zVal, newProfileSet);
                }
               
                               
                
            }
            Console.WriteLine("Number of lines: %d", counter);
        }

        /// <summary>
        /// Splits a line from an xyz file into an array containing x, y, and z coordinates. [0] = x, [1] = y, [2] = z
        /// </summary>
        /// <param name="txtLine"></param>
        /// <returns></returns>
        string[] splitLine(string txtLine)
        {
            string[] pointArray = txtLine.Split(" ");

            return pointArray;

        }

        /// <summary>
        /// using an array of xyz coordinates, creates a tuple of xy coordinates and returns
        /// </summary>
        /// <param name="coordArray"></param>
        /// <returns></returns>
        Tuple<Single, Single> xyAtZ(string[] coordArray)
        {
            Single xVal = Single.Parse(coordArray[0]);
            Single yVal = Single.Parse(coordArray[1]);
            //multiply by 1000 for thousandths
            xVal = xVal * 1000;
            yVal = yVal * 1000;


            Tuple<Single, Single> xyPoints = new Tuple<float, float>(xVal, yVal);

            return xyPoints;

        }



        /// <summary>
        /// takes the dictionary of profiles from the xyz read operation (Key = Unique z value, Value = List of x,y coordinates in tuples) and writes to a SawSim format file 
        /// </summary>
        /// <param name="pathToDestination"></param>
        public void writeSawSimFile(string pathToDestination, int logNum)
        {
            //create a list containing one string for each line in the SawSim file
            List<String> SawSimLine = new List<string>();
            //add the first line
            SawSimLine.Add("StartLog           " + logNum);
            //create a length index
            Single lastLength = 0;

            foreach (KeyValuePair<Single, List<Tuple<Single, Single>>> profile in ProfileSet)
            {
                //start a profile line using the current z value
                string currentProfile = profile.Key.ToString() + "      ";

                //for a given z value, concatenate all of the x, y coordinates on the end of the line
                foreach(Tuple<Single, Single> coordinate in profile.Value)
                {
                    currentProfile = currentProfile + coordinate.Item1.ToString() + "," + coordinate.Item2.ToString() + "     ";
                }

                SawSimLine.Add(currentProfile);
                lastLength = profile.Key;
            }
            lastLength = lastLength + 1000;
            SawSimLine.Add("EndLog          " + lastLength);

            Console.WriteLine("Total current log length: %d", lastLength);
        }


    }
}
