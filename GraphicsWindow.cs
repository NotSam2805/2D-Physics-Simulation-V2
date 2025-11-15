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

        private readonly object _shapeLock = new object();
        private readonly List<float[]> _shapeQueue = new List<float[]>();
        private readonly List<Shape> _shapes = new List<Shape>();

        public GraphicsWindow(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings()
            {
                Size = new OpenTK.Mathematics.Vector2i(width, height),
                Title = title
            })
        {
        }

        public void AddShape(float[] vertices)
        {
            lock (_shapeLock)
            {
                _shapeQueue.Add(vertices);
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1f);

            shader = new Shader("shader.vert", "shader.frag");
            renderer = new ShapeRenderer(shader);
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

            // Process queued shapes safely on the GL thread
            lock (_shapeLock)
            {
                foreach (var verts in _shapeQueue)
                {
                    _shapes.Add(new Shape(verts)); // <- creates VAO/VBO here on GL thread
                }
                _shapeQueue.Clear();
            }

            // now draw everything
            foreach (var shape in _shapes)
            {
                renderer.DrawShape(shape);
            }

            SwapBuffers();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }
    }

}
