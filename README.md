Adjuntamos en esta carpeta: la presentación que usamos en clase, los ficheros de prueba utilizados, este ReadMe para saber cómo funciona esta última versión del programa (ligeramente modificada respecto a la que mostramos en clase), el código utilizado y el ejecutable del programa.
__________________________________________________________________________________________________________________________________________________________________________________________________________________________________________

Lenguaje: Visual Studio C# 2019 (no se puede abrir con la versión de 2010 y posiblemente con ninguna otra anterior a la nuestra) usando WPF para mostrar las ventanas y la extensión GMAP.NET (solo funciona con conexión a Internet) para el mapa

Decodificador de ficheros ASTERIX (.ast) capaz de leer datos de CAT10, CAT20 y CAT21 (versiones 023 y 2.4) mediante cuatro clases:
 - Fichero: Lee los datos del fichero, los pasa a hexadecimal y crea las listas y las tablas de categorías
 - CAT10, CAT20 y CAT21: Leen un paquete del fichero y asignan los valores correspondientes a cada uno de los ítems que contenga el paquete

Visualización de los datos usando WPF:
 - En forma de tabla (mediante binding de DataTable a DataGrid), pudiendo expandir un paquete en una tabla indiviadual (lectura fácil), con opción a seleccionar la categoría a mostrar y a filtrar por número de paquete, callsign o track number
 - En forma de mapa (usando GMAP), que puede ser del aeropuerto de Barcelona, de Barcelona ciudad o de Cataluña (aunque se puede arrastrar el mapa a cualquier sitio del mundo). Simulación real del fichero con opción a pararla, reanudarla o reiniciarla y a acelerarla. Permite también elegir si mostrar solo los aviones o cualquier vehículo y si mostrar o el último trozo de la trayectoria.

Mensaje de ayuda:
 - Explicación detallada de cada componente de la app en caso de duda

About Us:
 - Pequeña ventana con fotos y nombres de cada uno de los componentes del grupo

Instalador:
 - Para instalar el programa simplemente click en el archivo .msi y despues seguir los pasos
_________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________
Grupo 2: Mercè Cuixart, Irene Medina, Laura Parga, Irene Rodríguez y Marc Vila.


- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
Inside the project folder: Presentation used in class, test files for the app, this ReadMe to explain how the last version of the app works (some changes made from the demo shown in class), source code from the project and the executable file to install the program.
________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________

Language: Visual Studio C# 2019 (compatibility issues with Visual Studio 2010 and probably any older version of the program), implementing WPF for the display windows and the GMAP.NET library extension (only works with Internet connection) for the display on map

ASTERIX (.ast) file decoder capable of reading files with CAT10, CAT20 and CAT21 (versions 023 and 2.4) by using four classes:
 - Fichero: Reads the data from an .ast file, translates them to hexadecimal and creates lists and tables for each of the three categories
 - CAT10, CAT20 and CAT21: Read a single Data field from the file and translates the package into values corresponding to each of the data items that the said category has

Display of data using WPF:
 - Table visualization (using binding of DataTable to DataGrid), being able to expand any package into an idividual table (easy read), with the option to select the category you want to show and filter for package number, callsign or track number
 - Map visualization (using GMAP), being the main focus El Prat Airport, Barcelona City or Cataluña (still can drag the map to anywhere in the world). Real simulation of the file with option to stop, play, restart and change speed. Also allows to select between showing only airplane or any package received from any source and/or the trail of the trayectory of the said objects.

Help window:
 - Detailed description of every component of the app in case of doubt

About Us:
 - Small window with pictures and name of each person of the team

Executable file:
 - To install the app as program just click on the .msi file and follow the instructions
____________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________
Group 2: Mercè Cuixart, Irene Medina, Laura Parga, Irene Rodríguez y Marc Vila.