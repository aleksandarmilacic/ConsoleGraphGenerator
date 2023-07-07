
using System;
using System.Drawing;
using MathNet.Symbolics;
using System.Diagnostics;
namespace ConsoleGraphGenerator
{



    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter a function in terms of 'x':");
            var input = Console.ReadLine();

            Console.WriteLine("Enter the start value for 'x':");
            var fromInput = Console.ReadLine();
            double fromValue;
            if (!double.TryParse(fromInput, out fromValue))
            {
                Console.WriteLine("Invalid input for start value.");
                return;
            }

            Console.WriteLine("Enter the end value for 'x':");
            var toInput = Console.ReadLine();
            double toValue;
            if (!double.TryParse(toInput, out toValue))
            {
                Console.WriteLine("Invalid input for end value.");
                return;
            }

            SymbolicExpression expression;
            try
            {
                expression = SymbolicExpression.Parse(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid function.");
                return;
            }

            Func<double, double> function = expression.Compile("x");

            const int width = 800;
            const int height = 600;
            using (var bitmap = new Bitmap(width, height))
            {
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White);

                    // Draw the X and Y axes
                    g.DrawLine(Pens.Black, width / 2, 0, width / 2, height);
                    g.DrawLine(Pens.Black, 0, height / 2, width, height / 2);

                    for (int x = 0; x < width; x++)
                    {
                        double xValue = fromValue + (toValue - fromValue) * ((double)x / width);
                        double yValue = function(xValue);
                        int y = (int)(((yValue + 10.0) / 20.0) * height); // Scale y for better viewing

                        if (y >= 0 && y < height)
                        {
                            bitmap.SetPixel(x, height - y - 1, Color.Black);
                        }
                    }
                }

                bitmap.Save("function.png");
            }

            Console.WriteLine("Function plotted to function.png.");

            // Open the image in the default image viewer
            var psi = new ProcessStartInfo
            {
                FileName = "function.png",
                UseShellExecute = true
            };
            Process.Start(psi);

        }
    }



}