public class Calculator : ICalculator
{
    public double VectorX;
    public double VectorY;

    public double A { get; set; }
    public double B { get; set; }


    public double Add(double a, double b)
    {
        return a + b;
    }

    public double Divide(double a, double b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException("Cannot divide by 0!");
        }
        return a / b;
    }

    public double Multiply(double a, double b)
    {
        return a * b;
    }

    public double Power(double a, double b)
    {
        return Math.Pow(a, b);
    }

    public double Subtract(double a, double b)
    {
        return a - b;
    }

    public void VectorCoords(double x, double y)
    {
        Console.WriteLine($"Vector A = {x}\nVector B = {y}\nX Coords: {VectorX}, Y Coords: {VectorY}");
    }

    public string Pythagoras()
    {
        double sum = PythagorasSum(A, B);
        return $"The sum of a evenly angled triangle where every angle is 90 degrees is: {sum}\nA={A}, B={B}\na^2+b^2=c^2";
    }

    private double PythagorasSum(double a, double b)
    {
        double C = Math.Pow(a, 2) + Math.Pow(b, 2);
        return Math.Sqrt(C);
    }

    private double CalculateScalarProduct(double angle)
    {
        double scalarProduct = Math.Abs(VectorX) * Math.Abs(VectorY) * Math.Cos(angle);
        if (scalarProduct == 0)
        {
            return scalarProduct;
        }
        else
        {
            return scalarProduct;
        }
    }
}