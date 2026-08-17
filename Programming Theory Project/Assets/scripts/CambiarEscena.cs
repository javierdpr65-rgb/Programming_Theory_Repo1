using UnityEngine;
using UnityEngine.SceneManagement; // Requerido para la gestión de escenas

public class CambiarEscena : MonoBehaviour
{
    // Carga la escena usando su nombre (Ejemplo: "Nivel1", "MenuPrincipal")
    public void CargarEscenaPorNombre(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    // Carga la escena usando su número de índice en Build Settings (Ejemplo: 0, 1, 2)
    public void CargarEscenaPorIndice(int indiceEscena)
    {
        SceneManager.LoadScene(indiceEscena);
    }

    // Método de utilidad para cerrar el juego
    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}