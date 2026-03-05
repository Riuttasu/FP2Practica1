using System.ComponentModel;

namespace Hoja3FP2
{
    class Complex
    {
        // Atributos
        private float re;
        private float im;
        // Constructores sin y con argumentos
        public Complex()
        {
            re = 0;
            im = 0;
        }
        public Complex(float re, float im)
        {
            this.re = re;
            this.im = im; 
        }
        // Métodos get
        public float GetRe()
        {
            return re;
        }
        public float GetIm()
        {
            return im;
        }
        // Operaciones
        // Suma
        public static Complex operator +(Complex a, Complex b)
        {
            Complex res = new Complex(); // Creación de un complejo vacío
            res.re = a.re + b.re; // Suma real
            res.im = a.im + b.im; // Suma imaginaria
            return res;
        }
        // Resta
        public static Complex operator -(Complex a, Complex b)
        {
            Complex res = new Complex(); // Creación de un complejo vacío
            res.re = a.re - b.re; // Resta real
            res.im = a.im - b.im; // Resta imaginaria
            return res;
        }
        // Multiplicación 
        public static Complex operator *(Complex a, Complex b)
        {
            Complex res = new Complex(); // Creación de un complejo vacío
            res.re = a.re * b.re - a.im * b.im; // Parte real
            res.im = a.re * b.im + a.im * b.re; // Parte imaginaria
            return res;
        }
        // División
        public static Complex operator /(Complex a, Complex b)
        {
            Complex res = new Complex(); // Creación de un complejo vacío
            if (b.re != 0 && b.im != 0)
            {
                Complex c = new Complex(b.re,-b.im); // Conjugado de b
                res = (a * c); // Multiplicación de a por el conjugado de b
                float den = MathF.Pow(b.re,2) + MathF.Pow(b.im, 2); // Denominador 
                res.re /= den;
                res.im /= den;
            }
            // División por cero -> error
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: division by 0");
                Console.ResetColor();
            }
            return res;
        }
        
    }
}
