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
        
        double zF_s1 = length - zF_s0;          // force position
        double force = 8E2;

        double[] absolutePositions = new double[] {0, ratio*length, length}; // absolute position for the mathematical segment splitting

        int numRows = 12;   // define rows of the matrix
        int numCols = 12;   // define columns of the matrix
        
        

        // v(0) = 0
        double[] row1 = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        // w(0) = 0
        double[] row2 = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        // M(0) = 0
        double[] row3 = { 0, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        // v(zF_s0) = 0
        double[] row4 = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        // Delta_w() = w_s1(0) - w_s0(zF_s0) = 0
        double[] row5 = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        // Delta_phi() = phi_s1(0) - phi_s0(zF_s0) = 0
        double[] row6 = { -(1/(E*I))*Math.Pow(zF_s0,2)/2, -(1/(E*I))*zF_s0, (1/(E*I)), 0, 0, 0, 0, 0,  -1/(E*I), 0, 0, 0 };
        // Delta_v() = v_s1(0) - v_s0(zF_s0) = 0
        double[] row7 = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        // Delta_M() = M_s1(0) - M_s0(zF_s0) = 0
        double[] row8 = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        // Delta_N() = N_s1(0) - N_s0(zF_s0) = 0
        double[] row9 = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        // M(zF_s1) = 0
        double[] row10 = {0, 0, 0, 0, 0, 0, -zF_s1, -1, 0, 0, 0, 0 };
        // T(zF_s1) = F
        double[] row11 = {0, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0 };
        // N(zF_s1) = 0
        double[] row12 = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        // Set rows collection for the matrix
        List<double[]> rows = new List<double[]>
        {
            row1, row2, row3, row4, row5, row6, row7, row8, row9, row10, row11, row12
        };

        // define the vector
        double[] vector = new double[]
        {
           0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
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

