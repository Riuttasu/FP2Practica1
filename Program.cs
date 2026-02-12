namespace FP2Practica1
{
    internal class Program
    {
        // coordenadas (x,y) para representar posiciones y direcciones de desplazamiento
        struct Coor
        {
            public int x, y;
        }
        struct Estado
        { // estado del juego
            public char[,] mat; // ’#’ muro; ’.’ libre; letras ’a’,’b’ ... bloques
            public char obj; // char correspondiente al bloque objetivo (el que hay que sacar)
            public Coor act, sal; // posiciones del cursor y de la salida
            public bool sel; // idica si hay bloque seleccionado para mover o no
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
        static Estado LeeNivel(string file, int n)
        {

        }
        static void Render(Estado est)
        {

        }
        static void MarcaSalida(Estado est)
        {

        }
        static void MueveCursor(Estado est, Coor dir)
        {

        }
        static void MueveBloque(Estado est, Coor dir)
        {

        }
        static void ProcesaInput(Estado est, char c)
        {

        }
        static char LeeInput()
        {

        }
    }
}
