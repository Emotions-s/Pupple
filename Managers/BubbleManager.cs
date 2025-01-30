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
    private bool _isShortRow = false;
    private float _time;

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
        // for (int row = 0; row < _maxRows; row++)
        // {
        //     AddNewTopLine();
        // }
    }

    public void Update()
    {
        _time += Globals.Time;
        if ((int)_time > 0)
        {
            _time -= 1;
            AddNewTopLine();
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

        int maxColForTopRow = _isShortRow ? _maxColumns - 1 : _maxColumns;

        for (int col = 0; col < maxColForTopRow; col++)
        {
            BubbleColor bubbleColor = PickColorBasedOnNeighbors(0, col, _isShortRow);
            CreateBubble(0, col, bubbleColor);
        }
        _isShortRow = !_isShortRow;
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
    }

    private void CreateBubble(int row, int col, BubbleColor color)
    {
        if (col < 0 || col >= _maxColumns) return; // ? Should we throw an exception here?

        Vector2 position = CalculatePosition(row, col, _isShortRow);
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
}