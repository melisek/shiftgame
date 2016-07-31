using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Windows = System.Windows;
using Point = System.Drawing.Point;
using System.Windows.Media;
using System.Windows.Threading;
using shiftgame.mapConverter;

namespace shiftgame
{
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

            bool validMap = MapConverter.ValidateMap(mapResource);

            if (validMap)
            {
                sectors = MapConverter.DecompressMap(mapResource, 31, 40, SS, ref playerStartPoint, ref mapFinishPoint);
                CreateMapStructure(false);
            }
            else
                throw new ValidateMapException();
                
            //string[] lines = mapResource.Split('\n');

            //sectors = new MapSector[lines.Length, lines[0].Length - 1];
            
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
                            mapSectors.Children.Add(new RectangleGeometry(new Windows.Rect(j * SS, i * SS, SS, SS)));
                    }
                    else if (shiftMapSectors && sectors[i, j] == MapSector.Unoccupied)
                    {
                        sectors[i, j] = MapSector.Occupied;
                        mapSectors.Children.Add(new RectangleGeometry(new Windows.Rect(j * SS, i * SS, SS, SS)));
                    }
                    else if (sectors[i, j] == MapSector.Disappearing)
                    {
                        disappearingSectors.Children.Add(new RectangleGeometry(new Windows.Rect(j * SS, i * SS, SS, SS)));
                    }
                    else if (sectors[i, j] == MapSector.Fatal)
                    {
                        PathFigure figure = new PathFigure(
                            new Windows.Point(j * SS + 20, i * SS),
                            new[] { new LineSegment(new Windows.Point(j * SS + SS, i * SS + SS), true), 
                                        new LineSegment(new Windows.Point(j * SS, i * SS + SS), true)}, true);
                        fatalSectors.Children.Add(new PathGeometry(new[] { figure }));
                    }
                    else if (sectors[i, j] == MapSector.Exit)
                    {
                        exitSector = new RectangleGeometry(new Windows.Rect(j * SS, i * SS, SS, SS));
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

