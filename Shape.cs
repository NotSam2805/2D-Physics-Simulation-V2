using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Physics_Simulation_V2
{
    public class Shape
    {
        public float[] Vertices;
        public uint[]? Indices;

        public int Vao;
        public int Vbo;
        public int Ebo;

        public Shape(float[] vertices, uint[]? indices = null)
        {
            Vertices = vertices;
            Indices = indices;
        }
    }

}
