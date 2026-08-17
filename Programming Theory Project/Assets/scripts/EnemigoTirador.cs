using UnityEngine;

public class EnemigoTirador : EnemigoBase
{
    [Header("Ajustes del Tirador")]
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private float rangoAtaque = 12f;
    [SerializeField] private float cadenciaDisparo = 2f;
    [SerializeField] private float velocidadProyectil = 15f;
    [SerializeField] private string tagJugador = "Player";

    private Transform jugador;
    private float tiempoSiguienteDisparo;

    protected override void Start()
    {
        // Importante: Ejecuta la inicialización de la salud y puntos de patrulla de EnemigoBase
        base.Start();

        BuscarJugador();
    }

    protected override void Update()
    {
        if (jugador == null)
        {
            BuscarJugador();
            base.Update(); // Si no hay jugador, sigue patrullando entre Punto A y Punto B
            return;
        }

        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        if (distanciaAlJugador <= rangoAtaque)
        {
            // Apuntar hacia el jugador en el eje horizontal
            OrientarHaciaJugador();

            // Gestionar la cadencia de disparo
            if (Time.time >= tiempoSiguienteDisparo)
            {
                Atacar();
                tiempoSiguienteDisparo = Time.time + cadenciaDisparo;
            }
        }
        else
        {
            // Si el jugador está fuera de rango, reanuda el movimiento de patrulla del padre
            base.Update();
        }
    }

    // IMPLEMENTACIÓN OBLIGATORIA DEL MÉTODO ABSTRACTO DE ENEMIGOBASE
    public override void Atacar()
    {
        if (proyectilPrefab == null || puntoDisparo == null)
        {
            Debug.LogError($"[EnemigoTirador] Referencias faltantes en el Inspector de {gameObject.name}");
            return;
        }

        // Orientar el punto de disparo directamente al objetivo
        puntoDisparo.LookAt(jugador);

        // Instanciar proyectil
        GameObject proyectil = Instantiate(proyectilPrefab, puntoDisparo.position, puntoDisparo.rotation);

        // Aplicar fuerza/velocidad
        if (proyectil.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.velocity = puntoDisparo.forward * velocidadProyectil;
        }
    }

    private void BuscarJugador()
    {
        GameObject objJugador = GameObject.FindGameObjectWithTag(tagJugador);
        if (objJugador != null)
        {
            jugador = objJugador.transform;
        }
    }

    private void OrientarHaciaJugador()
    {
        Vector3 direccion = (jugador.position - transform.position).normalized;
        direccion.y = 0f; // Mantiene al enemigo nivelado horizontalmente

        if (direccion != Vector3.zero)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 10f);
        }
    }

    // Muestra el rango de alcance en la vista Scene de Unity
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }
}