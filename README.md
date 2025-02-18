# Simply supported beam

### University of Rome Tor Vergata - [Multi2mech](www.multi2mech.com)

Educational project to investigate the deformation of a simply supported beam under an imposed loading condition.

This project lays the foundation for applying structural mechanics and Euler-Bernoulli beam theory in an augmented reality application (e.g., Unity), which is why it uses C#.

$`{d^2\over dz^2}\left(EI{d^2v\over dz^2}\right)=q`$

## Loading scenario

Two mathematical segments between \[`0`,`ratio*length`\] and \[`ratio*length`,`length`\]. Hinge in `0`, roller in `ratio*length` and force in `ratio*length`:

<p align="center">
  <img src="https://github.com/multi2mech/simply-supported-beam-edu/blob/9b54f5899f587631d48e46bd03442c58cc8505df/extra/beam.gif" width="250px" height="auto" />
</p>

## Prerequisites
 
- *.NET SDK*. This project is built using [.NET. SDK download](https://dotnet.microsoft.com/en-us/download) and install the latest stable version of the .NET SDK (e.g., .NET 6.0 or later). After cloning the repository, run `dotnet restore` to install the required NuGet packages.

- *Visual Studio Code*. For development and debugging, use an IDE that supports C#. If you choose VS Code, install the C# extension by Microsoft. 

- *Replit* (alrternative IDE). From [replit.com](https://replit.com) clone this repository. 
	
- *Unity* (Optional). If you plan to integrate or test the AR components, make sure you have Unity installed. See the [AR beam repostory]().

- Any additional libraries referenced in the project file (`.csproj`) will be restored automatically via dotnet restore.

## How to Run

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/your-repo.git
   ```

2. Navigate into the project folder:
    ```bash
    cd path/to/your/repo/
    ```

3. Restore dependencies and build:
    ```bash
    dotnet restore
    dotnet build
    ```

4. Run the project:
    ```bash    
    dotnet run
    ```

## Application

The project is easily extendable to any beam, constraint, or loading condition by simply modifying the matrix and solution vector. Remember to group the coefficients into sets of six ($`c_1`$ to $`c_6`$) for each mathematical segment—these coefficients are derived from beam theory by integrating the governing equation four times.

The plotting is automatic; you only need to specify the values for `E`, `I`, and `A`. The coefficients are automatically grouped in sets of six, and you must also provide the sequence of z-coordinates at which the segments are interrupted. See the example for details. 


Out is the image [plot.png](https://github.com/multi2mech/simply-supported-beam-edu/blob/6bd93d1dc326fb14b590996f13b45fdf164d1615/plot.png):

<img src="https://github.com/multi2mech/simply-supported-beam-edu/blob/6bd93d1dc326fb14b590996f13b45fdf164d1615/plot.png" width="250px" height="auto" />