using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Pupple.Objects;

namespace Pupple.Managers;

public class BubbleManager : IComponent
{
    private readonly int _maxRows;
    private readonly int _maxColumns;
    private readonly float _bubbleRadius;
    private readonly float _bubblePadding;
    private readonly float _bubbleLength;
    public readonly float _rowHeight;
    private readonly Bubble[,] _bubbles;
    private bool _isNextRowShort = false;
    private bool _isClusterPopping = false;
    public BubbleManager(int maxRows, int maxColumns)
    {
        _maxRows = maxRows;
        _maxColumns = maxColumns;

        _bubbleRadius = Globals.BubbleRadius;
        _bubblePadding = Globals.BubblePadding;
        _bubbleLength = _bubbleRadius * 2;


        _bubbles = new Bubble[_maxRows, _maxColumns];
        _rowHeight = (float)Math.Sqrt(
            Math.Pow((_bubbleRadius + _bubblePadding) * 2, 2)
            - Math.Pow(_bubbleRadius + _bubblePadding, 2)
        );

        Reset();
    }

    public void Reset()
    {
        for (int i = 0; i < 2; i++)
        {
            AddNewTopLine();
        }
    }

    public void Update()
    {
        // _time += Globals.Time;
        // if ((int)_time > 0)
        // {
        //     _time -= 1;
        //     AddNewTopLine();
        // }

        // for (int row = 0; row < _maxRows; row++)
        // {
        //     for (int col = 0; col < _maxColumns; col++)
        //     {
        //         if (_bubbles[row, col] != null)
        //         {
        //             _bubbles[row, col].Update();

        //             // Remove bubble only when the shrinking animation completes
        //             if (_bubbles[row, col].IsPopping && !_bubbles[row, col].IsActive)
        //             {
        //                 _bubbles[row, col] = null;
        //             }
        //         }
        //     }
        // }

        bool isAnyBubblePopping = false; // Track if any cluster bubbles are still shrinking

        for (int row = 0; row < _maxRows; row++)
        {
            for (int col = 0; col < _maxColumns; col++)
            {
                if (_bubbles[row, col] != null)
                {
                    _bubbles[row, col].Update();

                    if (_bubbles[row, col].IsPopping) 
                    {
                        isAnyBubblePopping = true; // If any bubble is shrinking, we wait
                    }

                    // Remove the bubble only after animation completes
                    if (_bubbles[row, col].IsPopping && !_bubbles[row, col].IsActive)
                    {
                        _bubbles[row, col] = null;
                    }
                }
            }
        }
         // If no bubbles are popping anymore, remove floating bubbles
        if (_isClusterPopping && !isAnyBubblePopping)
        {
            _isClusterPopping = false; // Reset flag
            RemoveFloatingBubbles(); // Now remove floating bubbles
        }
    }


    public void Draw()
    {
        for (int row = 0; row < _maxRows; row++)
        {
            for (int col = 0; col < _maxColumns; col++)
            {
                _bubbles[row, col]?.Draw();
            }
        }
    }

    public void HandleShotBubble(Bubble shotBubble)
    {
        Point shotGridPos = GetGridPosition(shotBubble.Position);

        Point[] gridPositions = new Point[6];
        for (int i = 0; i < 6; i++)
        {
            int[] offset = isShortRow(shotGridPos.Y) ? BubbleHelper.ShortRowOffsets[i] : BubbleHelper.LongRowOffsets[i];
            gridPositions[i] = new Point(shotGridPos.X + offset[1], shotGridPos.Y + offset[0]);
        }
        for (int i = 0; i < gridPositions.Length; i++)
        {
            Point neighborGridPos = gridPositions[i];
            if (IsValidGridPosition(neighborGridPos))
            {
                Bubble targetBubble = _bubbles[neighborGridPos.Y, neighborGridPos.X];
                if (targetBubble != null && shotBubble.IsColliding(targetBubble) || shotBubble.Position.Y < _bubbleRadius)
                {
                    AddBubble(shotBubble, shotGridPos.Y, shotGridPos.X);
                    Globals.Shooter.Reload();
                    return;
                }
            }
        }
    }

    private bool isShortRow(int row)
    {
        return row % 2 == 1 ? _isNextRowShort : !_isNextRowShort;
    }

    private Point GetGridPosition(Vector2 position)
    {
        int row = (int)(position.Y / _rowHeight);
        int col = (int)(position.X / (_bubbleLength + _bubblePadding * 2));

        if (isShortRow(row))
        {
            col = (int)((position.X - (_bubbleLength + _bubblePadding) / 2) / (_bubbleLength + _bubblePadding * 2));
        }

        return new Point(col, row);
    }

    public void AddNewTopLine()
    {
        for (int row = _maxRows - 1; row > 0; row--)
        {
            for (int col = 0; col < _maxColumns; col++)
            {
                _bubbles[row, col] = _bubbles[row - 1, col];
                _bubbles[row, col]?.ShiftDown(_rowHeight);
            }
        }

        for (int col = 0; col < _maxColumns; col++)
        {
            _bubbles[0, col] = null;
        }

        int maxColForTopRow = _isNextRowShort ? _maxColumns - 1 : _maxColumns;

        for (int col = 0; col < maxColForTopRow; col++)
        {
            BubbleColor bubbleColor = PickColorBasedOnNeighbors(0, col, _isNextRowShort);
            CreateBubble(0, col, bubbleColor);
        }
        _isNextRowShort = !_isNextRowShort;
    }

    private BubbleColor PickColorBasedOnNeighbors(int row, int col, bool isShortRow)
    {
        Random random = Globals.Instance.random;
        bool ignoreNeighbors = random.Next(0, 100) < 50;

        if (!ignoreNeighbors)
        {
            var neighborColors = GetNeighborColors(row, col, isShortRow);

            if (neighborColors.Count > 0)
            {
                int index = random.Next(neighborColors.Count);
                return neighborColors[index];
            }
        }

        return PickRandomColor();
    }

    private List<BubbleColor> GetNeighborColors(int row, int col, bool isShortRow)
    {
        List<BubbleColor> colors = new();

        int[][] offsets = isShortRow ? BubbleHelper.ShortRowOffsets : BubbleHelper.LongRowOffsets;

        foreach (int[] offset in offsets)
        {
            int nRow = row + offset[0];
            int nCol = col + offset[1];

            if (nRow >= 0 && nRow < _maxRows && nCol >= 0 && nCol < _maxColumns)
            {
                Bubble neighbor = _bubbles[nRow, nCol];
                if (neighbor != null && neighbor is NormalBubble)
                {
                    colors.Add(((NormalBubble)neighbor).Color);
                }
            }
        }

        return colors;
    }

    public void AddBubble(Bubble bubble, int row, int col)
    {
        _bubbles[row, col] = bubble;
        bubble.Position = CalculatePosition(row, col, isShortRow(row));
        bubble.Reset();
        CheckForBubblePop(row, col);
        RemoveFloatingBubbles();
    }

    private void CreateBubble(int row, int col, BubbleColor color)
    {
        if (col < 0 || col >= _maxColumns) return; // ? Should we throw an exception here?

        Vector2 position = CalculatePosition(row, col, _isNextRowShort);
        _bubbles[row, col] = new NormalBubble(position, Globals.BubbleTexture, color);
    }

    private BubbleColor PickRandomColor()
    {
        return Common.GetRandomElement(BubbleHelper.BubbleColorsLv1);
    }

    private Vector2 CalculatePosition(int row, int col, bool isShortRow)
    {
        float x = col * (_bubbleLength + _bubblePadding * 2) + _bubbleRadius;
        float y = row * _rowHeight + _bubbleRadius + _bubblePadding;

        if (isShortRow)
        {
            x += (_bubbleLength + _bubblePadding) / 2.0f;
        }
        return new Vector2(x, y);
    }

    private void CheckForBubblePop(int row, int col)
    {
        BubbleColor targetColor = ((NormalBubble)_bubbles[row, col]).Color;
        List<Vector2> connectedBubbles = FindConnectedBubblesColor(col, row, targetColor);

        if (connectedBubbles.Count >= 3)
        {
            _isClusterPopping = true; // Mark that a cluster is popping
            foreach (Vector2 bubblePos in connectedBubbles)
            {
                Bubble bubble = _bubbles[(int)bubblePos.Y, (int)bubblePos.X];
                if (bubble != null)
                {
                    bubble.StartPop(); // Start shrinking animation
                }
            }
        }
    }


    private List<Vector2> FindConnectedBubblesColor(int col, int row, BubbleColor color)
    {
        List<Vector2> connected = new();
        HashSet<Vector2> visited = new();
        Stack<Vector2> stack = new();

        stack.Push(new Vector2(col, row));
        while (stack.Count > 0)
        {
            Vector2 pos = stack.Pop();
            if (visited.Contains(pos)) continue;
            visited.Add(pos);

            int r = (int)pos.Y;
            int c = (int)pos.X;

            if (_bubbles[r, c] != null && ((NormalBubble)_bubbles[r, c]).Color == color)
            {
                connected.Add(new Vector2(c, r));

                foreach (var offset in isShortRow(r) ? BubbleHelper.ShortRowOffsets : BubbleHelper.LongRowOffsets)
                {
                    int neighborRow = r + offset[0];
                    int neighborCol = c + offset[1];

                    if (IsValidGridPosition(neighborCol, neighborRow) && !visited.Contains(new Vector2(neighborCol, neighborRow)))
                    {
                        stack.Push(new Vector2(neighborCol, neighborRow));
                    }
                }
            }
        }
        return connected;
    }

    private List<Vector2> FindConnectedBubbles(int col, int row)
    {
        List<Vector2> connected = new();
        HashSet<Vector2> visited = new();
        Stack<Vector2> stack = new();

        stack.Push(new Vector2(col, row));
        while (stack.Count > 0)
        {
            Vector2 pos = stack.Pop();
            if (visited.Contains(pos)) continue;
            visited.Add(pos);

            int r = (int)pos.Y;
            int c = (int)pos.X;

            if (_bubbles[r, c] != null)
            {
                connected.Add(new Vector2(c, r));

                foreach (var offset in isShortRow(r) ? BubbleHelper.ShortRowOffsets : BubbleHelper.LongRowOffsets)
                {
                    int neighborRow = r + offset[0];
                    int neighborCol = c + offset[1];

                    if (IsValidGridPosition(neighborCol, neighborRow) && !visited.Contains(new Vector2(neighborCol, neighborRow)))
                    {
                        stack.Push(new Vector2(neighborCol, neighborRow));
                    }
                }
            }
        }
        return connected;
    }

    private void RemoveFloatingBubbles()
    {
        HashSet<Vector2> connectedToTop = new();

        for (int col = 0; col < _maxColumns; col++)
        {
            if (_bubbles[0, col] != null)
            {
                FindConnectedBubbles(col, 0).ForEach(pos => connectedToTop.Add(pos));
            }
        }

        // Remove floating bubbles immediately
        for (int row = 1; row < _maxRows; row++)
        {
            for (int col = 0; col < _maxColumns; col++)
            {
                if (_bubbles[row, col] != null && !connectedToTop.Contains(new Vector2(col, row)))
                {
                    _bubbles[row, col] = null; // Directly remove floating bubbles
                }
            }
        }
    }

    private bool IsValidGridPosition(int x, int y)
    {
        return y >= 0 && y < _maxRows && x >= 0 && x < _maxColumns;
    }

    private bool IsValidGridPosition(Point pos)
    {
        return pos.Y >= 0 && pos.Y < _maxRows && pos.X >= 0 && pos.X < _maxColumns;
    }

    private void PrintGrid()
    {
        for (int row = 0; row < _maxRows; row++)
        {
            for (int col = 0; col < _maxColumns; col++)
            {
                System.Console.Write(_bubbles[row, col] == null ? " xxxx,xxxx |" : $" {(int)_bubbles[row, col].Position.X,0:D4},{(int)_bubbles[row, col].Position.Y,0:D4} |");
            }
            System.Console.WriteLine();
        }
    }
}