using ScottPlot;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

#nullable enable
public class PlotHelper
{
    public static void PlotFunction(List<ResultsGroup> groups)
    {
        ScottPlot.Multiplot multiplot = new();
        double dx = 0.1;
        
        ScottPlot.Plot displacement  = PlotDisplacement( groups, dx);
        ScottPlot.Plot N  = PlotN( groups, dx);
         
        ScottPlot.Plot T  = PlotT( groups, dx);
        ScottPlot.Plot M  = PlotM( groups, dx);
        
        
        multiplot.AddPlot(displacement);
        multiplot.AddPlot(N);
        multiplot.AddPlot(T);
        multiplot.AddPlot(M);

        // Save plot as PNG
        string imagePath = "plot.png";
        multiplot.SavePng(imagePath, 600, 1200);
        OpenImage(imagePath);
        
        
        }

    public static void OpenImage(string imagePath){
        Console.WriteLine();
        Console.WriteLine($"Immagine generata nel file: {imagePath}");
        Process.Start(new ProcessStartInfo
        {
            FileName = Path.GetFullPath(imagePath),
            UseShellExecute = true
        });
    }
    public static ScottPlot.Plot PlotDisplacement(List<ResultsGroup> groups, double dx)
    {
        return GeneratePlot(
        groups,
        dx,
        computeY: (x, group) => v(x, group), // Specific logic for M
        title : "v(z)",
        yLabel : "v(z)",
        lineColor : Colors.Navy,
        InvertedYAxisQ : true,
        FillQ : false,
        computeX: (x, group) => w(x, group)
        );

        }

    public static ScottPlot.Plot PlotM(List<ResultsGroup> groups, double dx)
    {
        return GeneratePlot(
            groups,
            dx,
            computeY: (x, group) => M(x, group), // Specific logic for M
            title : "M",
            yLabel : "M(z)",
            lineColor : Colors.DarkGreen,
            FillQ : true,
            InvertedYAxisQ : true);
    }


    public static ScottPlot.Plot PlotT(List<ResultsGroup> groups, double dx)
    {
        return GeneratePlot(
            groups,
            dx,
            computeY: (x, group) => T(x, group), // Specific logic for T
            title : "T",
            yLabel : "T(z)",
            lineColor : Colors.DarkGreen,
            FillQ: true,
            InvertedYAxisQ : false);
    }

    public static ScottPlot.Plot PlotN(List<ResultsGroup> groups, double dx)
    {
        return GeneratePlot(
            groups,
            dx,
            computeY: (x, group) => N(x, group), // Specific logic for M
            title : "N",
            yLabel : "N(z)",
            lineColor : Colors.DarkGreen,
            FillQ: true,
            InvertedYAxisQ : false);
    }

    public static ScottPlot.Plot GeneratePlot(
        List<ResultsGroup> groups,
        double dx,
        Func<double, ResultsGroup, double> computeY, // Delegate for Y computation
        string title,
        string yLabel,
        Color lineColor,
        bool InvertedYAxisQ,
        bool FillQ,
        Func<double, ResultsGroup, double>? computeX = null // Optional delegate for X computation
        )
    {
        ScottPlot.Plot plot = new();

        double minX = 0;
        double maxX = 0;
        double maxY = 0;
        double minY = 0;
        double max_xAxis = 0;
        double max_yAxis = 0;
        double min_yAxis = 0;

        foreach (var group in groups)
        {
            maxX = group.ZEnd - group.ZStart;
            int nPoints = (int)((maxX - minX) / dx) + 1;
            double[] dataX = new double[nPoints];
            double[] dataY = new double[nPoints];
            double[] dataY_Zero = new double[nPoints];

            for (int i = 0; i < nPoints; i++)
            {
                dataX[i] = i * dx; // x values
                dataY[i] = computeY(dataX[i], group); // Use the provided computation logic
                dataX[i] += group.ZStart; // Translate x values with respect to each segment
                // Apply computeX if provided
                if (computeX != null)
                {
                    dataX[i] += computeX(dataX[i], group); // Adjust x values based on computeX
                }
                dataY_Zero[i] = 0;
            }

            maxY = dataY.Max();
            minY = dataY.Min();
            if (minY >= 0 - 0.1*maxY)
            {
                minY = -0.1 * maxY;
            }
            if (maxY <= 0 + 0.1*Math.Abs(maxY))
            {
                maxY = 0.1*Math.Abs(maxY);
            }

            // Add scatter plot with specified line style and colors
            plot.Add.Scatter(dataX, dataY_Zero, Colors.Gray);
            var scatterPlot = plot.Add.Scatter(dataX, dataY, lineColor);
            if (FillQ){
                scatterPlot.FillY = true;
                scatterPlot.FillYColor = scatterPlot.Color.WithAlpha(.2);
            }

            max_xAxis = Math.Max(max_xAxis, group.ZEnd);
            max_yAxis = Math.Max(max_yAxis, maxY);
            min_yAxis = Math.Min(min_yAxis, minY);
        }

        min_yAxis = 1.05*min_yAxis;
        max_yAxis = 1.05*max_yAxis;
        
        // Customize axes
        plot.Axes.Right.FrameLineStyle.Width = 0;
        plot.Axes.Top.FrameLineStyle.Width = 0;
        if (InvertedYAxisQ) {
            plot.Axes.SetLimits(minX, max_xAxis, max_yAxis, min_yAxis);
        }
        else{
            plot.Axes.SetLimits(minX, max_xAxis, min_yAxis, max_yAxis);
        }
        
        plot.Axes.SetupMultiplierNotation(plot.Axes.Left);
        //plot.Axes.SetupMultiplierNotation(plot.Axes.Bottom);
        
        // Add titles and labels
        plot.Title(title);
        plot.XLabel("z");
        plot.YLabel(yLabel);

        return plot;
    }
    
    public static double v(double x, ResultsGroup group)
    {
        double c1 = group.c1;
        double c2 = group.c2;
        double c3 = group.c3;
        double c4 = group.c4;
        double c5 = group.c5;
        double c6 = group.c6;

        double E = group.E;
        double I = group.I;
        double A = group.A;
        double qIIIInt = group.qIIIInt;
        // Example computation using E and I
        return (1/(E*I)) * (qIIIInt + c1*Math.Pow(x, 3) / 6 + c2*Math.Pow(x, 2) / 3 + c3*x +c4);
    }

    public static double w(double x, ResultsGroup group)
    {
        double c1 = group.c1;
        double c2 = group.c2;
        double c3 = group.c3;
        double c4 = group.c4;
        double c5 = group.c5;
        double c6 = group.c6;

        double E = group.E;
        double I = group.I;
        double A = group.A;
        double qIIIInt = group.qIIIInt;
        double pIInt = group.pIInt;
        // Example computation using E and I
        return -(1/(E*A)) * (pIInt + c5*x + c6);
    }

    public static double M(double x, ResultsGroup group)
    {
        double c1 = group.c1;
        double c2 = group.c2;

        double qIInt = group.qIInt;
        // Example computation using E and I
        return -  (qIInt + c1*x + c2);
    }

    public static double T(double x, ResultsGroup group)
    {
        double c1 = group.c1;
        double qInt = group.qInt;
        
        return -  (qInt + c1);
    }

    public static double N(double x, ResultsGroup group)
    {
        double c5 = group.c5;
        double pInt = group.pInt;
        
        return -  (pInt + c5);
    }




    public class ResultsGroup
    {
        public double[] Coefficients { get; set; } // The list of coefficients for this group
        public double ZStart { get; set; }
        public double ZEnd { get; set; }
        public double E { get; set; }
        public double I { get; set; }
        public double A { get; set; }
        public double qIIIInt { get; set; }
        public double qIIInt { get; set; }
        public double pIInt { get; set; }
        public double qIInt { get; set; }
        public double qInt { get; set; }
        public double pInt { get; set; }
        public double c1 { get; set; }
        public double c2 { get; set; }
        public double c3 { get; set; }
        public double c4 { get; set; }
        public double c5 { get; set; }
        public double c6 { get; set; }

        public ResultsGroup(double[] coefficients, double zStart, double zEnd, double Ein, double Iin, double Ain)
        {
            Coefficients = coefficients;
            c1 = Coefficients[0];
            c2 = Coefficients[1];
            c3 = Coefficients[2];
            c4 = Coefficients[3];
            c5 = Coefficients[4];
            c6 = Coefficients[5];
            ZStart = zStart;
            ZEnd = zEnd;
            E = Ein;
            I = Iin;
            A = Ain;
            qInt = 0;
            qIInt= 0;
            qIIInt = 0;
            qIIIInt= 0;
            pInt= 0;
            pIInt= 0;
        }
    }

    public static List<ResultsGroup> GetSolutionGroups(double[] solution, double[] z_list, double E, double I, double A)
    {
        if (solution == null || solution.Length == 0)
            throw new ArgumentException("Segments cannot be null or empty.", nameof(solution));

       
        List<ResultsGroup> groups = new List<ResultsGroup>();
        int groupSize = 6;
        int noGroupd = solution.Length / groupSize;
        for (int i = 0; i < noGroupd; i++)
        {
            // Extract coefficients for the current segment
            double[] coefficients = solution
                .Skip(i * groupSize)
                .Take(groupSize)
                .ToArray();

            // Create a group using segment's InitialPosition and FinalPosition
            var group = new ResultsGroup(coefficients, z_list[i], z_list[i+1], E, I, A);
            groups.Add(group);
        }

        return groups;
    }

    
}



