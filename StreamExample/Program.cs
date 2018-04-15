using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace StreamExample
{
    class Program
    {
        //https://github.com/f4tp/Learning-StreamExamples
        static void Main(string[] args)
        {
            BasicDirInfoAndDirWork();
            WritingAndReadingArraysToFile();
            WorkingWithFilesInProgram();
            BytesFileStreamWork();
            StreamReaderWriterWork();
            BinaryReaderWriterWork();
            Console.ReadLine();

        }

        public static void BasicDirInfoAndDirWork()
        {
            //the current director the program is beign executed from
            DirectoryInfo currentDirectory = new DirectoryInfo(".");

            //mapping to another directory to see inside of it
            DirectoryInfo PaulsDir = new DirectoryInfo(@"C:\Users\Quad");
            Console.WriteLine(currentDirectory.FullName);
            Console.WriteLine(PaulsDir.Name);
            Console.WriteLine(PaulsDir.Parent);
            Console.WriteLine(PaulsDir.Attributes);
            Console.WriteLine(PaulsDir.CreationTime);
            Console.WriteLine(PaulsDir.CreationTimeUtc);
            //this does not exist so it was supposed to create it for us but it didn't
            DirectoryInfo datDir = new DirectoryInfo(@"C:\Users\Quad\Documents\Made Dir");
            Console.ReadLine();
            //deletes the directory - but it didn't create it so it causes a runtime error
            //Directory.Delete(@"C:\Users\Quad\Documents\MadeDir");
        }

        public static void WritingAndReadingArraysToFile()
        {
            string[] customers = { "Bob Smith", "Sally Smith", "Emma Salis" };
            string textFilePath = @"C:\Users\Quad\source\repos\StreamExample\StreamExample\bin\Debug\StreamTextFile1.txt";

            // write the string array object to the text file (filepath, object you want to write to the file) 
            File.WriteAllLines(textFilePath, customers);
            //read all lines reads into an array
            string[] tempArray = File.ReadAllLines(textFilePath);
            //loop through the array and output each string
            foreach (string s in tempArray)
            {
                Console.WriteLine(s);
            }
            //loop through the file and read it back in - more soph
            foreach (string cust in File.ReadAllLines(textFilePath))
            {
                Console.WriteLine($"Customer : {cust}");
            }
            //another method in there - INVESTIGATE
            IEnumerable<string> textSTringfromfile = File.ReadLines(textFilePath);
            Console.WriteLine(textSTringfromfile.ToString());
            Console.ReadLine();
        }

        public static void WorkingWithFilesInProgram()
        {
            //next bunch of methods require a DirectoryInfo object for the file path
            //rather than a string
            DirectoryInfo myDataDir = new DirectoryInfo(@"C:\Users\Quad\source\repos\StreamExample\StreamExample\bin\Debug\");
            //array of FileInfo objects
            FileInfo[] txtFiles = myDataDir.GetFiles("*.txt", SearchOption.AllDirectories);
            Console.WriteLine($"Matches : {txtFiles.Length}");
            foreach (FileInfo file in txtFiles)
            {
                Console.WriteLine(file.Name);
                Console.WriteLine(file.Length);
                Console.WriteLine(file.GetHashCode());
            }
            Console.ReadLine();
        }

        public static void BytesFileStreamWork()
        {
//WRITING A BYTE ARRAY TO FILE, BYTE ARRAY CREATED FROM A STRING
            string textfilepath2 = @"C:\Users\Quad\source\repos\StreamExample\StreamExample\bin\Debug\textfile2.txt";

            //create the file if it does not exist
            FileStream fs = File.Open(textfilepath2, FileMode.Create);
            string stringToConv = "This is a random string";

            //gets the bytes for the string passed to it - what does it means - bytes...
            ///does it mean the binary representations of the ASCII / Unicode chars?
            byte[] rsByteArray = Encoding.Default.GetBytes(stringToConv);

            //after creating above, we are now writing to the file...
            //.Write()will only write a block of bytes to the file...
            ///the array(only) of bytes to write, index to start at in the array, the number of byte objects to write - which is why it is .Length, and not .Length - 1
            fs.Write(rsByteArray, 0, rsByteArray.Length);
           
            //resets the position pointer in the array
            fs.Position = 0;

//START TO READ BACK IN AGAIN
            byte[] fileByteArray = new byte[rsByteArray.Length];
            for (int i = 0; i < rsByteArray.Length; i+=1)
            {
                fileByteArray[i] = (byte)fs.ReadByte();
            }
            //cannot output the bytes in the array as a string until it has been converted
            //back to a string 
            Console.WriteLine(Encoding.Default.GetString(fileByteArray));
            fs.Close();
        }

        public static void StreamReaderWriterWork()
        {
            //uses strings as file path, not a DirectoryInfo object like with others
            string textfilepath3 = @"C:\Users\Quad\source\repos\StreamExample\StreamExample\bin\Debug\textfile2.txt";
            
            StreamWriter sw = File.CreateText(textfilepath3);
            //writes without a new line character at the end
            sw.Write("This is a random ");
            //writes with the new line character at the end
            sw.WriteLine("sentence");
            sw.WriteLine("This is another sentence");
            sw.WriteLine("and another");
            sw.Close();

            StreamReader sr = File.OpenText(textfilepath3);
            //peek returns the the denary number for the first character - unicode standard, then we can convert that to the unicode character, to see what type of char it is... can determin whether it is a new line, or the end of the file for example
            //peek doesn't change the marker / pointer of the location, so you can still read the full work back after executing this instruction
            char nextCharInStream = Convert.ToChar(sr.Peek());
            Console.WriteLine($"Peek char gives : {nextCharInStream}");

            //will read up until it finds a new line char
            Console.WriteLine($"1st string : {sr.ReadLine()}");

            //will read from wherever the pointer / marker left off -  from the last instruction
            Console.WriteLine($"Read rest of file : {Environment.NewLine}{sr.ReadToEnd()}");
            sr.Close();

            Console.ReadLine();
        }

        //used to read and write data types
        public static void BinaryReaderWriterWork()
        {
    //writing binary to a file
            //dat file not a text file
            string textfilepath4 = @"C:\Users\Quad\source\repos\StreamExample\StreamExample\bin\Debug\textfile4.dat";

            FileInfo datFile = new FileInfo(textfilepath4);
            BinaryWriter bw = new BinaryWriter(datFile.OpenWrite());
            string RandomText = "Random Text";
            int myAge = 42;
            double height = 6.25;
            //binary writer can obvs. write different data types to the file, not just strings
            bw.Write(RandomText);
            bw.Write(myAge);
            bw.Write(height);
            bw.Close();


            //reading back in now
            BinaryReader br = new BinaryReader(datFile.OpenRead());
            //have to set out the data type that is going to be read back in...
            //QUESTION - is there a way to determine what data type is in there?
            //Reading wrong data types in messes up the data being read in afterwards
            Console.WriteLine(br.ReadString());
            Console.WriteLine(br.ReadInt32());
            Console.WriteLine(br.ReadDouble());


            Console.ReadLine();
        }
    }
}
