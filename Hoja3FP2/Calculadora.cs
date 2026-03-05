namespace Hoja3FP2
{
    class Calculadora
    {
        static void Main()
        {
            Complex a = new Complex(3, 4);
            Complex b = new Complex(2, -5);
            Complex c = a + b;
            WriteComplex(c);
            c = a - b;
            WriteComplex(c);
            c = a*b; 
            WriteComplex(c);
            c = a/b; 
            WriteComplex(c);
        }
        static void WriteComplex(Complex a)
        {
            float re = a.GetRe();
            float im = a.GetIm();
            bool hasRe = true;
            bool hasIm = true;
            bool ImNeg = false;
            if (re == 0)
            {
                hasRe = false;
            }
            if (im == 0)
            {
                hasIm = false;
            }
            else if (im < 0)
            {
                ImNeg = true;
            }
            if (hasRe) { Console.Write(re); }
            if (hasIm)
            {
                if (!ImNeg)
                {
                    Console.WriteLine($" + {im}i");
                }
                else
                {
                    Console.WriteLine($" - {MathF.Abs(im)}i");
                }
            }

        }
    }
}
