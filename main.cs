using System;
using System.Collections.Generic;


class Program
{
    static void Main()
    {
        //var segments = SegmentList.LoadSegment();
        //var body = new Body { Segments = segments };
        //var eqns = new EQ_IV();

        // Assemble the matrix
        //var assembler = new MatrixAssembler();
        //MatrixResult result = assembler.AssembleMatrix(body, eqns);
        // solve Ax=b
        // double[] solution = MatrixSolver.Solve(result.Matrix, result.Vector);
        double length = 10;
        double ratio = 0.5;

        double zF_s0 = ratio * length;
        double zF_s1 = length;
        double E = 1;
        double I = 1;
        double A = 1;
        double force = 800;

        int numRows = 12;
        int numCols = 12;
        
        // Define rows of the matrix

        // v(0) = 0
        double[] row1 = { 0, 0, 0, 1/(E*I), 0, 0, 0, 0, 0, 0, 0, 0 };
        // w(0) = 0
        double[] row2 = {0, 0, 0, 0, 0, -1/(E*A), 0, 0, 0, 0, 0, 0 };
        // M(0) = 0
        double[] row3 = { 0, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        // v(zF_s0) = 0
        double[] row4 = { (1/(E*I))*Math.Pow(zF_s0,3)/6,  (1/(E*I))*Math.Pow(zF_s0,2)/2, (1/(E*I))*zF_s0,  (1/(E*I)), 0, 0, 0, 0, 0, 0, 0, 0 };
        // Delta_w() = w_s1(0) - w_s0(zF_s0) = 0
        double[] row5 = { 0, 0, 0, 0, -(-1/(E*A)) *zF_s0,  -(-1/(E*A)), 0, 0, 0, 0, 0, (-1/(E*A)) };
        // Delta_phi() = phi_s1(0) - phi_s0(zF_s0) = 0
        double[] row6 = { -(1/(E*I))*Math.Pow(zF_s0,2)/2, -(1/(E*I))*zF_s0, -(1/(E*I)), 0, 0, 0, 0, 0,  (1/(E*I)), 0, 0, 0 };
        // Delta_v() = v_s1(0) - v_s0(zF_s0) = 0
        double[] row7 = {-(1/(E*I))*Math.Pow(zF_s0,3)/6, -(1/(E*I))*Math.Pow(zF_s0,2)/2, -(1/(E*I))*zF_s0,  -(1/(E*I)), 0, 0, 0, 0, 0,  (1/(E*I)), 0, 0 };
        // Delta_M() = M_s1(0) - M_s0(zF_s0) = 0
        double[] row8 = { -(-zF_s0), -(-1), 0, 0, 0, 0, 0, -1, 0, 0, 0, 0 };
        // Delta_N() = N_s1(0) - N_s0(zF_s0) = 0
        double[] row9 = { 0, 0, 0, 0, -(-1), 0, 0, 0, 0, 0, -1, 0 };
        // M(zF_s1) = 0
        double[] row10 = {0, 0, 0, 0, 0, 0, -zF_s1, -1, 0, 0, 0, 0 };
        // T(zF_s1) = 0
        double[] row11 = {0, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0 };
        // N(zF_s1) = 0
        double[] row12 = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0 };
        // Set rows collection
        List<double[]> rows = new List<double[]>
        {
            row1, row2, row3, row4, row5, row6, row7, row8, row9, row10, row11, row12
        };

        // Define the vector
        double[] vector = new double[]
        {
           0, 0, 0, 0, 0, 0, 0, 0, 0, 0, force, 0
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
        // Solve Ax = b
        double[] solution = MatrixSolver.Solve(matrix, vector);

        // Print the solution vector
        Console.WriteLine("Soluzione:");

        // Get groups
        List<PlotHelper.ResultsGroup> groups = PlotHelper.GetSolutionGroups(solution, [0, zF_s0, zF_s1], E, I, A);

        // Print groups
        foreach (var group in groups)
        {
            Console.WriteLine($"Da z: {group.ZStart} a z: {group.ZEnd}, coefficienti: {string.Join(", ", group.Coefficients)}");
            //Console.WriteLine($"zStart: {group.ZStart}, zEnd: {group.ZEnd}");
            //Console.WriteLine();
        }

        PlotHelper.PlotFunction(groups);

    }


    

    
}

