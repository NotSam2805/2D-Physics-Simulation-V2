using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace _2D_Physics_Simulation_V2
{
    public class GraphicsWindow : GameWindow
    {
        private Shader shader;
        private ShapeRenderer renderer;
        private List<Shape> shapes = new();

        public GraphicsWindow(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings()
            {
                Size = new OpenTK.Mathematics.Vector2i(width, height),
                Title = title
            })
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1f);

            shader = new Shader("shader.vert", "shader.frag");
            renderer = new ShapeRenderer(shader);

            // Example shape: Rectangle
            AddShape(
                new float[]
                {
                    0.5f,  0.5f, 0f,
                    0.5f, -0.5f, 0f,
                   -0.5f, -0.5f, 0f,
                   -0.5f,  0.5f, 0f
                },
                new uint[] { 0, 1, 3, 1, 2, 3 }
            );
        }

        public Shape AddShape(float[] vertices, uint[]? indices = null)
        {
            var shape = new Shape(vertices, indices);
            shapes.Add(shape);
            renderer.InitializeShape(shape);
            return shape;
        }

        public void UpdateShapeVertices(Shape shape, float[] newVerts)
        {
            shape.Vertices = newVerts;
            renderer.UpdateShapeVertices(shape);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            foreach (var s in shapes)
                renderer.DrawShape(s);

            SwapBuffers();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }
    }

}
