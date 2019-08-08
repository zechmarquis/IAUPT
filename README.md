# IAUPT
Librería para entrenar la red neuronal perceptrón multicapa, para extraer momentos geométricos y momentos invariantes de Hu para el reconocimiento de objetos.

Esta es una librería muy sencilla para entrenar un perceptrón multicapa con el lenguaje de programación C#.

Para poder entrenar un conjuto de patrones, primero debes de crear un archivo con extensión *.pml con la arquitectura de la red neuronal y con los datos de configuración, como el factor de aprendizaje, número de iteraciones, error mínimo, número de patrones y abajo los patones de entrada y hasta el último los patrones de salida deseados.

Una vez creado el archivo de entrenamiento, se instancia el objeto y recibe la ruta y nombre del archivo en el método constructor, esto es para iniciar todas las propiedades de la clase.

Posteriormente se ejecuta el método entrenar y se entrena la red creando un archivo con extensión *.ppm, el cual contiene los pesos y umbrales óptimos para reconocer un nuevo patrón.

Proceso de entrenamiento.

PerceptronMulticapa obj = new PerceptronMulticapa("ruta\archivo.pml");

obj.entrenar();


Proceso de reconocimiento.

PerceptronMulticapa obj = new PerceptronMulticapa("ruta\archivo.ppm"); //Este es el archivo que se generó al entrenar la red.

double[] patrones = new double[2.15, 3.56, 7.46]; //Estos patrones son para ejemplificar como debe ser el patron a reconocer.

obj.reconocer(patrones);