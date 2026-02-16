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
            public char[,] mat; // '#' muro; '.' libre; letras 'a','b' ... bloques
            public char obj; // char correspondiente al bloque objetivo (el que hay que sacar)
            public Coor act, sal; // posiciones del cursor y de la salida
            public bool sel; // idica si hay bloque seleccionado para mover o no
        }
        static void Main(string[] args)
        {
            Estado Nivel1 = LeeNivel("levels.txt", 1);
            Render(Nivel1);
        }
        static Estado LeeNivel(string file, int n)
        {
            Estado nivel; nivel.sel = false; nivel.sal.x = 0; nivel.sal.y = 0; //Inicializo las variables que se van a rellenan más tarde
            StreamReader File = new StreamReader(file);
            string l = "level " + n; //Busco una fila que tenga Level n en el contenido
            while (File.ReadLine() != l) ; //Muevo el cursor del archivo hasta llegar al level correspondiente 
            nivel.obj = char.Parse(File.ReadLine()); //Registro la primera linea que corresponde al bloque objetivo
            bool espacio = false; //Bandera hasta encontrar lineas vacías
            string tablero = ""; string linea; //tablero indica todo el tablero en una línea y linea será cada linea del archivo
            bool numColumna = false; //Bandera para leer solo la primera fila para le número de columnas ya que todas son iguales
            int numCol = 0; //el número de columnas
            int s = 0; //Número de filas
            while (!espacio)
            {
                linea = File.ReadLine() + ' ';
                if (!numColumna) { numColumna = true; numCol = linea.Length; }
                if (linea == " ") espacio = true;
                else { tablero += linea; s++; }
            }
            nivel.mat = new char[numCol + 1, s + 2];
            string[] lineas = tablero.Split(' ');
            for (int j = 0; j < nivel.mat.GetLength(1); j++) nivel.mat[0, j] = '#';
            for (int i = 1; i < nivel.mat.GetLength(0) - 1; i++)
            {
                nivel.mat[i, 0] = '#';
                for (int k = 1; k < nivel.mat.GetLength(1) - 1; k++)
                {
                    nivel.mat[i, k] = lineas[i - 1][k - 1];
                }
                nivel.mat[i, nivel.mat.GetLength(1) - 1] = '#';
            }
            for (int u = 0; u < nivel.mat.GetLength(1); u++) nivel.mat[nivel.mat.GetLength(0) - 1, u] = '#';
            nivel.act.x = 1; nivel.act.y = 1;
            return nivel;
        }
        static void Render(Estado est)
        {
            ConsoleColor[] colores = (ConsoleColor[])
            ConsoleColor.GetValues(typeof(ConsoleColor));
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < est.mat.GetLength(0); i++)
            {
                for (int j = 0; j < est.mat.GetLength(1); j++)
                {
                    if (est.mat[i, j] == '#') Console.BackgroundColor = colores[colores.GetLength(0) - 1];
                    else if (est.mat[i, j] == '.') Console.BackgroundColor = colores[0];
                    else Console.BackgroundColor = colores[BloqueToInt(est.mat[i, j])];
                    Console.Write("  ");
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }

        static int BloqueToInt(char c)
        {
            return ((int)c) - ((int)'a') + 1;
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
        static char CompruebaCasilla(Estado est, Coor dir, Coor pos)
        {
            char c = ' ';

            return c;
        }
        static Coor BuscaCabeza(Coor dir, Estado est)
        {
            Coor pos = est.act; // Posición a comprobar, empieza en el cursor 
            Coor cabeza = pos; // Posición inicial de la cabeza -> justo en el cursor
            char c = est.mat[pos.x, pos.y]; // Caracter del bloque
            bool fin = false; // bandera
            while (!fin)
            {
                pos.x += dir.x; pos.y += dir.y; // Se avanza en la dirección
                if (est.mat[pos.x, pos.y] != c) fin = true; // Si no coinciden los char, acaba
                else cabeza = pos; // Si coinciden, se reasigna la cabeza
            }
            return cabeza;
        }
        static void ProcesaInput(ref Estado est, char c)
        {
            Coor pos = est.act; // Posición del cursor actual (más fácil acceso)
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
            char d = ' ';
            while (d == ' ')
            {
                if (Console.KeyAvailable)
                {
                    string tecla = Console.ReadKey().Key.ToString();
                    switch (tecla)
                    {
                        case "LeftArrow": d = 'l'; break; // direccones
                        case "UpArrow": d = 'u'; break;
                        case "RightArrow": d = 'r'; break;
                        case "DownArrow": d = 'd'; break;
                        case "Delete": d = 'z'; break; // deshacer jugada
                        case "Escape": d = 'q'; break; // salir
                        case "Spacebar": d = 's'; break; // selección de bloque
                    }
                }
            }
            return d;
        }
    }
}

