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
        static void MueveCursor(ref Estado est, Coor dir)
        {
            if (!est.sel) // Solo mueve si no hay bloque seleccionado
            {
                if (est.act.x + dir.x > 1 && est.act.x + dir.x < est.mat.GetLength(0)) // Comprueba que no se sale de los bordes de juego
                    est.act.x += dir.x; // Movimiento horizontal
                if (est.act.y + dir.y > 1 && est.act.y + dir.y < est.mat.GetLength(1)) // Comprueba que no se sale de los bordes de juego
                    est.act.y += dir.y; // Movimiento vertical
            }
        }
        static void MueveBloque(ref Estado est, Coor dir)
        {
            if (est.sel) // Solo mueve si hay bloque seleccionado
            {
                Coor cabeza = BuscaCabeza(dir, est); // Busca la última parte del bloque en la dirección buscada
            }
        }
        static Coor BuscaCabeza(Coor dir, Estado est)
        {
            Coor pos = est.act; // Posición a comprobar, empieza en el cursor 
            Coor cabeza = pos; // Posición inicial de la cabeza -> justo en el cursor
            char c = est.mat[pos.x, pos.y]; // Caracter del bloque
            bool fin = false;
            while (!fin)
            {
                if (est.mat[pos.x, pos.y] != c) fin = true;
                else cabeza = pos;
            }

        }
        static void ProcesaInput(ref Estado est, char c)
        {
            Coor pos = est.act; // Posición del cursor actual (más fácil acceso)
            Coor dir; // Dirección de movimiento
            switch (c)
            {
                case 's':
                    if (est.mat[pos.x, pos.y] != '#' && est.mat[pos.x, pos.y] != '.') // Comprueba si está el cursor sobre un bloque
                        est.sel = !est.sel; // Invierte sel
                    break;
                case 'u':
                    Mueve(0, -1, ref est);
                    break;
                case 'd':
                    Mueve(0, 1, ref est);
                    break;
                case 'l':
                    Mueve(-1, 0, ref est);
                    break;
                case 'r':
                    Mueve(1, 0, ref est);
                    break;
                case 'z': break;
                case 'q': break;
                default: break;
            }
        }
        static void Mueve(int x, int y, ref Estado est)
        {
            Coor dir;
            dir.x = x; dir.y = y;
            if (est.sel) MueveBloque(ref est, dir);
            else MueveCursor(ref est, dir);
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
