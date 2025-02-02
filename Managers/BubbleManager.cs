using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Pupple.Objects;
using Pupple.States;

namespace Pupple.Managers;

public class BubbleManager : IComponent
{
    private readonly int _maxRows;
    private readonly int _maxColumns;
    private readonly float _bubbleRadius;
    private readonly float _bubblePadding;
    private readonly float _bubbleLength;
    public readonly float _rowHeight;
    private Bubble[,] _bubbles;
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
        _bubbles = new Bubble[_maxRows, _maxColumns];
        for (int i = 0; i < Globals.GameState.StartLine; i++)
        {
            AddNewTopLine();
        }
    }

    public void Update()
    {
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
                    // Remove floating bubbles after they fall off the screen
                    if (_bubbles[row, col]?.IsFloating == true && !_bubbles[row, col].IsActive)
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
        CheckLost();
    }

    public bool IsPassTheStage()
    {
        for (int row = 0; row < _maxRows; row++)
        {
            for (int col = 0; col < _maxColumns; col++)
            {
                if (_bubbles[row, col] != null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void CheckLost()
    {
        for (int col = 0; col < _maxColumns; col++)
        {
            if (_bubbles[_maxRows - 1, col] != null)
            {
                if (Globals.PlayerState.HaveShields)
                {
                    Globals.PlayerState.HaveShields = false;
                    for (int i = 0; i <3; i++)
                    {
                        ClearRow(_maxRows - i - 1);
                    }
                    return;
                }
                Globals.GameState.CurrentState = GameState.State.GameOver;
                return;
            }
        }
    }

    private void ClearRow(int row)
    {
        for (int col = 0; col < _maxColumns; col++)
        {
            _bubbles[row, col] = null;
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
        Globals.SpriteBatch.Draw(Globals.Pixel, new Rectangle(0, _maxRows * (int)_rowHeight, Globals.ScreenW, Globals.ScreenH), Color.Red * 0.5f);
    }

    public void HandleShotBubble(Bubble shotBubble)
    {
        Point shotGridPos = GetGridPosition(shotBubble.Position);

        Point[] gridPositions = new Point[6];
        for (int i = 0; i < 6; i++)
        {
            int[] offset = IsShortRow(shotGridPos.Y) ? BubbleHelper.ShortRowOffsets[i] : BubbleHelper.LongRowOffsets[i];
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

    private bool IsShortRow(int row)
    {
        return row % 2 == 1 ? _isNextRowShort : !_isNextRowShort;
    }

    private Point GetGridPosition(Vector2 position)
    {
        int row = (int)(position.Y / _rowHeight);
        int col = (int)(position.X / (_bubbleLength + _bubblePadding * 2));

        if (IsShortRow(row))
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
        Random random = Globals.Instance.Random;
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
        bubble.Position = CalculatePosition(row, col, IsShortRow(row));
        bubble.Reset();
        CheckForBubblePop(row, col);
        RemoveFloatingBubbles();
    }

    private void CreateBubble(int row, int col, BubbleColor color)
    {
        if (col < 0 || col >= _maxColumns) return; // ? Should we throw an exception here?

        Vector2 position = CalculatePosition(row, col, _isNextRowShort);
        _bubbles[row, col] = new NormalBubble(position, Globals.ShooterSceneSheet, color);
    }

    private BubbleColor PickRandomColor()
    {
        return Common.GetRandomElement(Globals.GameState.BubbleColorsInGame);
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
        Bubble targetBubble = _bubbles[row, col];

        if (targetBubble == null) return;

        HashSet<Vector2> bubblesToDestroy = new();

        if (targetBubble is NormalBubble)
        {
            bubblesToDestroy = GetNormalBubblePop(row, col);
        }
        else if (targetBubble is BombBubble)
        {
            bubblesToDestroy = GetBombBubblePop(row, col);
        }
        else if (targetBubble is RainbowBubble)
        {
            bubblesToDestroy = GetRainbowBubblePop(row, col);
        }
        else if (targetBubble is FreezeBubble)
        {
            bubblesToDestroy = GetFreezeBubblePop(row, col);
        }

        if (bubblesToDestroy.Count == 0 && Globals.GameState.FreezeTime <= 0)
        {
            Globals.GameState.MissCount++;
            if (Globals.GameState.MissCount == Globals.GameState.MaxMissCount)
            {
                AddNewTopLine();
                Globals.GameState.MissCount = 0;
            }
            return;
        }

        if (targetBubble is NormalBubble)
        {
            Globals.GameState.MissCount = 0;
        }

        // Reset miss count if bubbles were destroyed
        Globals.GameState.MissCount = 0;
        _isClusterPopping = true; // Mark that a cluster is popping

        foreach (Vector2 pos in bubblesToDestroy)
        {
            Bubble bubble = _bubbles[(int)pos.Y, (int)pos.X];
            if (bubble != null)
            {
                bubble.StartPop(); // Start shrinking animation
            }
        }

    }

    private HashSet<Vector2> GetNormalBubblePop(int row, int col)
    {
        BubbleColor targetColor = ((NormalBubble)_bubbles[row, col]).Color;
        List<Vector2> connectedBubbles = FindConnectedBubblesColor(col, row, targetColor);

        if (connectedBubbles.Count >= 3)
        {
            return new HashSet<Vector2>(connectedBubbles);
        }
        return new HashSet<Vector2>();

    }

    private HashSet<Vector2> GetBombBubblePop(int row, int col)
    {
        HashSet<Vector2> bubblesToDestroy = [new Vector2(col, row)];

        for (int i = 0; i < BombBubble.BombRadius; i++)
        {
            HashSet<Vector2> neighbors = new();
            foreach (Vector2 pos in bubblesToDestroy)
            {
                GetAllNeighbors((int)pos.X, (int)pos.Y).ForEach(neighbor => neighbors.Add(neighbor));
            }
            foreach (Vector2 neighbor in neighbors)
            {
                bubblesToDestroy.Add(neighbor);
            }
        }

        return bubblesToDestroy;
    }

    private HashSet<Vector2> GetRainbowBubblePop(int row, int col)
    {
        HashSet<Vector2> bubblesToDestroy = new();

        List<Vector2> neighbors = GetAllNeighbors(col, row);

        List<BubbleColor> colors = new();

        foreach (Vector2 pos in neighbors)
        {
            if (_bubbles[(int)pos.Y, (int)pos.X] is NormalBubble)
            {
                colors.Add(((NormalBubble)_bubbles[(int)pos.Y, (int)pos.X]).Color);
            }
        }

        foreach (var color in colors)
        {
            var connectBubble = FindConnectedBubblesColor(col, row, color);
            if (connectBubble.Count >= 3)
            {
                bubblesToDestroy.UnionWith(connectBubble);
            }
        }

        bubblesToDestroy.Add(new Vector2(col, row));
        return bubblesToDestroy;
    }

    private HashSet<Vector2> GetFreezeBubblePop(int row, int col)
    {
        Globals.GameState.FreezeTime = FreezeBubble.FreezeTime;
        return [new(col, row)];
    }

    private List<Vector2> GetAllNeighbors(int col, int row)
    {
        List<Vector2> neighbors = new();
        foreach (var offset in IsShortRow(row) ? BubbleHelper.ShortRowOffsets : BubbleHelper.LongRowOffsets)
        {
            int neighborRow = row + offset[0];
            int neighborCol = col + offset[1];

            if (IsValidGridPosition(neighborCol, neighborRow))
            {
                neighbors.Add(new Vector2(neighborCol, neighborRow));
            }
        }
        return neighbors;
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

            if (_bubbles[r, c] != null && (_bubbles[r, c] is RainbowBubble || ((NormalBubble)_bubbles[r, c]).Color == color))
            {
                connected.Add(new Vector2(c, r));

                foreach (var offset in IsShortRow(r) ? BubbleHelper.ShortRowOffsets : BubbleHelper.LongRowOffsets)
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

                foreach (var offset in IsShortRow(r) ? BubbleHelper.ShortRowOffsets : BubbleHelper.LongRowOffsets)
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

        for (int row = 1; row < _maxRows; row++)
        {
            for (int col = 0; col < _maxColumns; col++)
            {
                if (_bubbles[row, col] != null && !connectedToTop.Contains(new Vector2(col, row)))
                {
                    // _bubbles[row, col] = null;
                    _bubbles[row, col].StartFloating(); // Make it float up and fall
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

    public void ClearAllBubble()
    {
        for (int row = 0; row < _maxRows; row++)
        {
            for (int col = 0; col < _maxColumns; col++)
            {
                _bubbles[row, col] = null;
            }
        }
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