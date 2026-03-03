namespace FP2Practica1
{
    internal class Program
    {
        // Máximo de movimientos registrados en memoria para poder deshacerlos
        const int maxMem = 10;
        // Sistema de coordenadas vertical-horizontal en la matriz bidimensional
        struct Coor
        {
            public int ver, hor;
        }
        // Cuando se hace un movimiento de bloque, donde ha empezado y terminado el cursor
        struct Jugada
        {
            public Coor aca; //Posición final de la jugada
            public Coor emp; //Posición inicial de la jugada
        }
        // Memoria de jugadas hechas por el jugador para poder deshacerlas si se requiere
        struct Memoria
        {
            public Jugada[] Jugadas; //Serie de jugadas
            public int ind; //Indice de la jugada actual
        }
        // Estado del juego
        struct Estado
        { 
            public char[,] mat; // '#' muro; '.' libre; letras 'a','b' ... bloques
            public char obj; // char correspondiente al bloque objetivo (el que hay que sacar)
            public Coor act, sal; // posiciones del cursor y de la salida
            public bool sel; // idica si hay bloque seleccionado para mover o no
        }
        static void Main()
        {
            Memoria memoria; memoria.Jugadas = new Jugada[maxMem]; // Inicialización de la memoria de jugadas
            int level;
            Estado est; // Estado de juego
            bool hayJuego, victoria;
            while (true)
            {
                memoria.ind = 0;
                // Lectura de nivel
                Console.Write("¿Qué nivel desea jugar?: ");
                level = int.Parse(Console.ReadLine());
                Console.CursorVisible = false; Console.Clear();
                // Se inicializa el estado al nivel deseado
                est = LeeEst("levels.txt", level);
                // Se marca la casilla de salida del nivel (condición de victoria)
                MarcaSalida(ref est);
                hayJuego = true;
                victoria = false;
                // Render inicial
                Render(est);
                // Bucle principal de juego
                while (hayJuego && !victoria)
                {
                    // Lectura de input
                    char c = LeeInput();
                    // Procesamiento de input
                    if (c == 'q') hayJuego = false;
                    else if (c == 'z') Delete(ref memoria, ref est);
                    else ProcesaInput(ref est, c, ref memoria);
                    // Render del estado de juego
                    Render(est);
                    // Comprobación de condición de victoria
                    if (est.mat[est.sal.ver, est.sal.hor] == est.obj) victoria = true;
                }
                // Si se ha logrado conseguir la condición de victoria
                if (victoria)
                {
                    Console.WriteLine("GANASTE");
                    Record(memoria, level); // 
                }
                // Si se ha salido del juego mediante input
                else
                {
                    Console.Clear();
                    Console.WriteLine("Juego Abortado");
                }
            }
        }
        // Deshacer jugada
        static void Delete(ref Memoria mem, ref Estado est)
        {
            // Solo deshace si hay jugada registrada
            if (mem.ind > 0)
            {
                Coor dir;
                mem.ind--;  // Retrocede en el índice de jugadas registradas
                // dir = coordenada de empezar - coordenada de acabar
                dir.hor = mem.Jugadas[mem.ind % maxMem].emp.hor - mem.Jugadas[mem.ind % maxMem].aca.hor;
                dir.ver = mem.Jugadas[mem.ind % maxMem].emp.ver - mem.Jugadas[mem.ind % maxMem].aca.ver;
                // Solo mueve si no ha comenzado y acabado la última jugada en la misma coordenada
                if (!(dir.hor == 0 && dir.ver == 0))
                {
                    // Establece la posición del cursor donde acabó el cursor en el último movimiento de bloque
                    est.act.hor = mem.Jugadas[mem.ind % maxMem].aca.hor; 
                    est.act.ver = mem.Jugadas[mem.ind % maxMem].aca.ver;
                    // Movimiento del bloque sin registro
                    est.sel = true;
                    MueveBloque(ref est, dir, ref mem, false);
                    est.sel = false;
                }
                mem.Jugadas[mem.ind % maxMem].aca.hor = mem.Jugadas[mem.ind % maxMem].emp.hor; 
                mem.Jugadas[mem.ind % maxMem].aca.ver = mem.Jugadas[mem.ind % maxMem].emp.ver;
                
            }

        }
        // Lee un nivel n de un archivo dado
        static Estado LeeEst(string file, int n)
        {
            Estado nivel; nivel.sel = false; nivel.sal.hor = 0; nivel.sal.ver = 0; //Inicializo las variables que se van a rellenan más tarde
            StreamReader File = new StreamReader(file);
            string l = "level " + n; //Busco una fila que tenga Level n en el contenido
            while (File.ReadLine() != l) ; //Muevo el cursor del archivo hasta llegar al level correspondiente 
            nivel.obj = char.Parse(File.ReadLine()); //Registro la primera linea que corresponde al bloque objetivo
            bool espacio = false; //Bandera hasta encontrar lineas vacías
            string tablero = ""; string linea; //tablero indica todo el tablero en una línea y linea será cada linea del archivo
            bool numColumna = false; //Bandera para leer solo la primera fila para le número de columnas ya que todas son iguales
            int numCol = 0; //Número de columnas
            int numFilas = 0; //Número de filas
            while (!espacio)
            {
                //Voy registrando linea por linea
                linea = File.ReadLine() + ' ';
                if (!numColumna) { numColumna = true; numCol = linea.Length - 1; } //Calcula cuantas columnas hay solo una vez
                if (linea == " ") espacio = true; //Final de tablero encontrado
                else { tablero += linea; numFilas++; } //Registro la fila y las cuento
            }
            nivel.mat = new char[numCol + 2, numFilas + 2]; //Declara array con bordes
            string[] lineas = tablero.Split(' '); //Separa las lineas del tablero en filas de un array
            for (int i = 0; i < nivel.mat.GetLength(1); i++) nivel.mat[0, i] = '#'; //Primera fila de borde
            for (int fila = 1; fila < nivel.mat.GetLength(0) - 1; fila++)
            {
                nivel.mat[fila, 0] = '#'; //Primera columna de borde
                for (int colu = 1; colu < nivel.mat.GetLength(1) - 1; colu++)
                {
                    nivel.mat[fila, colu] = lineas[fila - 1][colu - 1]; //Registra la letra correspondiente aprovechando que un string es un array de chars.
                }
                nivel.mat[fila, nivel.mat.GetLength(1) - 1] = '#'; //Última columna de borde
            }
            for (int u = 0; u < nivel.mat.GetLength(1); u++) nivel.mat[nivel.mat.GetLength(0) - 1, u] = '#'; //Última fila de borde
            nivel.act.hor = 1; nivel.act.ver = 1;
            File.Close();
            return nivel;
        }
        
        // Establece la salida (condición de victoria) del nivel
        static void MarcaSalida(ref Estado est)
        {
            int fila, colu;
            int i = 0;
            int j = 0;
            Coor bloqObj; bloqObj.hor = 0; bloqObj.ver = 0;
            bool encontrado = false;
            while (i < est.mat.GetLength(1) && !encontrado)
            {
                j = 0;
                while (j < est.mat.GetLength(0) && !encontrado)
                {
                    if (est.mat[i, j] == est.obj) { encontrado = true; bloqObj.ver = i; bloqObj.hor = j; }
                    j++;
                }
                i++;
            }
            if (est.mat[bloqObj.ver + 1, bloqObj.hor] == est.obj) { colu = bloqObj.hor; fila = est.mat.GetLength(0) - 1; }
            else { colu = est.mat.GetLength(0) - 1; fila = bloqObj.ver; }
            est.sal.hor = colu; est.sal.ver = fila;
            est.mat[fila, colu] = '.';
        }
        // ---- Render ----
        #region Métodos del render
        // Render del estado de juego interno para la consola
        static void Render(Estado est)
        {
            // Array de colores disponibles para el render
            ConsoleColor[] colores = (ConsoleColor[])
            ConsoleColor.GetValues(typeof(ConsoleColor));
            // Inicia en la esquina superior izquierda
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < est.mat.GetLength(0); i++)
            {
                for (int j = 0; j < est.mat.GetLength(1); j++)
                {
                    // Selección de color
                    if (est.mat[i, j] == '#') Console.BackgroundColor = colores[colores.GetLength(0) - 1];
                    else if (est.mat[i, j] == '.') Console.BackgroundColor = colores[0];
                    else Console.BackgroundColor = colores[BloqueToInt(est.mat[i, j])];
                    // Escritura del bloque
                    Console.Write("  ");
                }
                Console.WriteLine();
            }
            // Selección de color del cursor
            if (est.mat[est.act.ver, est.act.hor] == '.') Console.BackgroundColor = colores[0];
            else Console.BackgroundColor = colores[BloqueToInt(est.mat[est.act.ver, est.act.hor])];
            Console.SetCursorPosition(est.act.hor * 2, est.act.ver);
            // Diferenciación del cursor si está seleccionado o no
            if (est.sel) Console.WriteLine("<>");
            else Console.WriteLine("**");
            // Render de la salida, solo si no es parte del bloque que se busca
            if (est.mat[est.sal.ver, est.sal.hor] != est.obj)
            {
                Console.SetCursorPosition(est.sal.hor * 2, est.sal.ver);
                Console.BackgroundColor = colores[0]; // La salida siempre es vacía si no ha terminado el juego
                Console.Write("  ");
            }
            Console.SetCursorPosition(0, est.mat.GetLength(0) + 3);
            Console.ResetColor();
        }
        // Convierte un char en un int para array de colores
        static int BloqueToInt(char c)
        {
            return ((int)c) - ((int)'a') + 1;
        }
        #endregion
        // ---- Movimiento ----
        #region Métodos de movimiento
        // Mueve el cursor del jugador en una dirección dir
        static void MueveCursor(ref Estado est, Coor dir)
        {
            if (est.act.hor + dir.hor >= 1 && est.act.hor + dir.hor < est.mat.GetLength(0) - 1) // Comprueba que no se sale de los bordes de juego
                est.act.hor += dir.hor; // Movimiento horizontal
            if (est.act.ver + dir.ver >= 1 && est.act.ver + dir.ver < est.mat.GetLength(1) - 1) // Comprueba que no se sale de los bordes de juego
                est.act.ver += dir.ver; // Movimiento vertical
        }
        // Mueve un bloque seleccionado en una dirección dada si es posible
        // A su vez mueve el cursor
        static void MueveBloque(ref Estado est, Coor dir, ref Memoria mem, bool registro)
        {
            if (est.sel) // Solo mueve si hay bloque seleccionado
            {
                Coor negdir = dir; negdir.hor *= -1; negdir.ver *= -1; // Dirección contraria
                Coor cabeza = BuscaCabeza(dir, est); // Busca la última parte del bloque en la dirección buscada
                Coor cola = BuscaCabeza(negdir, est); // Cola es la última posición en la dirección contraria del bloque entero (cabeza contraria)
                char c = est.mat[cabeza.ver, cabeza.hor]; // Carácter de la cabeza
                if ((cola.hor != cabeza.hor || cola.ver != cabeza.ver) && est.mat[cabeza.ver + dir.ver, cabeza.hor + dir.hor] == '.') // Solo lo mueve si el espacio de alante está libre, y la cola no es la misma que la cabeza
                {
                    est.mat[cola.ver, cola.hor] = '.'; // Reemplaza la cola por un espacio en blanco
                    est.mat[cabeza.ver + dir.ver, cabeza.hor + dir.hor] = c; // Reemplaza el espacio frente a la cabeza en dir por el carácter del bloque
                    // Registra la posición del cursor previa al movimiento
                    if (registro)
                    {
                        mem.Jugadas[mem.ind % maxMem].emp.hor = est.act.hor; 
                        mem.Jugadas[mem.ind % maxMem].emp.ver = est.act.ver;
                    }
                    MueveCursor(ref est, dir); // Mueve el cursor junto al bloque
                    // Registra la posición del cursor posterior al movimiento y aumenta el indice de jugadas en memoria
                    if (registro)
                    {
                        mem.Jugadas[mem.ind % maxMem].aca.hor = est.act.hor; mem.Jugadas[mem.ind % maxMem].aca.ver = est.act.ver;
                        mem.ind++;
                    }
                }
            }
        }
        // Búsqueda de la casilla más alejada en una dirección de un bloque dado
        static Coor BuscaCabeza(Coor dir, Estado est)
        {
            Coor pos = est.act; // Posición a comprobar, empieza en el cursor 
            Coor cabeza = pos; // Posición inicial de la cabeza -> justo en el cursor
            char c = est.mat[pos.ver, pos.hor]; // Caracter del bloque
            while (est.mat[pos.ver + dir.ver, pos.hor + dir.hor] == c)
            {
                pos.hor += dir.hor; pos.ver += dir.ver; // Se avanza en la dirección
                cabeza = pos; // Si coinciden, se reasigna la cabeza
            }
            return cabeza;
        }
        // Acción de movimiento en una dirección
        static void Mueve(int hor, int ver, ref Estado est, ref Memoria mem)
        {
            // Inicialización de la dirección
            Coor dir;
            dir.hor = hor; dir.ver = ver;
            // Mueve bloque o cursor dependiendo de si hay un bloque seleccionado
            if (est.sel) MueveBloque(ref est, dir, ref mem, true);
            else MueveCursor(ref est, dir);
        }
        #endregion
        // ---- Input ----
        #region Métodos de input
        // Procesamiento del input del jugador
        static void ProcesaInput(ref Estado est, char c, ref Memoria mem)
        {
            Coor pos = est.act; // Posición del cursor actual (más fácil acceso)
            switch (c)
            {
                // Selección de bloques
                case 's':
                    if (est.mat[pos.ver, pos.hor] != '#' && est.mat[pos.ver, pos.hor] != '.') // Comprueba si está el cursor sobre un bloque
                        est.sel = !est.sel; // Invierte sel
                    break;
                // Movimiento
                case 'u':
                    Mueve(0, -1, ref est, ref mem);
                    break;
                case 'd':
                    Mueve(0, 1, ref est, ref mem);
                    break;
                case 'l':
                    Mueve(-1, 0, ref est, ref mem);
                    break;
                case 'r':
                    Mueve(1, 0, ref est, ref mem);
                    break;
                // Deshacer jugada
                case 'z': break;
                // Salida
                case 'q': break;
                default: break;
            }
        }
        // Lectura de input del jugador
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
        #endregion
        // ---- Archivo de records ----
        #region Métodos función records
        // Añade el record al archivo de records, si ya existe un record previo, lo compara
        static void Record(Memoria mem, int level)
        {
            StreamReader sr = new StreamReader("record.txt"); // Lectura del archivo de records
            bool encontrado = false; // Nivel buscado encontrado
            // 
            while(!encontrado && !sr.EndOfStream) // Busca el nivel por el archivo
            {
                if (sr.ReadLine() == "level " + level) { encontrado = true; }
            }
            // Se ha encontrado el nivel, comprobar si se ha batido el record
            if (encontrado)
            {
                int OldMoves = int.Parse(sr.ReadLine()); // Lee el anterior record para ese nivel
                sr.Close(); // Cierra la lectura
                if (mem.ind < OldMoves) // Si los movimientos actuales son menores que el record -> cambiar
                {
                    EstableceRecord(level,mem.ind);
                }
            }
            // No se ha encontrado el nivel en el registro de records, añadirlo al archivo
            else
            {
                sr.Close();
                // append añade nuevas lineas al documento en vez de sobreescribir
                StreamWriter sw = new StreamWriter("record.txt", true);
                // Se escribe el nuevo nivel con el número de movimientos requeridos para pasarlo
                sw.WriteLine("level " + level);
                sw.WriteLine(mem.ind);
                sw.Close();
            }
        }
        // Reescribe el archivo de records con un nuevo record para un nivel
        static void EstableceRecord(int level, int NewMoves)
        {
            // Establecimiento de los flujos,
            // streamwriter escribe en un archivo nuevo, streamreader lee los records antiguos
            StreamReader sr = new StreamReader("record.txt");
            StreamWriter sw = new StreamWriter("newrecord.txt");
            bool encontrado = false; // Condición de bandera de nivel encontrado
            while (!encontrado && !sr.EndOfStream) // Programación defensiva endofstream
            {
                // Copia linea por línea los records antiguos
                string s = sr.ReadLine();
                if (s == $"level {level}") // Si el nivel coincide con el buscado, establece bandera
                {
                    encontrado = true;
                }
                sw.WriteLine(s);
            }
            // Escribe el nuevo record para el nivel y se salta la línea en el StreamReader
            sw.WriteLine(NewMoves) ;
            sr.ReadLine();
            // Continua copiando línea por línea hasta acabar el archivo
            while (!sr.EndOfStream)
            {
                string s = sr.ReadLine();
                sw.WriteLine(s);
            }
            // Cierre de ambos Flujos
            sr.Close();
            sw.Close();
            // Reemplaza el archivo de records viejo por el archivo de records nuevo
            File.Replace("newrecord.txt", "record.txt", null);
            // Felicidades
            Console.WriteLine($"Nuevo record para el nivel {level}!!!!!! felicidades ");
        }
        #endregion
    }
}

