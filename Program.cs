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
            Estado nivel;
            StreamReader file = new StreamReader(file);
            string l = "level " + n;
            while (file.ReadLine() != l) ;
            nivel.obj = file.Read();
            file.ReadLine();
            bool espacio = false;
            string tablero, linea;
            bool numColumna = false;
            int numCol = 0;
            int i = 0;
            while (!espacio)
            {
                linea = file.ReadLine() + ' ';
                if (!numColumna) { numColumna = true;  numCol = linea.GetLength; }
                if (linea == " ") espacio = true;
                else { tablero += linea; i++; }
            }
            nivel.mat = new char[numCol+2, i+2];
            string[] lineas = tablero.Split (' ');
            for (int i = 0; i < nivel.mat.GetLength[0];i++) nivel.mat[i, 0] = '#';
            for (int i = 0; i < nivel.mat.GetLength[1]-2; i++)
            {
                for (int j = 0; i < nivel.mat.GetLength[0]; j++)
                {
                    nivel.mat[0] = '#';
                    for (int k = 0;i< nivel.mat.GetLength[0]-2;k++)
                    {
                        nivel.mat[k + 1, i] = lineas[k][i];
                    }
                    nivel.mat[nivel.mat.GetLenght[0]] = '#';
                }
            }
            for (int i = 0; i < mat.GetLength[0];i++) nivel.mat[i, mat.GetLenght[1]] = '#';
            nivel.act.x = 1; nivel.act.y = 1;
            return nivel;
        }
        static void Render(Estado est)
        {
            ConsoleColor[] colores = (ConsoleColor[])
            ConsoleColor.GetValues(typeof(ConsoleColor));

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
        static int BloqueToInt(char c)
        {
            return ((int)c) - ((int) 'a') +1;
        }

    }
}
