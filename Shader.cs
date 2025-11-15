using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace _2D_Physics_Simulation_V2
{
    public class Shader : IDisposable
    {
        public int Handle;

        public Shader(string vertexPath, string fragmentPath)
        {
            string vertexCode = File.ReadAllText(vertexPath);
            string fragmentCode = File.ReadAllText(fragmentPath);

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexCode);
            GL.CompileShader(vertexShader);
            CheckCompileErrors(vertexShader, "VERTEX");

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentCode);
            GL.CompileShader(fragmentShader);
            CheckCompileErrors(fragmentShader, "FRAGMENT");

            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            GL.LinkProgram(Handle);
            CheckLinkErrors(Handle);

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        private void CheckCompileErrors(int shader, string type)
        {
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);

            if (success == 0)
            {
                string log = GL.GetShaderInfoLog(shader);
                throw new Exception($"Shader compile error ({type}):\n{log}");
            }
        }

        private void CheckLinkErrors(int program)
        {
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);

            if (success == 0)
            {
                string log = GL.GetProgramInfoLog(program);
                throw new Exception($"Shader link error:\n{log}");
            }
        }

        public void Dispose()
        {
            GL.DeleteProgram(Handle);
        }
    }
}
