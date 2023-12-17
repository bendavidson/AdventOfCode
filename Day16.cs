using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day16 : DayBase
    {
        private Tuple<int, int> _lastTile;
        public Day16() : base("Day16") { }

        protected override void Solve()
        {
            List<Tile> tiles = new List<Tile>();

            int x = 0;
            
            foreach(var line in lines)
            {
                for(int y = 0; y < line.Length; y++)
                {
                    Tile tile = new Tile
                    {
                        Position = new Tuple<int, int>(x, y),
                        TileType = line[y]
                    };
                    tiles.Add(tile);
                }
                x++;
            }

            _lastTile = new Tuple<int, int>(lines.Count()-1, lines[0].Length-1);

            if(partNo == 1)
            {
                total = CountEnergisedTiles(tiles[0], tiles,Direction.Right);
            }
            else
            {
                for(x = 0; x < _lastTile.Item1; x++)
                {
                    tiles = tiles.ConvertAll(x => new Tile { Position = x.Position, TileType = x.TileType });
                    total = Math.Max(total, CountEnergisedTiles(tiles.FirstOrDefault(t => t.Position.Equals(new Tuple<int, int>(x,0))), tiles,Direction.Right));

                    tiles = tiles.ConvertAll(x => new Tile { Position = x.Position, TileType = x.TileType }); 
                    total = Math.Max(total, CountEnergisedTiles(tiles.FirstOrDefault(t => t.Position.Equals(new Tuple<int, int>(x, _lastTile.Item2))), tiles, Direction.Left));
                }

                for(int y = 0; y < _lastTile.Item2; y++)
                {
                    tiles = tiles.ConvertAll(x => new Tile { Position = x.Position, TileType = x.TileType }); 
                    total = Math.Max(total, CountEnergisedTiles(tiles.FirstOrDefault(t => t.Position.Equals(new Tuple<int, int>(0, y))), tiles, Direction.Down));

                    tiles = tiles.ConvertAll(x => new Tile { Position = x.Position, TileType = x.TileType }); 
                    total = Math.Max(total, CountEnergisedTiles(tiles.FirstOrDefault(t => t.Position.Equals(new Tuple<int, int>(_lastTile.Item1, y))), tiles, Direction.Up));
                }
            }

        }

        private int CountEnergisedTiles(Tile energisedTile, List<Tile> tiles, Direction startingDirection)
        {
            Console.WriteLine(energisedTile.Position.Item1.ToString() + ":" + energisedTile.Position.Item2.ToString());
            var newConnections = GetTileConnections(energisedTile, startingDirection);

            List<Tuple<int, int, Direction>> splitDirections = new List<Tuple<int, int, Direction>>();

            while (newConnections.Count() > 0 || splitDirections.Count > 0)
            {
                if (newConnections.Count == 0)
                {
                    energisedTile = tiles.FirstOrDefault(t => t.Position.Equals(new Tuple<int, int>(splitDirections.LastOrDefault().Item1, splitDirections.LastOrDefault().Item2)));
                    newConnections = GetTileConnections(energisedTile, splitDirections.LastOrDefault().Item3);
                    splitDirections.RemoveAt(splitDirections.Count - 1);
                    continue;
                }
                else if (newConnections.Count() > 1)
                {
                    splitDirections.Add(newConnections[1]);
                }
                energisedTile = tiles.FirstOrDefault(t => t.Position.Equals(new Tuple<int, int>(newConnections[0].Item1, newConnections[0].Item2)));
                newConnections = GetTileConnections(energisedTile, newConnections[0].Item3);
            }

            return tiles.Where(x => x.Connections != null).Count();
        }

        private List<Tuple<int,int,Direction>> GetTileConnections(Tile tile, Direction direction)
        {
            List<Tuple<int,int,Direction>> result = new List<Tuple<int,int,Direction>>();

            switch (tile.TileType)
            {
                case '.':
                    switch (direction)
                    {
                        case Direction.Left:
                            result = new List<Tuple<int, int, Direction>> { GetConnection(tile.Position, Direction.Left) }; break;
                        case Direction.Right:
                            result = new List<Tuple<int, int, Direction>> { GetConnection(tile.Position, Direction.Right) }; break;
                        case Direction.Up:
                            result = new List<Tuple<int, int, Direction>> { GetConnection(tile.Position, Direction.Up) }; break;
                        case Direction.Down:
                            result = new List<Tuple<int, int, Direction>> { GetConnection(tile.Position, Direction.Down) }; break;
                    }
                    break;
                case '\\':
                    switch (direction)
                    {
                        case Direction.Left:
                            result = new List<Tuple<int, int, Direction>> { GetConnection(tile.Position, Direction.Up) }; break;
                        case Direction.Right:
                            result = new List<Tuple<int, int, Direction>> { GetConnection(tile.Position, Direction.Down) }; break;
                        case Direction.Up:
                            result = new List<Tuple<int, int, Direction>> { GetConnection(tile.Position, Direction.Left) }; break;
                        case Direction.Down:
                            result = new List<Tuple<int, int, Direction>> { GetConnection(tile.Position, Direction.Right) }; break;
                    }
                    break;
                case '/':
                    switch (direction)
                    {
                        case Direction.Left:
                            result = new List<Tuple<int, int, Direction>> { GetConnection(tile.Position, Direction.Down) }; break;
                        case Direction.Right:
                            result = new List<Tuple<int, int, Direction>> { GetConnection(tile.Position, Direction.Up) }; break;
                        case Direction.Up:
                            result = new List<Tuple<int, int, Direction>> { GetConnection(tile.Position, Direction.Right) }; break;
                        case Direction.Down:
                            result = new List<Tuple<int, int, Direction>> { GetConnection(tile.Position, Direction.Left) }; break;
                    }
                    break;
                case '-':
                    switch (direction)
                    {
                        case Direction.Left:
                            result = new List<Tuple<int, int, Direction>> { GetConnection(tile.Position, Direction.Left) }; break;
                        case Direction.Right:
                            result = new List<Tuple<int, int, Direction>> { GetConnection(tile.Position, Direction.Right) }; break;
                        case Direction.Up:
                        case Direction.Down:
                            result = new List<Tuple<int, int, Direction>> { GetConnection(tile.Position, Direction.Left), GetConnection(tile.Position, Direction.Right) }; 
                            break;
                    }
                    break;
                case '|':
                    switch (direction)
                    {
                        case Direction.Left:
                        case Direction.Right:
                            result = new List<Tuple<int, int, Direction>> { GetConnection(tile.Position, Direction.Up), GetConnection(tile.Position, Direction.Down) };
                            break;
                        case Direction.Up:
                            result = new List<Tuple<int, int, Direction>> { GetConnection(tile.Position, Direction.Up) }; break;
                        case Direction.Down:
                            result = new List<Tuple<int, int, Direction>> { GetConnection(tile.Position, Direction.Down) }; break;
                    }
                    break;
            }

            if(result.Count() > 0)
            {
                if (tile.Connections == null)
                    tile.Connections = result.Select(x => new Tuple<int, int>(x.Item1, x.Item2)).ToList();
                else
                {
                    // Remove all connections that we've already assigned to the tile (are in a loop)
                    result.RemoveAll(x => tile.Connections.Contains(new Tuple<int, int>(x.Item1, x.Item2)));
                    tile.Connections.AddRange(result.Select(x => new Tuple<int, int>(x.Item1, x.Item2)).ToList().Except(tile.Connections));
                }
            }

            // Remove all connections that go off the edge
            result.RemoveAll(x => x.Item1 < 0 || x.Item1 > _lastTile.Item1 || x.Item2 < 0 || x.Item2 > _lastTile.Item2);

            return result;
        }

        private Tuple<int,int, Direction> GetConnection(Tuple<int,int> position, Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return new Tuple<int, int, Direction>(position.Item1, position.Item2 - 1, direction);
                case Direction.Right:
                    return new Tuple<int, int, Direction>(position.Item1, position.Item2 + 1, direction);
                case Direction.Up:
                    return new Tuple<int, int, Direction>(position.Item1 - 1, position.Item2, direction);
                case Direction.Down:
                    return new Tuple<int, int, Direction>(position.Item1 + 1, position.Item2, direction);
                default:
                    return new Tuple<int,int,Direction>(position.Item1,position.Item2,direction);
            }
        }

        class Tile
        {
            public Tuple<int, int> Position;
            public char TileType;
            public List<Tuple<int, int>> Connections;
        }

        enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}
