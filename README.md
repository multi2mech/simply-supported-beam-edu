# Simply supported beam

### University of Rome Tor Vergata 
### [Multi2mech](www.multi2mech.com)

Educational project to investigate the deformation of a simply supported beam under an imposed loading condition.

This project lays the foundation for applying structural mechanics and Euler-Bernoulli beam theory in an augmented reality application (e.g., Unity), which is why it uses C#.

$`{d^2\over dz^2}\left(EI{d^2v\over dz^2}\right)=q`$

<img src="https://github.com/multi2mech/simply-supported-beam-edu/blob/9b54f5899f587631d48e46bd03442c58cc8505df/extra/beam.gif" width="250px" height="auto" />



## How to Run

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/your-repo.git
   ```

   2. 2.	Navigate into the project folder:
```bash
cd your-repo/src/YourProject
```

	3.	Restore dependencies and build:
```bash
    dotnet restore
dotnet build
```

	4.	Run the project:
```bash    
    dotnet run
```

## Application

The project is easily extendable to any beam, constraint, or loading condition by simply modifying the matrix and solution vector. Remember to group the coefficients into sets of six (c1 to c6) for each mathematical segment—these coefficients are derived from beam theory by integrating the governing equation four times.

The plotting is automatic; you only need to specify the values for E, I, and A. The coefficients are automatically grouped in sets of six, and you must also provide the sequence of z-coordinates at which the segments are interrupted. See the example for details.
