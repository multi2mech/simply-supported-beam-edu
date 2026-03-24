using System;
using System.Collections.Generic;


class Program
{
    static void Main()
    {
        double E = 200E9;                 // 200 GPA
        double W = 0.2;                   // 20 cm
        double H = 0.4;                   // 40 cm
        double A = W*H;                   // area
        double I = W*Math.Pow(H,3)/12;    // inertia

        double length = 10;              // beam length

        double ratio = 0.5;             // roller position ratio
        double zF_s0 = ratio * length;  // roller position
        
        double load_ratio = 0.7;
        double zF_s1 = length*load_ratio - zF_s0;          // local force position
        double force = 8E2;

        double zF_s2 = length - zF_s0 - zF_s1;          // free extremity position

        double[] absolutePositions = new double[] {0, ratio*length, load_ratio*length, length}; // absolute position for the mathematical segment splitting

        int numRows = 18;   // define rows of the matrix
        int numCols = 18;   // define columns of the matrix
        


                
        // row1: same as before   -> w(0)=0
        double[] row1 = {0, 0, 0, 1/(E*I), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

        // row2: kept as reference condition v(0)=0
        double[] row2 = {0, 0, 0, 0, 0, -1/(E*A), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

        // row3: CHANGED   M(0)=0  ->  phi(0)=0
        double[] row3 = {0, 0, 1/(E*I), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

        // row4: same as before   -> w(zF_s0)=0 at middle support
        double[] row4 = {1/(E*I)*Math.Pow(zF_s0,3)/6, 1/(E*I)*Math.Pow(zF_s0,2)/2, 1/(E*I)*zF_s0, 1/(E*I), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

        // row5: same as before   -> axial displacement continuity at zF_s0
        double[] row5 = {0, 0, 0, 0, 1/(E*A)*zF_s0, 1/(E*A), 0, 0, 0, 0, 0, -1/(E*A), 0, 0, 0, 0, 0, 0};

        // row6: same as before   -> slope continuity at zF_s0
        double[] row6 = {1/(E*I)*Math.Pow(zF_s0,2)/2, 1/(E*I)*zF_s0, 1/(E*I), 0, 0, 0, 0, 0, -1/(E*I), 0, 0, 0, 0, 0, 0, 0, 0, 0};

        // row7: same as before   -> displacement continuity at zF_s0
        double[] row7 = {-1/(E*I)*Math.Pow(zF_s0,3)/6, -1/(E*I)*Math.Pow(zF_s0,2)/2, -1/(E*I)*zF_s0, -1/(E*I), 0, 0, 0, 0, 0, 1/(E*I), 0, 0, 0, 0, 0, 0, 0, 0};

        // row8: same as before   -> moment continuity at zF_s0
        double[] row8 = {1*zF_s0, 1, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

        // row9: same as before   -> axial force continuity at zF_s0
        double[] row9 = {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, 0};

        // row10-row15: same as before
        double[] row10 = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1/(E*A)*zF_s1, 1/(E*A), 0, 0, 0, 0, 0, -1/(E*A)};
        double[] row11 = {0, 0, 0, 0, 0, 0, 1/(E*I)*Math.Pow(zF_s1,2)/2, 1/(E*I)*zF_s1, 1/(E*I), 0, 0, 0, 0, 0, -1/(E*I), 0, 0, 0};
        double[] row12 = {0, 0, 0, 0, 0, 0, -1/(E*I)*Math.Pow(zF_s1,3)/6, -1/(E*I)*Math.Pow(zF_s1,2)/2, -1/(E*I)*zF_s1, -1/(E*I), 0, 0, 0, 0, 0, 1/(E*I), 0, 0};
        double[] row13 = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, -1, 0};
        double[] row14 = {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0};
        double[] row15 = {0, 0, 0, 0, 0, 0, 1*zF_s1, 1, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0};

        // row16: same as original row16   -> N(zF_s2)=0  (axially free right end)
        double[] row16 = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0};

        // row17: CHANGED   was T(zF_s2)=0, now M(zF_s2)=0
        double[] row17 = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1*zF_s2, -1, 0, 0, 0, 0};

        // row18: CHANGED   w(zF_s2)=0 at right roller
        double[] row18 = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            1/(E*I)*Math.Pow(zF_s2,3)/6,
            1/(E*I)*Math.Pow(zF_s2,2)/2,
            1/(E*I)*zF_s2,
            1/(E*I),
            0, 0};


        // Set rows collection for the matrix
        List<double[]> rows = new List<double[]>
        {
            row1, row2, row3, row4, row5, row6, row7, row8, row9, row10, row11, row12, row13, row14, row15, row16, row17, row18
        };

        double[] vector = new double[]
        {
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -force, 0, 0, 0, 0
        };
                
        // reshape the matrix from [][] to [,]
        double[,] matrix = new double[numRows, numCols];
        for (int i = 0; i < rows.Count; i++)
        {
            for (int j = 0; j < rows[i].Length; j++)
            {
                matrix[i, j] = rows[i][j];
            }
        }
        

        double[] solution = MatrixSolver.Solve(matrix, vector);      // solve Ax = b
        
       // plot the beam
        List<PlotHelper.ResultsGroup> groups = PlotHelper.GetSolutionGroups(solution, absolutePositions, E, I, A); 
        PlotHelper.PlotBeamFunction(groups);

    }


    

    
}

