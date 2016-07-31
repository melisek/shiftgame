using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace shiftgame.mapConverter
{
    public enum MapSector
    {
        Unoccupied, // szabad szektor
        Occupied,   // foglalt szektor
        Fatal,      // halálos szektor
        Disappearing, // eltűnő szektor
        Exit        // kijárat
    }
    static public class MapConverter
    {
        static List<string> list = new List<string>();
        static char sep = ',';

        static public bool Convert(string[] lines)
        {
            list = lines.ToList();
            return CompressMapCharCount() != "";
        }
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

        static public MapSector[,] DecompressMap(string s, int rowCnt, int colCnt, int sectorSize, ref Point playerStartPoint, ref Point mapFinishPoint)
        {
            MapSector[,] sectors = new MapSector[rowCnt, colCnt];
            int row = 0;
            int col = 0;
            int lastSep;
            string[] splitted = CutLRC(s, out lastSep).Split(sep);

            foreach (string chunk in splitted)
            {
                int sectorLength = int.Parse(chunk.Substring(1));
                int sectorNum = 0;


                if (chunk[0] == ' ')
                {
                    int filledEntireRowCnt = sectorLength / colCnt;
                    int remainedColCnt = sectorLength % colCnt;

                    row += filledEntireRowCnt;
                    col += remainedColCnt;

                    if(col >= colCnt)
                    {
                        row++;
                        col -= colCnt - 1;
                    }
                }
                else
                {
                    switch (chunk[0])
                    {
                        case 'x':
                            sectorNum = 1; break;
                        case '*':
                            sectorNum = 2; break;
                        case 'D':
                            sectorNum = 3; break;
                        case 'S':
                            playerStartPoint = new Point(col * sectorSize, row * sectorSize); break;
                        case 'F':
                            sectorNum = 4;
                            mapFinishPoint = new Point(col * sectorSize, row * sectorSize); break;

                    }

                    for (int i = 0; i < sectorLength; i++)
                    {
                        sectors[row, col] = (MapSector)sectorNum;

                        if (col < colCnt - 1 && row < rowCnt - 1)
                            col++;
                        else
                        { row++; col = 0; }
                    }
                }
                
            }

            return sectors;

        }

        static string CutLRC(string s, out int lastSep)
        {
            lastSep = s.LastIndexOf(sep);
            return s.Substring(0, lastSep);
        }

        static public bool ValidateMap(string s)
        {
            int lastSep;
            string noLRC = CutLRC(s, out lastSep);
            int lrcheck = int.Parse(s.Substring(lastSep + 1));
            return LRC(noLRC + ',') == lrcheck;
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
}
