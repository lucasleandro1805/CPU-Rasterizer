# CPURasterizer

<h4>A simple CPU Rasterizer made with c#.</h4>

![](https://github.com/lucasleandro1805/CPU-Rasterizer/blob/master/images/showcase.gif)

<p>
An OpenGL-like triangle rasterizer designed to run on the CPU only, with support for Vertex & Fragment Shaders.

Features:(<br>)
-It has the same nomenclature as OpenGL.(<br>)
-It has Vertex and Fragment shaders.(<br>)
-Ultra parallelized.(<br>)
-Memory-efficient.(<br>)
-Renders to a Bitmap, which can be modified to support quick bitmap switching (thus creating a framebuffer system and allowing off-screen rendering).(<br>)
-Uniforms, Attributes & Varyings.(<br>)
-Simple.(<br>)

I developed this project with the aim of studying and learning more about rasterizers and shaders. Also I would like to study more about parallelization techniques in C# and I found this a great project to test it.

Performance:
My CPU: Intel Core I5 ​​8250U 1.6ghz
Running a screen of 300x300:
Pixels per second: something around 2.5 million.

I found the performance very pleasant to be my first project of these.

The operation is very similar to OpenGL.
You must create shaders (VertexShader and FragmentShaders).
Then create a ShaderProgram and attach the shaders.

Your shader can contain Uniforms, Attributes and Varyings (Just like OpenGL).

I made an example inside the project for you to analyze.
It is in the Examples/Cube/ folder.
</p>