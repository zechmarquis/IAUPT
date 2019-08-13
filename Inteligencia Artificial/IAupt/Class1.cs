using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace IAupt
{
    namespace uptRNA
    {
        /// <summary>
        /// Clase rna perceptrón multicapa.
        /// </summary>
        public class PerceptronMultiCapa
        {
            /// <summary>
            /// Número total de capas.
            /// </summary>
            public int C;
            /// <summary>
            /// Número de neuronas por capa.
            /// </summary>
            public List<int> n = new List<int>();
            /// <summary>
            /// Patrones de entrenamiento
            /// </summary>
            public double[,] x;
            /// <summary>
            /// Patrones de salida deseados.
            /// </summary>
            public double[,] s;
            /// <summary>
            /// Patrones de salida obtenidos por la red.
            /// </summary>
            public double[,] y;
            /// <summary>
            /// Número total de patrones.
            /// </summary>
            public int N;
            /// <summary>
            /// Error cuadrático medio.
            /// </summary>
            public double errorCM = 0;
            /// <summary>
            /// Error de entrenamiento.
            /// </summary>
            public double E = double.MaxValue;
            /// <summary>
            /// Error de entrenamiento por iteración.
            /// </summary>
            public double[] GraficarE;
            /// <summary>
            /// Factor de aprendizaje.
            /// </summary>
            public double alfa;
            /// <summary>
            /// Error mínimo a alcanzar.
            /// </summary>
            public double errorMinimo;
            /// <summary>
            /// Número de iteraciones.
            /// </summary>
            public int numIteraciones;
            /// <summary>
            /// Número de iteraciones realizadas al entrenar.
            /// </summary>
            public int iteracionesAlcanzadas = 0;
            /// <summary>
            /// Lista de objetos de la clase Capa.
            /// </summary>
            public List<Capa> capas = new List<Capa>();

            private double[] xmax;
            private double[] xmin;
            private double[] smax;
            private double[] smin;

            String rutaArchivo;
            /// <summary>
            /// Inicializa la red neuronal para aprendizaje o para reconocimiento
            /// </summary>
            /// <param name="rutaArchivo">Especifica la ruta del archivo. Extensión pml -> Enrenamiento. Extensión ppm -> Reconocimiento.</param>
            public PerceptronMultiCapa(String rutaArchivo)
            {
                this.rutaArchivo = rutaArchivo;

                String[] extension = rutaArchivo.Split('.');

                if (extension[1] == "pml")
                {
                    leerArchivoPMC(rutaArchivo);
                }
                else if (extension[1] == "ppm")
                {
                    leerArchivoPPM(rutaArchivo);
                }
                else
                {

                }
            }
            /// <summary>
            /// Lee un archivo *.pml para entrenar la red neuronal.
            /// </summary>
            /// <param name="rutaArchivo">Especifica la ruta y nombre del archivo para entrenar.</param>
            public void leerArchivoPMC(String rutaArchivo)
            {
                FileStream archivo;
                StreamReader leer;
                String arquitectura;

                archivo = new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read);
                leer = new StreamReader(archivo);

                arquitectura = leer.ReadLine();
                String[] elementos = arquitectura.Split(' ');
                C = int.Parse(elementos[0]);
                alfa = double.Parse(leer.ReadLine());
                numIteraciones = int.Parse(leer.ReadLine());
                errorMinimo = double.Parse(leer.ReadLine());
                N = int.Parse(leer.ReadLine());

                for (int i = 1; i <= C; i++)
                {
                    n.Add(int.Parse(elementos[i]));
                }

                x = new double[N, n[0]];
                s = new double[N, n[C - 1]];

                leer.ReadLine();

                for (int p = 0; p < N; p++)
                {
                    String linea = leer.ReadLine();
                    String[] patrones = linea.Split('\t');

                    for (int i = 0; i < int.Parse(arquitectura.Split(' ')[1]); i++)
                    {
                        x[p, i] = double.Parse(patrones[i]);
                    }
                }

                leer.ReadLine();

                for (int p = 0; p < N; p++)
                {
                    String linea = leer.ReadLine();
                    String[] patrones = linea.Split('\t');

                    for (int i = 0; i < int.Parse(arquitectura.Split(' ')[C]); i++)
                        s[p, i] = double.Parse(patrones[i]);
                }

                leer.Close();
                archivo.Close();

                crearRNAupt(N, x, s, alfa, errorMinimo, numIteraciones, arquitectura);
            }
            /// <summary>
            /// Lee un archivo *.pml para reconocer un nuevo patron.
            /// </summary>
            /// <param name="rutaArchivo">Especifica la ruta del archivo para cargar pesos y umbrales.</param>
            public void leerArchivoPPM(String rutaArchivo)
            {
                FileStream archivos;
                StreamReader leer;

                int c, j;
                String arquitectura;

                archivos = new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read);
                leer = new StreamReader(archivos);

                arquitectura = leer.ReadLine();
                String[] elementos = arquitectura.Split(' ');
                C = int.Parse(elementos[0]);

                for (int i = 1; i <= C; i++)
                {
                    n.Add(int.Parse(elementos[i]));
                }

                //Variables para almacenar los valores máximos y mínimos de entrada y salida
                xmax = new double[n[0]];
                xmin = new double[n[0]];
                smax = new double[n[C - 1]];
                smin = new double[n[C - 1]];

                //Se lee la segunda línea que corresponde a los valores máximos y mínimos
                //de los patrones de entrada
                String texto = leer.ReadLine();
                String[] datos = texto.Split(' ');
                j = 0;
                for (int i = 0; i < n[0]; i++)
                {
                    xmax[i] = Convert.ToDouble(datos[j]);
                    xmin[i] = Convert.ToDouble(datos[j + 1]);
                    j += 2;
                }

                //Se lee la tercera línea que corresponde a los valores máximos y mínimos
                //de los patrones de salida
                texto = leer.ReadLine();
                datos = texto.Split(' ');
                j = 0;
                for (int i = 0; i < n[C - 1]; i++)
                {
                    smax[i] = Convert.ToDouble(datos[j]);
                    smin[i] = Convert.ToDouble(datos[j + 1]);
                    j += 2;
                }

                texto = leer.ReadLine();

                crearRNAupt(arquitectura);

                //A partir de esta línea se leen los pesos
                for (c = 0; c < C - 1; c++)
                    for (j = 0; j < n[c]; j++)
                        for (int i = 0; i < n[c + 1]; i++)
                            capas[c].neuronas[j].w[i] = Convert.ToDouble(leer.ReadLine());

                texto = leer.ReadLine();

                //Apartir de esta línea se leen los umbrales
                for (c = 1; c <= C - 1; c++)
                    for (int i = 0; i < n[c]; i++)
                        capas[c].neuronas[i].u = Convert.ToDouble(leer.ReadLine());
                leer.Close();
            }
            /// <summary>
            /// Agrega objetos de la clase Capas.
            /// </summary>
            public void agregarCapas()
            {
                for (int c = 0; c < C; c++)
                {
                    int tipoCapa;
                    if (c == 0)
                        tipoCapa = 0;
                    else if (c == C - 1)
                        tipoCapa = 2;
                    else
                        tipoCapa = 1;

                    if (c < C - 1)
                    {
                        Capa capa = new Capa(tipoCapa, c, n[c], n[c + 1]);
                        capas.Add(capa);
                    }
                    else
                    {
                        Capa capa = new Capa(tipoCapa, c, n[c], 0);
                        capas.Add(capa);
                    }
                }
            }
            /// <summary>
            /// Crea todo el especio de estados entre capas.
            /// </summary>
            public void crearEspacioEstados()
            {
                for (int c = 0; c < C - 1; c++)
                {
                    for (int i = 0; i < n[c + 1]; i++)
                    {
                        for (int j = 0; j < n[c]; j++)
                        {
                            capas[c].neuronas[j].neuronaSiguiente.Add(capas[c + 1].neuronas[i]);
                            capas[c + 1].neuronas[i].neuronaAnterior.Add(capas[c].neuronas[j]);
                        }
                    }
                }
            }
            /// <summary>
            /// Desnormaliza los patrones de salida.
            /// </summary>
            public void desnormalizarSalidas()
            {
                for (int p = 0; p < N; p++)
                {
                    propagar(p);
                    for (int i = 0; i < n[C - 1]; i++)
                    {
                        //y[p, i] = Math.Round(capas[C - 1].neuronas[i].a * (smax[i] - smin[i]) + smin[i], 0);
                        y[p, i] = capas[C - 1].neuronas[i].a * (smax[i] - smin[i]) + smin[i];
                    }
                }
            }
            /// <summary>
            /// Entrena la red neuronal.
            /// </summary>
            public void entrenar()
            {
                int contador = 0;
                GraficarE = new double[numIteraciones];

                while (E >= errorMinimo && contador < numIteraciones)
                {
                    errorCM = 0;
                    for (int p = 0; p < N; p++)
                    {
                        propagar(p);
                        errorCM += calcularErrorCM(p);
                        retroPropagar(p);
                    }
                    E = (1.0 / N) * errorCM;
                    GraficarE[contador] = E;

                    contador++;
                }
                iteracionesAlcanzadas = contador;
                for (int p = 0; p < N; p++)
                {
                    propagar(p);
                    for (int i = 0; i < n[C - 1]; i++)
                    {
                        y[p, i] = capas[C - 1].neuronas[i].a;
                    }
                }
                guardarPesos(rutaArchivo);
                desnormalizarSalidas();
            }
            /// <summary>
            /// Reconoce un patron.
            /// </summary>
            /// <param name="x">Patron a reconocer.</param>
            public void reconocer(double[] x)
            {
                this.x = new double[1, x.Length];
                this.y = new double[1, n[C - 1]];

                //Se normalizan los patrones de entrada y de salida
                for (int i = 0; i < n[0]; i++)
                {
                    this.x[0, i] = (x[i] - xmin[i]) / (xmax[i] - xmin[i]);
                }

                propagar(0);
                for (int i = 0; i < n[C - 1]; i++)
                {
                    y[0, i] = Math.Round(capas[C - 1].neuronas[i].a * (smax[i] - smin[i]) + smin[i], 0);
                }
            }
            /// <summary>
            /// Progpaga un patron específoco hasta la capa de salida.
            /// </summary>
            /// <param name="p">Esecifica el número de patron a propagar.</param>
            public void propagar(int p)
            {
                int c = 0;
                capas[c].calcularActivacion(p, x);

                for (c = 1; c < C; c++)
                {
                    capas[c].calcularActivacion();
                }
            }
            /// <summary>
            /// Calcula el error cuadrático medio.
            /// </summary>
            /// <param name="p">Especifica el número de patron.</param>
            /// <returns></returns>
            public double calcularErrorCM(int p)
            {
                double suma = 0;
                for (int i = 0; i < n[C - 1]; i++)
                {
                    suma += Math.Pow(s[p, i] - capas[C - 1].neuronas[i].a, 2);
                }
                return 0.5 * suma;
            }
            /// <summary>
            /// Calcula los valores delta de cada neurona.
            /// </summary>
            /// <param name="p">Especifica el número de patrón.</param>
            public void retroPropagar(int p)
            {
                int c = C - 1;

                capas[c].calcularDelta(p, s);

                for (c = C - 2; c > 0; c--)
                {
                    capas[c].calcularDelta();
                }

                for (c = 1; c < C; c++)
                {
                    capas[c].actualizar(alfa);
                }
            }
            /// <summary>
            /// Crea la red neuronal.
            /// </summary>
            /// <param name="N">Especifica el número total de patrones.</param>
            /// <param name="x">Especifica los patrones de entrada para entrenar.</param>
            /// <param name="s">Especifica los patrones de salida deseados para entrenar.</param>
            /// <param name="alfa">Especifica el factor de aprendizaje.</param>
            /// <param name="errorMinimo">Especifica el error mínimo a alcanzar.</param>
            /// <param name="numIteraciones">Especifica el número de iteraciones a realizar.</param>
            /// <param name="arquitectura">Especifica la arquitectura de la red.</param>
            public void crearRNAupt(int N, double[,] x, double[,] s, double alfa, double errorMinimo, int numIteraciones, String arquitectura)
            {
                this.N = N;
                this.x = x;
                this.s = s;
                this.alfa = alfa;
                this.errorMinimo = errorMinimo;
                this.numIteraciones = numIteraciones;
                int i, p;

                String[] elemento = arquitectura.Split(' ');
                this.C = int.Parse(elemento[0]);

                for (int c = 1; c < elemento.Length; c++)
                {
                    n.Add(int.Parse(elemento[c]));
                }

                y = new double[N, n[C - 1]];

                //Variables para almacenar los valores máximos y mínimos de entrada y salida
                xmax = new double[n[0]];
                xmin = new double[n[0]];
                smax = new double[n[C - 1]];
                smin = new double[n[C - 1]];

                for (i = 0; i < n[0]; i++)
                {
                    xmax[i] = 0;
                    xmin[i] = 10000;
                }

                for (i = 0; i < n[C - 1]; i++)
                {
                    smax[i] = 0;
                    smin[i] = 10000;
                }

                //Se encuentras los valores máximos y mínimos de x
                for (i = 0; i < n[0]; i++)
                {
                    for (p = 0; p < N; p++)
                    {
                        if (xmax[i] < x[p, i])
                        {
                            xmax[i] = x[p, i];
                        }
                        if (xmin[i] > x[p, i])
                        {
                            xmin[i] = x[p, i];
                        }
                    }
                }

                //Se encuentran los valores máximos y mínimos de las salidas deseadas.
                for (i = 0; i < n[C - 1]; i++)
                {
                    for (p = 0; p < N; p++)
                    {
                        if (smax[i] < s[p, i])
                        {
                            smax[i] = s[p, i];
                        }
                        if (smin[i] > s[p, i])
                        {
                            smin[i] = s[p, i];
                        }
                    }
                }

                //Se normalizan los patrones de entrada y de salida
                for (i = 0; i < n[0]; i++)
                {
                    for (p = 0; p < N; p++)
                    {
                        x[p, i] = (x[p, i] - xmin[i]) / (xmax[i] - xmin[i]);
                    }
                }

                for (i = 0; i < n[C - 1]; i++)
                {
                    for (p = 0; p < N; p++)
                    {
                        s[p, i] = (s[p, i] - smin[i]) / (smax[i] - smin[i]);
                    }
                }

                agregarCapas();

                crearEspacioEstados();
            }
            /// <summary>
            /// Crea la red neuronal para reconocer.
            /// </summary>
            /// <param name="arquitectura">Especifica la arquitectura de la red neurona.</param>
            public void crearRNAupt(String arquitectura)
            {
                String[] elemento = arquitectura.Split(' ');
                this.C = int.Parse(elemento[0]);
                n.Clear();
                for (int c = 1; c < elemento.Length; c++)
                {
                    n.Add(int.Parse(elemento[c]));
                }

                y = new double[N, n[C - 1]];

                agregarCapas();

                crearEspacioEstados();
            }
            /// <summary>
            /// Guarda los pesos entrenados por la red.
            /// </summary>
            /// <param name="rutaArchivo">Especifica la ruta del archivo donde se guardarán los pesos y umbrales.</param>
            private void guardarPesos(String rutaArchivo)
            {
                String[] renombrar;
                renombrar = rutaArchivo.Split('.');

                FileStream pesos = new FileStream(renombrar[0] + ".ppm", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter escribir = new StreamWriter(pesos);
                int i, j, c, p;

                escribir.Write("{0}", C);

                for (i = 1; i <= C; i++)
                {
                    escribir.Write(" {0}", n[i - 1]);
                }

                escribir.WriteLine();

                for (i = 0; i < n[0]; i++)
                {
                    escribir.Write("{0} {1} ", xmax[i], xmin[i]);
                }

                escribir.WriteLine();

                for (i = 0; i < n[C - 1]; i++)
                {
                    escribir.Write("{0} {1} ", smax[i], smin[i]);
                }

                escribir.WriteLine();
                escribir.WriteLine();

                //Almacenar en el archivo los pesos.
                for (c = 0; c < C - 1; c++)
                    for (j = 0; j < n[c]; j++)
                        for (i = 0; i < n[c + 1]; i++)
                            escribir.WriteLine("{0}", capas[c].neuronas[j].w[i]);

                escribir.WriteLine();

                for (c = 1; c < C; c++)
                    for (i = 0; i < n[c]; i++)
                        escribir.WriteLine("{0}", capas[c].neuronas[i].u);

                escribir.WriteLine();

                //Escribir en el archivo la comparación de los valores deseados y obtenidos de la red
                for (i = 0; i < n[C - 1]; i++)
                    escribir.Write("y[{0}]\t\ts[{1}]", i, i);

                escribir.WriteLine();

                escribir.WriteLine();


                for (p = 0; p < N; p++)
                {
                    for (i = 0; i < n[C - 1]; i++)
                    {
                        escribir.Write("{0}\t{1} ", y[p, i], s[p, i]);
                    }
                    escribir.WriteLine();
                }

                escribir.Close();
            }
        }
        /// <summary>
        /// Clase que representa una capa del perceptrón multicapa.
        /// </summary>
        public class Capa
        {
            /// <summary>
            /// Número de capa.
            /// </summary>
            public int numCapa;
            /// <summary>
            /// Número de neurona en la capa.
            /// </summary>
            public int numNeuronas;
            /// <summary>
            /// Número de neuronas de la capa siguiente.
            /// </summary>
            public int numNeuronasCapaSiguiente;
            /// <summary>
            /// Indica el tipo de capa. 0 = entrada, 1 = oculta o 2 = salida.
            /// </summary>
            public int tipoCapa;
            /// <summary>
            /// Lista de objetos de la clase neurona.
            /// </summary>
            public List<Neurona> neuronas = new List<Neurona>();
            /// <summary>
            /// Inicia un objeto de la clase capa.
            /// </summary>
            /// <param name="tipoCapa"></param>
            /// <param name="numCapa"></param>
            /// <param name="numNeuronas"></param>
            /// <param name="numNeuronasCapaSiguiente"></param>
            public Capa(int tipoCapa, int numCapa, int numNeuronas, int numNeuronasCapaSiguiente)
            {
                this.numCapa = numCapa;
                this.numNeuronas = numNeuronas;
                this.tipoCapa = tipoCapa;
                this.numNeuronasCapaSiguiente = numNeuronasCapaSiguiente;
                agregarNeuronas(numNeuronas);
            }
            /// <summary>
            /// Agrega las neuronas que se requieran a la capa.
            /// </summary>
            /// <param name="numNeuronas">Especifica el número de neuronas a agregar a la capa.</param>
            public void agregarNeuronas(int numNeuronas)
            {
                for (int i = 0; i < numNeuronas; i++)
                {
                    Neurona neurona = new Neurona("" + numCapa + "," + i, numNeuronasCapaSiguiente, tipoCapa);
                    neuronas.Add(neurona);
                }
            }
            /// <summary>
            /// Calcula la activación de cada neurona de la capa de entrada.
            /// </summary>
            /// <param name="p">Especifica el número de patrón.</param>
            /// <param name="x">Especifica los patrones de entrada para el entrenamiento.</param>
            public void calcularActivacion(int p, double[,] x)
            {
                for (int i = 0; i < neuronas.Count; i++)
                {
                    neuronas[i].a = x[p, i];
                }
            }
            /// <summary>
            /// Calcula la activación de las neuronas de las capas ocultas y de las de salida
            /// </summary>
            public void calcularActivacion()
            {
                for (int i = 0; i < numNeuronas; i++)
                {
                    double suma = 0;
                    for (int j = 0; j < neuronas[i].neuronaAnterior.Count; j++)
                    {
                        suma += neuronas[i].neuronaAnterior[j].a * neuronas[i].neuronaAnterior[j].w[i];
                    }
                    suma += neuronas[i].u;
                    neuronas[i].a = funcionActivacionSigmoidal(suma);
                }
            }
            /// <summary>
            /// Calcula la función de activación sigmoidal de un valor dado.
            /// </summary>
            /// <param name="x"></param>
            /// <returns></returns>
            public double funcionActivacionSigmoidal(double x)
            {
                x = 1 / (1 + Math.Exp(-x));
                return x;
            }
            /// <summary>
            /// Calcula los valores delta de la capa de salida.
            /// </summary>
            /// <param name="p">Especifica el número de patrón.</param>
            /// <param name="s">Especifica los patrones de salida deseados.</param>
            public void calcularDelta(int p, double[,] s)
            {
                for (int i = 0; i < numNeuronas; i++)
                {
                    neuronas[i].delta = -1.0 * (s[p, i] - neuronas[i].a) * neuronas[i].a * (1 - neuronas[i].a);
                }
            }
            /// <summary>
            /// Calcula los valore delta de la última capa oculta, hasta la primera capa oculta.
            /// </summary>
            public void calcularDelta()
            {
                for (int j = 0; j < numNeuronas; j++)
                {
                    double suma = 0;
                    for (int i = 0; i < neuronas[j].neuronaSiguiente.Count; i++)
                    {
                        suma += neuronas[j].neuronaSiguiente[i].delta * neuronas[j].w[i];
                    }
                    neuronas[j].delta = neuronas[j].a * (1 - neuronas[j].a) * suma;
                }
            }
            /// <summary>
            /// Actualiza los pesos y umbrales de toda la red.
            /// </summary>
            /// <param name="alfa">Especifica el factor de aprendizaje.</param>
            public void actualizar(double alfa)
            {
                for (int i = 0; i < numNeuronas; i++)
                {
                    for (int j = 0; j < neuronas[i].neuronaAnterior.Count; j++)
                    {
                        neuronas[i].neuronaAnterior[j].w[i] = neuronas[i].neuronaAnterior[j].w[i] - (alfa * neuronas[i].delta * neuronas[i].neuronaAnterior[j].a);
                    }
                    neuronas[i].u = neuronas[i].u - alfa * neuronas[i].delta;
                }
            }
        }
        /// <summary>
        /// Clase que reprsenta una neurona del perceptrón multicapa.
        /// </summary>
        public class Neurona
        {
            /// <summary>
            /// Nombre de la neurona.
            /// </summary>
            public String nombre;
            /// <summary>
            /// Activación.
            /// </summary>
            public double a;
            /// <summary>
            /// Umbral.
            /// </summary>
            public double u;
            /// <summary>
            /// Lista de pesos a la siguiente capa.
            /// </summary>
            public List<double> w = new List<double>();
            /// <summary>
            /// Delta.
            /// </summary>
            public double delta;
            /// <summary>
            /// Lista de neuronas de la capa siguiente.
            /// </summary>
            public List<Neurona> neuronaSiguiente = new List<Neurona>();
            /// <summary>
            /// Lista de neuronas de la capa anterior.
            /// </summary>
            public List<Neurona> neuronaAnterior = new List<Neurona>();

            Random rand = new Random();
            /// <summary>
            /// Crea una neurona.
            /// </summary>
            /// <param name="nombre">Especifica el nombre de la neurona.</param>
            /// <param name="numNeuronasCapaSiguiente">Especifica el número de neuronas de la capa siguiente.</param>
            /// <param name="tipoCapa">Especifica el tipo de capa. 0 = Entrada, 1 = Oculta, 2 = Salida.</param>
            public Neurona(String nombre, int numNeuronasCapaSiguiente, int tipoCapa)
            {
                this.nombre = nombre;

                if (tipoCapa == 0)  //Es capa de entrada.
                {
                    for (int i = 0; i < numNeuronasCapaSiguiente; i++)
                        w.Add(rand.NextDouble());
                }
                else if (tipoCapa == 1) //Es capa oculta.
                {
                    for (int i = 0; i < numNeuronasCapaSiguiente; i++)
                        w.Add(rand.NextDouble());
                    u = rand.NextDouble();
                }
                else if (tipoCapa == 2) //Es capa de salida
                {
                    u = rand.NextDouble();
                }
            }
        }
    }

    namespace uptVC
    {
        /// <summary>
        /// Clase Visión por computadora.
        /// </summary>
        public class Vision
        {
            private int m, n;    //Dimensiones de la imagen (m=alto n=ancho)

            private int factorEscala;
            /// <summary>
            /// Especifica el número de objetos encontrados en la imagen.
            /// </summary>
            public int num_objetos;

            byte[] arregloGris;
            int[] arregloBinario;
            //Contador de etiquetas.

            private int x, y, xi, yi, xf, yf;

            /// <summary>
            /// Imagen a procesar.
            /// </summary>
            public Bitmap img;

            /// <summary>
            /// Método constructor si argumentos.
            /// </summary>
            public Vision()
            {
                this.factorEscala = 1;
            }
            /// <summary>
            /// Método contructor para procesar la imagen a diferente escala.
            /// </summary>
            /// <param name="factorEscala">Un valor mayor o igual que 1, indica el número de veces que va a dividir la imagen.</param>
            public Vision(int factorEscala)
            {
                this.factorEscala = factorEscala;
            }

            /*--------------------------------------- Escalar coordenadas al pictureBox --------------------------------------------*/

            /// <summary>
            /// Asigna memoria dinámica a los arreglos y matrices.
            /// </summary>
            private void asignarMemoria()
            {
                arregloGris = new byte[img.Width * img.Height];
                arregloBinario = new int[img.Width * img.Height];
            }

            /// <summary>
            /// Escala la imagen completa para convertir a escala de gris y binarizar.
            /// La escala corresponde al factor de esala que se ingresa en el método constructor.
            /// </summary>
            /// <param name="img">Especifica la imagen a escalar.</param>
            public void escalarCoordenadas(Bitmap img)
            {
                try
                {
                    this.img.Dispose();
                }
                catch (Exception)
                {

                }
                this.img = new Bitmap(img, img.Width / factorEscala, img.Height / factorEscala);

                m = this.img.Height;

                n = this.img.Width;

                this.xi = 0;
                this.yi = 0;

                this.xf = n;
                this.yf = m;

                asignarMemoria();
            }

            /// <summary>
            /// Escala un segmento de la imagen para convertir a escala de gris y binarizar.
            /// </summary>
            /// <param name="img">Especifica la imagen a escalar.</param>
            /// <param name="anchoPictureBox">Especifica el ancho del pictureBox.</param>
            /// <param name="altoPictureBox">Especifica el alto del pictureBox.</param>
            /// <param name="xi">Especifica la coordenada 'x' inicial.</param>
            /// <param name="yi">Especifica la coordenada 'y' inicial.</param>
            /// <param name="xf">Especifica la coordenada 'x' final.</param>
            /// <param name="yf">Especifica la coordenada 'y' final.</param>
            public void escalarCoordenadas(Bitmap img, int anchoPictureBox, int altoPictureBox, int xi, int yi, int xf, int yf)
            {
                this.img = new Bitmap(img, img.Width / factorEscala, img.Height / factorEscala);

                m = this.img.Height;

                m = this.img.Height;
                n = this.img.Width;

                this.xi = (xi * n) / anchoPictureBox;
                this.yi = (yi * m) / altoPictureBox;

                this.xf = (xf * n) / anchoPictureBox;
                this.yf = (yf * m) / altoPictureBox;

                asignarMemoria();
            }

            /// <summary>
            /// Escala la imagen completa para utilizar el segmentado por color RGB.
            /// </summary>
            /// <param name="img">Especifica la imagen a escalar.</param>
            /// <param name="anchoPictureBox">Especifica el ancho del pictureBox.</param>
            /// <param name="altoPictureBox">Especifica el alto del pictureBox.</param>
            /// <param name="coordX">Especifica la coordenada 'x' del pixel de referencia.</param>
            /// <param name="coordY">Especifica la coordenada 'y' del pixel de referencia.</param>
            private void escalarCoordenadas(Bitmap img, int anchoPictureBox, int altoPictureBox, int coordX, int coordY)
            {
                this.img = new Bitmap(img, img.Width / factorEscala, img.Height / factorEscala);

                m = this.img.Height;

                m = this.img.Height;
                n = this.img.Width;

                this.xi = 0;
                this.yi = 0;

                this.xf = n;
                this.yf = m;

                this.x = (coordX * n) / anchoPictureBox;
                this.y = (coordY * m) / altoPictureBox;

                asignarMemoria();
            }

            /// <summary>
            /// Escala un segmento de la imagen para utilizar el segmentado por color RGB.
            /// </summary>
            /// <param name="img">Especifica la imagen a escalar.</param>
            /// <param name="anchoPictureBox">Especifica el ancho del pictureBox.</param>
            /// <param name="altoPictureBox">Especifica el alto del pictureBox.</param>
            /// <param name="xi">Especifica la coordenada 'x' inicial.</param>
            /// <param name="yi">Especifica la coordenada 'y' inicial.</param>
            /// <param name="xf">Especifica la coordenada 'x' final.</param>
            /// <param name="yf">Especifica la coordenada 'y' final.</param>
            /// <param name="coordX">Especifica la coordenada 'x' del pixel de referencia.</param>
            /// <param name="coordY">Especifica la coordenada 'y' del pixel de referencia.</param>
            private void escalarCoordenadas(Bitmap img, int anchoPictureBox, int altoPictureBox, int xi, int yi, int xf, int yf, int coordX, int coordY)
            {
                this.img = new Bitmap(img, img.Width / factorEscala, img.Height / factorEscala);

                m = this.img.Height;

                m = this.img.Height;
                n = this.img.Width;

                this.xi = (xi * n) / anchoPictureBox;
                this.yi = (yi * m) / altoPictureBox;

                this.xf = (xf * n) / anchoPictureBox;
                this.yf = (yf * m) / altoPictureBox;

                this.x = (coordX * n) / anchoPictureBox;
                this.y = (coordY * m) / altoPictureBox;

                asignarMemoria();
            }

            /*--------------------------------------------- Manipulación de píxeles ------------------------------------------------------*/

            /// <summary>
            /// Convierte una imagen RGB a escala de grises.
            /// </summary>
            public void rgbAgris()
            {
                int x = 0, y = 0;
                BitmapData bmpdata = null;

                bmpdata = this.img.LockBits(new Rectangle(0, 0, this.img.Width, this.img.Height), ImageLockMode.ReadOnly, this.img.PixelFormat);
                int numbytes = bmpdata.Stride * this.img.Height;
                byte[] bytedata = new byte[numbytes];
                IntPtr arregloImagen = bmpdata.Scan0;

                Marshal.Copy(arregloImagen, bytedata, 0, numbytes);

                x = xi;
                y = yi;
                int numPixel = 0;
                int cont = 0;
                for (int k = 0; k < numbytes; k += 4)
                {
                    if ((cont % n) >= xi && (cont % n) <= xf && (cont / n) >= yi && (cont / n) <= yf)
                    {
                        arregloGris[numPixel] = (byte)(0.3 * bytedata[k + 2] + 0.59 * bytedata[k + 1] + 0.11 * bytedata[k]);
                        bytedata[k + 2] = bytedata[k + 1] = bytedata[k] = arregloGris[numPixel];
                        numPixel++;
                    }
                    cont++;
                }

                Marshal.Copy(bytedata, 0, arregloImagen, numbytes);
                this.img.UnlockBits(bmpdata);
            }

            /// <summary>
            /// Convierte una imagen RGB a binario.
            /// </summary>
            /// <param name="umbral">Determina el nivel gris para binarizar.</param>
            /// <param name="fondo">true: fondo negro, false: fondo blanco.</param>
            public void rgbAbinario(byte umbral, bool fondo)
            {
                int cont = 0;
                int numPixel = 0;
                BitmapData bmpdata = null;

                bmpdata = this.img.LockBits(new Rectangle(0, 0, this.img.Width, this.img.Height), ImageLockMode.ReadOnly, this.img.PixelFormat);
                int numbytes = bmpdata.Stride * this.img.Height;
                byte[] bytedata = new byte[numbytes];
                IntPtr arregloImagen = bmpdata.Scan0;

                Marshal.Copy(arregloImagen, bytedata, 0, numbytes);


                for (int k = 0; k < numbytes; k += 4)
                {
                    if ((cont % n) >= xi && (cont % n) <= xf && (cont / n) >= yi && (cont / n) <= yf)
                    {
                        if (arregloGris[numPixel] < umbral)
                        {
                            if (fondo == false)
                            {
                                arregloBinario[cont] = 1;
                                bytedata[k + 2] = bytedata[k + 1] = bytedata[k] = 255;
                            }
                            else
                            {
                                arregloBinario[cont] = 0;
                                bytedata[k + 2] = bytedata[k + 1] = bytedata[k] = 0;
                            }
                        }
                        else
                        {
                            if (fondo == false)
                            {
                                arregloBinario[cont] = 0;
                                bytedata[k + 2] = bytedata[k + 1] = bytedata[k] = 0;
                            }
                            else
                            {
                                arregloBinario[cont] = 1;
                                bytedata[k + 2] = bytedata[k + 1] = bytedata[k] = 255;
                            }
                        }
                        numPixel++;
                    }
                    cont++;
                }

                Marshal.Copy(bytedata, 0, arregloImagen, numbytes);
                this.img.UnlockBits(bmpdata);
            }

            /*----------------------------------------------- Procesos de segmentación y proceso digital a imágenes -------------------------------------------------------*/



            /*--------------------------------------------------- Procesos aplicados para reconocimiento de objetos --------------------------------------*/
            /// <summary>
            /// Etiqueta una imagen y almacena las etiquetas en especios de estados.
            /// </summary>
            /// <param name="area">Especifica el tamaño mínimo del área del objeto que se puede considerar para etiquetar.</param>
            /// <returns></returns>
            public List<Hu> etiquetarImagenEspaciosEstados(int area)
            {
                Etiquetado etiquetado = new Etiquetado();

                //FileStream archivo;
                //StreamWriter escribir;

                int Etiqueta, aux = 0;

                Etiqueta = 0;
                etiquetado.insertarEtiqueta(Etiqueta);
                etiquetado.etiquetas[0].esPadre = false;

                for (int p = 0, p1 = arregloBinario.Length - 1; p < n; p++, p1--)
                {
                    arregloBinario[p] = 0;
                    arregloBinario[p1] = 0;
                }

                for (int p = 0; p < arregloBinario.Length; p += n)
                {
                    arregloBinario[p] = 0;

                    if (p - 1 > 0)
                        arregloBinario[p - 1] = 0;
                }

                for (int p = 0; p < arregloBinario.Length; p++)
                {
                    if (arregloBinario[p] == 1)
                    {
                        List<int> equivalencias = new List<int>();
                        aux = int.MaxValue;

                        if (arregloBinario[p - 1] != 0)
                        {
                            if (aux > arregloBinario[p - 1])
                            {
                                aux = arregloBinario[p - 1];
                                arregloBinario[p] = aux;
                            }

                            if (!equivalencias.Contains(arregloBinario[p - 1]))
                            {
                                equivalencias.Add(arregloBinario[p - 1]);
                            }
                        }

                        if (arregloBinario[p - n - 1] != 0)
                        {
                            if (aux > arregloBinario[p - n - 1])
                            {
                                aux = arregloBinario[p - n - 1];
                                arregloBinario[p] = aux;
                            }

                            if (!equivalencias.Contains(arregloBinario[p - n - 1]))
                            {
                                equivalencias.Add(arregloBinario[p - n - 1]);
                            }
                        }

                        if (arregloBinario[p - n] != 0)
                        {
                            if (aux > arregloBinario[p - n])
                            {
                                aux = arregloBinario[p - n];
                                arregloBinario[p] = aux;
                            }

                            if (!equivalencias.Contains(arregloBinario[p - n]))
                            {
                                equivalencias.Add(arregloBinario[p - n]);
                            }
                        }

                        if (arregloBinario[p - n + 1] != 0)
                        {
                            if (aux > arregloBinario[p - n + 1])
                            {
                                aux = arregloBinario[p - n + 1];
                                arregloBinario[p] = aux;
                            }

                            if (!equivalencias.Contains(arregloBinario[p - n + 1]))
                            {
                                equivalencias.Add(arregloBinario[p - n + 1]);
                            }
                        }

                        if (aux == int.MaxValue)
                        {
                            Etiqueta++;
                            arregloBinario[p] = Etiqueta;

                            etiquetado.insertarEtiqueta(Etiqueta);
                            etiquetado.etiquetas[Etiqueta].insertarPixel((short)(p % n), (short)(p / n));
                        }
                        else
                        {
                            etiquetado.etiquetas[aux].insertarPixel((short)(p % n), (short)(p / n));

                            equivalencias.Sort();
                            equivalencias.Reverse();

                            if (equivalencias.Count() > 1)
                            {
                                for (int i = 0; i < equivalencias.Count - 1; i++)
                                {
                                    if (etiquetado.etiquetas[equivalencias[i]].esPadre)
                                    {
                                        if (etiquetado.etiquetas[equivalencias.Last()].esPadre)
                                        {
                                            cambiarHijos(etiquetado.etiquetas[equivalencias.Last()], etiquetado.etiquetas[equivalencias[i]]);
                                        }
                                        else
                                        {
                                            //if (!etiquetado.etiquetas[equivalencias[i]].Hijos.Contains(etiquetado.etiquetas[equivalencias.Last()]))
                                            if (!etiquetado.etiquetas[equivalencias[i]].Equals(etiquetado.etiquetas[equivalencias.Last()].Padre))
                                            {
                                                if (etiquetado.etiquetas[equivalencias[i]].numeroEtiqueta < etiquetado.etiquetas[equivalencias.Last()].Padre.numeroEtiqueta)
                                                    cambiarHijos(etiquetado.etiquetas[equivalencias[i]], etiquetado.etiquetas[equivalencias.Last()].Padre);
                                                else
                                                    cambiarHijos(etiquetado.etiquetas[equivalencias.Last()].Padre, etiquetado.etiquetas[equivalencias[i]]);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (etiquetado.etiquetas[equivalencias.Last()].esPadre)
                                        {
                                            //if (!etiquetado.etiquetas[equivalencias.Last()].Hijos.Contains(etiquetado.etiquetas[equivalencias[i]]))
                                            if (!etiquetado.etiquetas[equivalencias.Last()].Equals(etiquetado.etiquetas[equivalencias[i]].Padre))
                                            {
                                                if (etiquetado.etiquetas[equivalencias[i]].Padre.numeroEtiqueta < etiquetado.etiquetas[equivalencias.Last()].numeroEtiqueta)
                                                    cambiarHijos(etiquetado.etiquetas[equivalencias[i]].Padre, etiquetado.etiquetas[equivalencias.Last()]);
                                                else
                                                    cambiarHijos(etiquetado.etiquetas[equivalencias.Last()], etiquetado.etiquetas[equivalencias[i]].Padre);
                                            }
                                        }
                                        else
                                        {
                                            if (!etiquetado.etiquetas[equivalencias.Last()].Padre.Equals(etiquetado.etiquetas[equivalencias[i]].Padre))
                                            {
                                                if (etiquetado.etiquetas[equivalencias[i]].Padre.numeroEtiqueta < etiquetado.etiquetas[equivalencias.Last()].Padre.numeroEtiqueta)
                                                    cambiarHijos(etiquetado.etiquetas[equivalencias[i]].Padre, etiquetado.etiquetas[equivalencias.Last()].Padre);
                                                else
                                                    cambiarHijos(etiquetado.etiquetas[equivalencias.Last()].Padre, etiquetado.etiquetas[equivalencias[i]].Padre);
                                            }
                                            else
                                            {

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //archivo = new FileStream(".\\etiquetado1.txt", FileMode.Create, FileAccess.Write);
                //escribir = new StreamWriter(archivo);
                //String datos;

                //for (int p = 0; p < arregloBinario.Length; p++)
                //{
                //    if (p >= n)
                //    {
                //        if ((p % n) == 0)
                //            escribir.WriteLine();
                //    }

                //    datos = arregloBinario[p] + " ";
                //    escribir.Write(datos);
                //}
                //escribir.Close();

                List<Etiqueta> totalObjetos = etiquetado.etiquetas.Where(c => c.esPadre == true).ToList();

                foreach (Etiqueta etiquetaPadre in totalObjetos)
                {
                    foreach (Etiqueta etiquetaHija in etiquetaPadre.Hijos)
                    {
                        foreach (Pixel pixel in etiquetaHija.Pixeles)
                        {
                            etiquetaPadre.Pixeles.Add(pixel);
                        }
                        etiquetaHija.Pixeles.Clear();
                    }
                    etiquetaPadre.Hijos.Clear();
                }

                etiquetado.etiquetas.RemoveAll(c => c.esPadre == false);
                etiquetado.etiquetas.RemoveAll(c => c.Pixeles.Count() < area);
                num_objetos = etiquetado.etiquetas.Count();

                //archivo = new FileStream(".\\etiquetado2.txt", FileMode.Create, FileAccess.Write);
                //escribir = new StreamWriter(archivo);

                //for (int p = 0; p < arregloBinario.Length; p++)
                //{
                //    if (p >= n)
                //    {
                //        if ((p % n) == 0)
                //            escribir.WriteLine();
                //    }

                //    int valor = 0;

                //    foreach (Etiqueta e in etiquetado.etiquetas)
                //    {
                //        foreach (Pixel pixel in e.Pixeles)
                //        {
                //            if (pixel.x == (p % n) && pixel.y == (p / n))
                //            {
                //                valor = e.numeroEtiqueta;
                //            }
                //        }
                //    }

                //    datos = valor + " ";
                //    escribir.Write(datos);
                //}
                //escribir.Close();

                etiquetado.calcularMomentos();

                etiquetado.etiquetas.Clear();
                return etiquetado.momentosHu;
            }
            /// <summary>
            /// Cambios los hijos de un padre a otro padre.
            /// </summary>
            /// <param name="padre">Representa el padre al que se le van a pasar los hijos del otro padre.</param>
            /// <param name="hijo">Representa el padre que se va a convertir en hijo.</param>
            private void cambiarHijos(Etiqueta padre, Etiqueta hijo)
            {
                if (!padre.Hijos.Contains(hijo))
                {
                    padre.agregarHijo(hijo);
                    hijo.agregarPadre(padre);
                    hijo.esPadre = false;
                }
                else
                {
                    //Aparentemente nunca entra a esta condición.
                }
                foreach (Etiqueta h in hijo.Hijos)
                {
                    if (!padre.Hijos.Contains(h))
                    {
                        padre.agregarHijo(h);
                        h.agregarPadre(padre);
                    }
                }
                //for (int j = 0; j < hijo.Hijos.Count; j++)
                //{
                //    if (!padre.Hijos.Contains(hijo.Hijos[j]))
                //    {
                //        padre.agregarHijo(hijo.Hijos[j]);
                //        hijo.Hijos[j].agregarPadre(padre);
                //    }
                //}
                hijo.Hijos.Clear();
            }
        }
        public class Etiquetado
        {
            /// <summary>
            /// Representa una lista de objetos de la clase Etiqueta para almacenar cada uno de los objetos contenidos en la imagen.
            /// </summary>
            public List<Etiqueta> etiquetas = new List<Etiqueta>();
            /// <summary>
            /// Representa una lista de objetos de la clase Hu, en la que contiene las coordenadas x, y, así como los mementos invariantes de Hu.
            /// </summary>
            public List<Hu> momentosHu = new List<Hu>();
            /// <summary>
            /// Inserta una nueva etiqueta a la lista de Etiquetas.
            /// </summary>
            /// <param name="numeroEtiqueta">Especifica el número que va a referenciar a la etiqueta.</param>
            public void insertarEtiqueta(int numeroEtiqueta)
            {
                Etiqueta etiqueta = new Etiqueta(numeroEtiqueta);
                this.etiquetas.Add(etiqueta);
            }
            /// <summary>
            /// Calcula los momentos geométricos hasta los momentos invariantes de Hu.
            /// </summary>
            public void calcularMomentos()
            {
                foreach (Etiqueta objeto in etiquetas.Where(c => c.esPadre == true))
                {
                    Hu hu = new Hu();
                    hu.calcularMomentosGeometricos(objeto);
                    hu.calcularMomentosCentrales(objeto);
                    hu.calcularMomentosCentralesNormalizados();
                    hu.calcularMomentosHu();
                    momentosHu.Add(hu);

                }
            }
        }
        /// <summary>
        /// Clase que contiene los pixeles correspondientes a una etiqueta
        /// </summary>
        public class Etiqueta
        {
            /// <summary>
            /// Representa el número de eqitueta.
            /// </summary>
            public int numeroEtiqueta { get; set; }
            /// <summary>
            /// Lista de píxeles contenidos en cada objeto de la clase Etiqueta.
            /// </summary>
            public List<Pixel> Pixeles = new List<Pixel>();
            /// <summary>
            /// Lista de hijos que puede contener un objeto de la clase Etiqueta.
            /// </summary>
            public List<Etiqueta> Hijos = new List<Etiqueta>();
            /// <summary>
            /// Es una referencia hacia su etiqueta padre.
            /// </summary>
            public Etiqueta Padre;
            /// <summary>
            /// Representa un valor para conocer si la etiqueta es padre o hijo.
            /// </summary>
            public bool esPadre;
            /// <summary>
            /// Método constructor que asigna el número de etiqueta y siempre asigna la etiqueta como padre.
            /// </summary>
            /// <param name="numeroEtiqueta"></param>
            public Etiqueta(int numeroEtiqueta)
            {
                this.numeroEtiqueta = numeroEtiqueta;
                this.esPadre = true;
            }
            /// <summary>
            /// Inserta un objeto de la clase Pixel a la lista Pixel.
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public void insertarPixel(short x, short y)
            {
                Pixel pixel = new Pixel(x, y);
                Pixeles.Add(pixel);
            }
            /// <summary>
            /// Agrega un hijo a la lista Etiqueta.
            /// </summary>
            /// <param name="etiqueta"></param>
            public void agregarHijo(Etiqueta etiqueta)
            {
                this.Hijos.Add(etiqueta);
            }
            /// <summary>
            /// Hace referencia al padre de este objeto de la clase Etiqueta.
            /// </summary>
            /// <param name="etiqueta"></param>
            public void agregarPadre(Etiqueta etiqueta)
            {
                this.Padre = etiqueta;
            }
        }
        /// <summary>
        /// Clase que calcula los momentos geométricos hasta los momentos invariantes de Hu.
        /// </summary>
        public class Hu
        {
            private double[,] m;
            private double[,] mu;
            private double[,] eta;
            /// <summary>
            /// Momenos invariantes de Hu.
            /// </summary>
            public double[] fi;

            /// <summary>
            /// Coordenada x del objeto encontrado.
            /// </summary>
            public double x;
            /// <summary>
            /// Coordenada y del objeto encontrado.
            /// </summary>
            public double y;

            /// <summary>
            /// Calcula los momentos geométicos de un objeto en particular.
            /// </summary>
            /// <param name="objeto">Representa un objeto de clase Etiqueta.</param>
            public void calcularMomentosGeometricos(Etiqueta objeto)
            {
                m = new double[4, 4];

                foreach (Pixel pixel in objeto.Pixeles)
                {
                    for (int p = 0; p <= 3; p++)
                    {
                        for (int q = 0; q <= 3; q++)
                        {
                            if ((p + q) <= 3)
                            {
                                m[p, q] += Math.Pow(pixel.x, p) * Math.Pow(pixel.y, q);
                            }
                        }
                    }
                }
                x = m[1, 0] / m[0, 0];
                y = m[0, 1] / m[0, 0];
            }
            /// <summary>
            /// Calcula los momentos centrales de un objeto en particular.
            /// </summary>
            /// <param name="objeto">Representa un objeto de clase Etiqueta.</param>
            public void calcularMomentosCentrales(Etiqueta objeto)
            {
                mu = new double[4, 4];

                foreach (Pixel pixel in objeto.Pixeles)
                {
                    for (int p = 0; p <= 3; p++)
                    {
                        for (int q = 0; q <= 3; q++)
                        {
                            if ((p + q) <= 3)
                            {
                                mu[p, q] += Math.Pow(pixel.x - x, p) * Math.Pow(pixel.y - y, q);
                            }
                        }
                    }
                }
            }
            /// <summary>
            /// Calcula los momentos centrales normalizados.
            /// </summary>
            public void calcularMomentosCentralesNormalizados()
            {
                eta = new double[4, 4];

                for (int p = 0; p <= 3; p++)
                {
                    for (int q = 0; q <= 3; q++)
                    {
                        if ((p + q) >= 2)
                        {
                            double gama = (p + q) / 2 + 1;
                            eta[p, q] = mu[p, q] / Math.Pow(mu[0, 0], gama);
                        }
                    }
                }
            }
            /// <summary>
            /// Calcula los momentos invariantes de Hu.
            /// </summary>
            public void calcularMomentosHu()
            {
                fi = new double[7];

                fi[0] = eta[2, 0] + eta[0, 2];
                fi[1] = Math.Pow(eta[2, 0] - eta[0, 2], 2) + 4 * Math.Pow(eta[1, 1], 2);
                fi[2] = Math.Pow(eta[3, 0] - 3 * eta[1, 2], 2) + 3 * Math.Pow(eta[2, 1] - eta[0, 3], 2);

                //fi[h, 1] = eta[2, 0] + eta[0, 2];
                //fi[h, 2] = Math.Pow(eta[2, 0] - eta[0, 2], 2) + (4 * Math.Pow(eta[1, 1], 2));
                //fi[h, 3] = Math.Pow(eta[3, 0] - 3 * eta[1, 2], 2) + (3 * Math.Pow(eta[2, 1] - eta[0, 3], 2));
                //fi[h, 4] = Math.Pow(eta[3, 0] + eta[1, 2], 2) + Math.Pow(eta[2, 1] + eta[0, 3], 2);
                //fi[h, 5] = (eta[3, 0] - (3 * eta[1, 2])) * (eta[3, 0] + eta[1, 2]) * (Math.Pow(eta[3, 0] + eta[1, 2], 2) - (3 * Math.Pow(eta[2, 1] + eta[0, 3], 2))) + (3 * eta[2, 1] - eta[0, 3]) * (eta[2, 1] + eta[0, 3]) * (3 * Math.Pow(eta[3, 0] + eta[1, 2], 2) - Math.Pow(eta[2, 1] + eta[0, 3], 2));
                //fi[h, 6] = (eta[2, 0] - eta[0, 2]) * (Math.Pow(eta[3, 0] + eta[1, 2], 2) - (eta[2, 1] + eta[0, 3])) + 4 * eta[1, 1] * (eta[3, 0] + eta[1, 2]) * (eta[2, 1] + eta[0, 3]);
                //fi[h, 7] = (3 * eta[2, 1] - eta[3, 0]) * (eta[3, 0] + eta[1, 2]) * (Math.Pow(eta[3, 0] + eta[1, 2], 2) - 3 * Math.Pow(eta[2, 1] + eta[0, 3], 2)) + (3 * eta[1, 2] - eta[3, 0]) * (eta[2, 1] + eta[0, 3]) * (3 * Math.Pow(eta[3, 0] + eta[1, 2], 2) - Math.Pow(eta[2, 1] + eta[0, 3], 2));
            }
        }
        /// <summary>
        /// Clase que contiene las coordenadas x,y de un pixel a etiquetar.
        /// </summary>
        public class Pixel
        {
            /// <summary>
            /// Representa la coordenda x del objeto.
            /// </summary>
            public short x { get; set; }
            /// <summary>
            /// Representa la coordenda y del objeto.
            /// </summary>
            public short y { get; set; }
            /// <summary>
            /// Método constructor que asigna las coordenadas x,y del objeto.
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public Pixel(short x, short y)
            {
                this.x = x;
                this.y = y;
            }
        }
    }
}
