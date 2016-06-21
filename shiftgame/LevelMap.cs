using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace shiftgame
{
    enum MapSector
    {
        Unoccupied, // szabad szektor
        Occupied,   // foglalt szektor
        Fatal,      // halálos szektor
        Disappearing, // eltűnő szektor
        Exit        // kijárat
    }
    class LevelMap : Bindable
    {
        private MapSector[,] sectors; // pálya szektorai
        public MapSector[,] Sectors { get { return sectors; } private set { } }

        private const int SS = 20; // SectorSize: 20px

        private Point playerStartPoint; // játékos kezdőpontja
        public Point PlayerStartPoint { get { return playerStartPoint; } private set { } }

        private Point mapFinishPoint; // pálya végpontja
        public Point MapFinishPoint { get { return mapFinishPoint; } private set { } }

        private bool shiftMode; // shift mód aktív?
        private bool showDisappearingSectors = true;

        public bool ShowDisappearingSectors
        {
            get { return showDisappearingSectors; }
            set { showDisappearingSectors = value; }
        }

        public bool ShiftMode
        {
            get { return shiftMode; }
            set { shiftMode = value; }
        }

        GeometryGroup mapSectors; // elfoglalt pálya szektorok DrawingGeometry()-hez
        public Geometry MapSectors
        {
            get
            {
                if (showDisappearingSectors) 
                    return Geometry.Combine(mapSectors, disappearingSectors, GeometryCombineMode.Union, null);
                else 
                    return mapSectors;
            }
        }

        GeometryGroup fatalSectors; // halálos szektorok DrawingGeometry()-hez
        public GeometryGroup FatalSectors
        {
            get { return fatalSectors; }
        }

        public Geometry MapStructure // elfoglalt + halálos szektorok DrawingGeometry()-hez
        {
            get
            {
                return Geometry.Combine(mapSectors, fatalSectors, GeometryCombineMode.Union, null);
            }
        }

        RectangleGeometry exitSector; // kijárat a pályáról

        public RectangleGeometry ExitSector
        {
            get { return exitSector; }
        }

        GeometryGroup disappearingSectors;

        public GeometryGroup DisappearingSectors
        {
            get { return disappearingSectors; }
        }

        public LevelMap(int mapNumber)
        {
            // pálya txt beolvasás Resources-ból
            ResourceManager rm = new ResourceManager("shiftgame.Properties.Resources", Assembly.GetExecutingAssembly());
            string mapResource = rm.GetString("map_" + mapNumber);
            string[] lines = mapResource.Split('\n');

            sectors = new MapSector[lines.Length, lines[0].Length - 1];
            // sor
            for (int i = 0; i < sectors.GetLength(0); i++)
            {
                // oszlop
                for (int j = 0; j < sectors.GetLength(1); j++)
                {
                    // elfoglalt szektor
                    if (lines[i][j] == 'x')
                    {
                        sectors[i, j] = MapSector.Occupied;
                    }
                    // halálos szektor
                    else if (lines[i][j] == '*')
                    {
                        sectors[i, j] = MapSector.Fatal;
                    }
                    else if (lines[i][j] == 'D')
                    {
                        sectors[i, j] = MapSector.Disappearing;
                    }
                    // start mező
                    else if (lines[i][j] == 'S')
                    {
                        playerStartPoint = new Point(j * SS, i * SS);
                    }
                    // cél mező
                    else if (lines[i][j] == 'F')
                    {
                        mapFinishPoint = new Point(j * SS, i * SS);
                        sectors[i, j] = MapSector.Exit;
                    }
                }
            }

            CreateMapStructure(false);
        }

        public void ShiftMapSectors()
        {
            CreateMapStructure(true);
        }

        // elfoglalt és halálos szektor Geometry-k összegyűjtése,
        // shift módban szabad <-> elfoglalt szektor csere
        private void CreateMapStructure(bool shiftMapSectors)
        {
            mapSectors = new GeometryGroup();
            fatalSectors = new GeometryGroup();
            disappearingSectors = new GeometryGroup();

            for (int i = 0; i < sectors.GetLength(0); i++)
            {
                for (int j = 0; j < sectors.GetLength(1); j++)
                {
                    if (sectors[i, j] == MapSector.Occupied)
                    {
                        if (shiftMapSectors)
                            sectors[i, j] = MapSector.Unoccupied;
                        else
                            mapSectors.Children.Add(new RectangleGeometry(new Rect(j * SS, i * SS, SS, SS)));
                    }
                    else if (shiftMapSectors && sectors[i, j] == MapSector.Unoccupied)
                    {
                        sectors[i, j] = MapSector.Occupied;
                        mapSectors.Children.Add(new RectangleGeometry(new Rect(j * SS, i * SS, SS, SS)));
                    }
                    else if (sectors[i, j] == MapSector.Disappearing)
                    {
                        disappearingSectors.Children.Add(new RectangleGeometry(new Rect(j * SS, i * SS, SS, SS)));
                    }
                    else if (sectors[i, j] == MapSector.Fatal)
                    {
                        PathFigure figure = new PathFigure(
                            new Point(j * SS + 20, i * SS),
                            new[] { new LineSegment(new Point(j * SS + SS, i * SS + SS), true), 
                                        new LineSegment(new Point(j * SS, i * SS + SS), true)}, true);
                        fatalSectors.Children.Add(new PathGeometry(new[] { figure }));
                    }
                    else if (sectors[i, j] == MapSector.Exit)
                    {
                        exitSector = new RectangleGeometry(new Rect(j * SS, i * SS, SS, SS));
                    }
                }
            }
        }
        // ütközés vizsgálat az elfoglalt szektorokkal
        public bool CollideMapSectors(Geometry clone)
        {
            Geometry combined = Geometry.Combine(clone, this.MapSectors, GeometryCombineMode.Intersect, null);
            return combined.GetArea() > 0;
        }
        // ütközés vizsgálat a halálos szektorokkal
        public bool CollideFatalSectors(Geometry clone)
        {
            Geometry combined = Geometry.Combine(clone, fatalSectors, GeometryCombineMode.Intersect, null);
            return combined.GetArea() > 0;
        }
    }
}

