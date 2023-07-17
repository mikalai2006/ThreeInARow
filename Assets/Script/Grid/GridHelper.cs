using System;
using System.Collections.Generic;
using UnityEngine;

public class GridHelper
{
  private Grid<GridNode> _grid;
  public Grid<GridNode> Grid
  {
    get { return _grid; }
    private set { }
  }

  public GridHelper(int width, int height, float cellSize = 0.5f)
  {
    _grid = new Grid<GridNode>(width, height, cellSize, this, (Grid<GridNode> grid, GridHelper gridHelper, int x, int y) => new GridNode(grid, gridHelper, x, y));
  }

  public List<GridNode> GetAllGridNodes()
  {
    List<GridNode> list = new List<GridNode>(_grid.GetWidth() * _grid.GetHeight());
    if (_grid != null)
    {
      for (int x = 0; x < _grid.GetWidth(); x++)
      {
        for (int y = 0; y < _grid.GetHeight(); y++)
        {
          list.Add(_grid.GetGridObject(new Vector3Int(x, y)));
        }
      }
    }
    return list;
  }


  public GridNode GetNode(int x, int y)
  {
    return _grid.GetGridObject(new Vector3Int(Math.Abs(x), Math.Abs(y)));
  }
  public GridNode GetNode(Vector3 pos)
  {
    return _grid.GetGridObject(new Vector3Int((int)Math.Abs(pos.x), (int)Math.Abs(pos.y)));
  }
  public GridNode GetNode(Vector3Int pos)
  {
    return _grid.GetGridObject(new Vector3Int(Math.Abs(pos.x), Math.Abs(pos.y)));
  }


  /// <summary>
  /// Get All neighbours node with char
  /// </summary>
  /// <param name="startNode"></param>
  /// <param name="isDiagonal"></param>
  /// <returns></returns>
  public List<GridNode> GetAllNeighboursWithChar(GridNode startNode, bool isDiagonal = true)
  {
    List<GridNode> neighbours = new();

    var leftNode = startNode.LeftNode;
    if (leftNode != null && leftNode.StateNode.HasFlag(StateNode.Occupied))
    {
      neighbours.Add(leftNode);
    }
    var rightNode = startNode.RightNode;
    if (rightNode != null && rightNode.StateNode.HasFlag(StateNode.Occupied))
    {
      neighbours.Add(rightNode);
    }
    var topNode = startNode.TopNode;
    if (topNode != null && topNode.StateNode.HasFlag(StateNode.Occupied))
    {
      neighbours.Add(topNode);
    }
    var bottomNode = startNode.BottomNode;
    if (bottomNode != null && bottomNode.StateNode.HasFlag(StateNode.Occupied))
    {
      neighbours.Add(bottomNode);
    }
    if (isDiagonal)
    {
      var bottomLeftNode = GetNode(startNode.x - 1, startNode.y - 1);
      if (bottomLeftNode != null && bottomLeftNode.StateNode.HasFlag(StateNode.Occupied))
      {
        neighbours.Add(bottomLeftNode);
      }
      var topLeftNode = GetNode(startNode.x - 1, startNode.y + 1);
      if (topLeftNode != null && topLeftNode.StateNode.HasFlag(StateNode.Occupied))
      {
        neighbours.Add(topLeftNode);
      }
      var bottomRightNode = GetNode(startNode.x + 1, startNode.y - 1);
      if (bottomRightNode != null && bottomRightNode.StateNode.HasFlag(StateNode.Occupied))
      {
        neighbours.Add(bottomRightNode);
      }
      var topRightNode = GetNode(startNode.x + 1, startNode.y + 1);
      if (topRightNode != null && topRightNode.StateNode.HasFlag(StateNode.Occupied))
      {
        neighbours.Add(topRightNode);
      }
    }

    return neighbours;
  }

}
