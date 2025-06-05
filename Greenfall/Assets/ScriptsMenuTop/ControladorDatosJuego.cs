using UnityEngine;
using System.IO;
public class ControladorDatosJuego : MonoBehaviour
{
   public GameObject jugador;   
    public string archivoGuardado;
    public  DatosJuego datosJuego = new DatosJuego();
    int slotActual = 1;

    private void Awake()
    {
        archivoGuardado = Application.dataPath + "/datosJuego.json";

        jugador = GameObject.FindGameObjectWithTag("Player");
    }




    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CargarDatos();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            GuardarDatos();
        }
    }
    public void GuardarPosicion(Vector2 posicionDeseada)
    {
        DatosJuego nuevosDatos = new DatosJuego();
        nuevosDatos.posicion = posicionDeseada;

        string cadenaJSON = JsonUtility.ToJson(nuevosDatos, true);
        File.WriteAllText(archivoGuardado, cadenaJSON);
        Debug.Log("ControladorDatosJuego: Posición guardada = " + posicionDeseada);
    }

    private void CargarDatos() 
    {
        if (File.Exists(archivoGuardado))
        {
            string contenido = File.ReadAllText(archivoGuardado);
            datosJuego = JsonUtility.FromJson<DatosJuego>(contenido);
            Debug.Log("Posicion jugador: " + datosJuego.posicion);

            jugador.transform.position = datosJuego.posicion;
            if (jugador != null)
            {
                jugador.transform.position = datosJuego.posicion;
            }
            else
            {
                Debug.LogWarning("ControladorDatosJuego: No existe ningún GameObject con tag 'Player' para reposicionar.");
            }
        }
        else
        {
            Debug.Log("El archivo no eciste");
            
        }
    }
    private void GuardarDatos()
    {
        DatosJuego nuevosDatos = new DatosJuego()
        {
            posicion = jugador.transform.position
        };

        string cadenaJSON = JsonUtility.ToJson(nuevosDatos);
        File.WriteAllText(archivoGuardado, cadenaJSON);
        Debug.Log("Archivo Guardado");
    }
}
