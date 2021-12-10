using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GSStorm.RPG.Engine{
	/// <summary>
	/// Map factory type mesh.
	/// This factory generate the map 
	/// as single mesh, using the marching-square
	/// algorithm, mesh will exist for ground(ON)
	/// and empty for water(OFF)
	/// </summary>
	public class MapFactoryTypeMesh
	{

		#region MapNode Classes
		/// <summary>
		/// Represents a node on a mesh, 
		/// each square has eight nodes
		/// </summary>
		public class MapNode{
			public Vector3 Position;
			public int VertextIndex = -1; //for creating mesh

			public MapNode(Vector3 pos){
				Position = pos;
			}
		}

		/// <summary>
		/// Corner node of a square,
		/// each control node will control
		/// the edge nodes on its right and above
		/// </summary>
		public class MapControlNode : MapNode{
			public TileValue NodeValue; //what terrain is on this node

			public MapNode Above, Right; //each control node, controls the two edge nodes to its top and right

			public MapControlNode(Vector3 pos, TileValue nodeValue, float squareSize) : base(pos){
				NodeValue = nodeValue;

				//create the controlled edge nodes;
				Above = new MapNode(pos +  new Vector3(0,1,0) * squareSize/2);
				Right = new MapNode(pos + new Vector3(1,0,0) * squareSize/2);
			}
		}

		/// <summary>
		/// Each Square contains 4 control nodes
		/// and 4 edge nodes
		/// </summary>
		public class MapSquare{
			public MapControlNode TopLeft, TopRight, BottomLeft, BottomRight;
			public MapNode CenterTop, CenterRight, CenterBottom, CenterLeft;

			public int Configuration = 0; //0-15, 0000-1111, 16 different binary configurations

			public MapSquare(MapControlNode topLeft, MapControlNode topRight, MapControlNode bottomRight, MapControlNode bottomLeft){
				TopLeft = topLeft;
				TopRight = topRight;
				BottomLeft = bottomLeft;
				BottomRight = bottomRight;

				CenterTop = TopLeft.Right;
				CenterRight = BottomRight.Above;
				CenterBottom = BottomLeft.Right;
				CenterLeft = BottomLeft.Above;

				if(TopLeft.NodeValue == TileValue.GROUND)
					Configuration += 8;

				if(TopRight.NodeValue == TileValue.GROUND)
					Configuration += 4;

				if(BottomLeft.NodeValue == TileValue.GROUND)
					Configuration += 1;

				if(BottomRight.NodeValue == TileValue.GROUND)
					Configuration += 2;

			}
		}
		#endregion

		#region Private Properties
		List<Vector3> _vertices;
		List<int> 	  _triangles;
		MapSquare[,] _squares;
		#endregion


		#region Public Properties
		public Mesh GeneratedMapMesh {
			get;
			private set;
		}
		#endregion
	
		public IEnumerator GenerateMesh(int[,] map, float squareSize){
			//Initialize the vertices and triangles
			_triangles = new List<int> ();
			_vertices = new List<Vector3> ();

			//Generate the actual map, base on the data map[,]
			int xCount = map.GetLength(0);
			int yCount = map.GetLength(1);

			float mapWidth = xCount * squareSize;
			float mapHeight = yCount * squareSize;

			//Create the control node
			MapControlNode[,] controlNodes = new MapControlNode[xCount, yCount];
			for (int x = 0; x < xCount; x++) {
				for (int y = 0; y < yCount; y++) {
					//Create the control node from the bottom left position
					Vector3 pos = new Vector3( - mapWidth/2 + x * squareSize , -mapHeight/2 + y * squareSize , 0 );
					//Debug.Log ("Create control node at : " + x + "," + y + "at map value" + MapData [x, y]);
					controlNodes[x,y] = new MapControlNode(pos, map[x,y] == 1 ? TileValue.GROUND : TileValue.WATER, squareSize); 
				}
			}

			//Create the squares, and triangulate them
			_squares = new MapSquare[xCount - 1, yCount - 1];
			for (int x = 0; x < xCount - 1; x++) {
				for (int y = 0; y < yCount - 1; y++) {
					//we index each square by the bottom left control node
					_squares[x,y] = new MapSquare(controlNodes[x,y+1],controlNodes[x+1,y+1], controlNodes[x+1,y], controlNodes[x,y]);
					TriangulateSquare (_squares [x, y]);
				}
			}


			//Generate the actuall mesh
			GeneratedMapMesh = new Mesh();
			GeneratedMapMesh.vertices = _vertices.ToArray();
			GeneratedMapMesh.triangles = _triangles.ToArray();
			GeneratedMapMesh.RecalculateNormals ();
			//
			Debug.Log ("The generated mesh has vertices : " + _vertices.Count);
			Debug.Log ("The generated mesh has triangles : " + _triangles.Count);
		
			yield return null;
		}

		void TriangulateSquare(MapSquare square){
			switch (square.Configuration) {
			case 0:
				break;

				//1 point on
			case 1:
				MeshFromPoints (square.CenterBottom, square.BottomLeft, square.CenterLeft);
				break;
			case 2:
				MeshFromPoints (square.CenterRight, square.BottomRight, square.CenterBottom);
				break;
			case 4:
				MeshFromPoints (square.CenterTop, square.TopRight, square.CenterRight);
				break;
			case 8:
				MeshFromPoints (square.TopLeft, square.CenterTop, square.CenterLeft);
				break;

				//2 points on
			case 3:
				MeshFromPoints (square.CenterRight, square.BottomRight, square.BottomLeft, square.CenterLeft);
				break;
			case 6:
				MeshFromPoints (square.CenterTop, square.TopRight, square.BottomRight, square.CenterBottom);
				break;
			case 9:
				MeshFromPoints (square.TopLeft, square.CenterTop, square.CenterBottom, square.BottomLeft);
				break;
			case 12:
				MeshFromPoints (square.TopLeft, square.TopRight, square.CenterRight, square.CenterLeft);
				break;
			case 5:
				MeshFromPoints (square.CenterTop, square.TopRight, square.CenterRight, square.CenterBottom, square.BottomLeft, square.CenterLeft);
				break;
			case 10:
				MeshFromPoints (square.TopLeft, square.CenterTop, square.CenterRight, square.BottomRight, square.CenterBottom, square.CenterLeft);
				break;

				//3 points on
			case 7:
				MeshFromPoints (square.CenterTop, square.TopRight, square.BottomRight, square.BottomLeft, square.CenterLeft);
				break;
			case 11:
				MeshFromPoints (square.TopLeft, square.CenterTop, square.CenterRight, square.BottomRight, square.BottomLeft);
				break;
			case 13:
				MeshFromPoints (square.TopLeft, square.TopRight, square.CenterRight, square.CenterBottom, square.BottomLeft);
				break;
			case 14:
				MeshFromPoints (square.TopLeft, square.TopRight, square.BottomRight, square.CenterBottom, square.CenterLeft);
				break;

				//4 points on
			case 15:
				MeshFromPoints (square.TopLeft, square.TopRight, square.BottomRight, square.BottomLeft);
				break;
			
			}
		}

		void MeshFromPoints(params MapNode[] points){
			//To create a mesh, 1. assign the vertexes, 2.create the triangles

			AssignVertices(points);

			//Create the triangles, put them in correct order in a list
			if (points.Length >= 3)
				CreateTriangle (points [0], points [1], points [2]);

			if (points.Length >= 4)
				CreateTriangle (points [0], points [2], points [3]);

			if (points.Length >= 5)
				CreateTriangle (points [0], points [3], points [4]);

			if (points.Length >= 6)
				CreateTriangle (points [0], points [4], points [5]);

		}

		void AssignVertices(MapNode[] points){
			for (int i = 0; i < points.Length; i++) {
				if (points [i].VertextIndex == -1) {
					points [i].VertextIndex = _vertices.Count;
					_vertices.Add (points [i].Position);
				}
			}
		}

		void CreateTriangle(MapNode a, MapNode b, MapNode c){
			_triangles.Add (a.VertextIndex);
			_triangles.Add (b.VertextIndex);
			_triangles.Add (c.VertextIndex);
		}
	
		public void DrawGizmos(){
			if (_squares != null) {
				for (int x = 0; x < _squares.GetLength(0); x++) {
					for (int y = 0; y < _squares.GetLength(1); y++) {
						//Draw the map control nodes	
						Gizmos.color = _squares [x, y].TopLeft.NodeValue == TileValue.GROUND ? Color.black : Color.white; 
						Gizmos.DrawCube (_squares [x, y].TopLeft.Position, Vector3.one * .4f);

						Gizmos.color = _squares [x, y].TopRight.NodeValue == TileValue.GROUND ? Color.black : Color.white; 
						Gizmos.DrawCube (_squares [x, y].TopRight.Position, Vector3.one * .4f);

						Gizmos.color = _squares [x, y].BottomLeft.NodeValue == TileValue.GROUND ? Color.black : Color.white; 
						Gizmos.DrawCube (_squares [x, y].BottomLeft.Position, Vector3.one * .4f);

						Gizmos.color = _squares [x, y].BottomRight.NodeValue == TileValue.GROUND ? Color.black : Color.white; 
						Gizmos.DrawCube (_squares [x, y].BottomRight.Position, Vector3.one * .4f);

						//Draw the edge nodes
						Gizmos.color = Color.grey;

						Gizmos.DrawCube (_squares [x, y].CenterTop.Position, Vector3.one * .15f);
						Gizmos.DrawCube (_squares [x, y].CenterLeft.Position, Vector3.one * .15f);
						Gizmos.DrawCube (_squares [x, y].CenterRight.Position, Vector3.one * .15f);
						Gizmos.DrawCube (_squares [x, y].CenterBottom.Position, Vector3.one * .15f);

					}
				}
			}
		}

	}
}
