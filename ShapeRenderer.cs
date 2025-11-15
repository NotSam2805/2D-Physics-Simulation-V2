using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Physics_Simulation_V2
{
    using OpenTK.Graphics.OpenGL4;

    public class ShapeRenderer
    {
        private Shader shader;

        public ShapeRenderer(Shader shader)
        {
            this.shader = shader;
        }

        public void InitializeShape(Shape shape)
        {
            shape.Vao = GL.GenVertexArray();
            shape.Vbo = GL.GenBuffer();

            GL.BindVertexArray(shape.Vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, shape.Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer,
                shape.Vertices.Length * sizeof(float),
                shape.Vertices,
                BufferUsageHint.DynamicDraw);

            if (shape.Indices != null)
            {
                shape.Ebo = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, shape.Ebo);
                GL.BufferData(BufferTarget.ElementArrayBuffer,
                    shape.Indices.Length * sizeof(uint),
                    shape.Indices,
                    BufferUsageHint.StaticDraw);
            }

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);
        }

        public void UpdateShapeVertices(Shape shape)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, shape.Vbo);
            GL.BufferSubData(BufferTarget.ArrayBuffer,
                IntPtr.Zero,
                shape.Vertices.Length * sizeof(float),
                shape.Vertices);
        }

        public void DrawShape(Shape shape)
        {
            shader.Use();
            GL.BindVertexArray(shape.Vao);

            if (shape.Indices != null)
                GL.DrawElements(PrimitiveType.Triangles, shape.Indices.Length, DrawElementsType.UnsignedInt, 0);
            else
                GL.DrawArrays(PrimitiveType.Triangles, 0, shape.Vertices.Length / 3);
        }
    }

}
