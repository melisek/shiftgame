using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace mapConverter
{
    static class MapConverter
    {
        static List<string> list = new List<string>();
        static char sep = ',';
        static public bool Convert(string filename)
        {
            ReadMap(filename);
            string map = CompressMapCharCount();
            if (map != "")
            {
                WriteMap(filename, map);
                return true;
            }
            return false;
        }

        static private void ReadMap(string filename)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

        }

        static private void WriteMap(string filename, string s)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("compressed-" + filename))
                {
                    sw.Write(s);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        static private string CompressMapCharCount()
        {
            string newstr = "";
            int allChCount = 0;
            int j = 0;

            while (j < list.Count - 1)
            {
                string output = "";
                int curChCount = 1;
                int i = 0;

                while (j < list.Count && i < list.ElementAt(j).Length)
                {
                    while (i < list.ElementAt(j).Length - 1 && list.ElementAt(j)[i] == list.ElementAt(j)[i + 1])
                    {
                        curChCount++;
                        i++;
                    }

                    if (i == list.ElementAt(j).Length - 1)
                    {
                        if (j < list.Count - 1 && list.ElementAt(j)[i] == list.ElementAt(j + 1)[0])
                        {
                            j++;
                            i = 0;
                            curChCount++;
                            continue;
                        }
                        else
                        {
                            output += list.ElementAt(j)[i] + curChCount.ToString() + sep;
                            allChCount += curChCount;
                            curChCount = 1;
                            j++;
                            i = -1;
                        }
                    }
                    else if (i == list.ElementAt(j).Length - 1 || list.ElementAt(j)[i] != list.ElementAt(j)[i + 1])
                    {
                        output += list.ElementAt(j)[i] + curChCount.ToString() + sep;
                        allChCount += curChCount;
                        curChCount = 1;
                    }
                    i++;
                }
                newstr += output;
            }

            int lineChCount = 0;
            for (int i = 0; i < list.Count; i++)
            {
                lineChCount += list.ElementAt(i).Length;
            }

            return (lineChCount == allChCount) ? newstr + LRC(newstr) : "";
        }

        static private int LRC(string s)
        {
            var LRC = 0;
            var byteArr = Encoding.ASCII.GetBytes(s);

            foreach (byte b in byteArr)
            {
                LRC = (LRC + b) & 255;
            }

            return ((LRC ^ 255) + 1) & 255;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {           
            if (MapConverter.Convert("map_4.txt"))
            {
                Console.WriteLine("Térkép tömörítve.");
            }
            else
            {
                Console.WriteLine("Sikertelen tömörítés.");
            }
            
            Console.ReadLine();
        }
    }
}
