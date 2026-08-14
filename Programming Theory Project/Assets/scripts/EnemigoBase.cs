using UnityEngine;
// herencia de MonoBehaviour para que pueda ser adjuntado a GameObjects en Unity
// Clase base para todos los enemigos, proporcionando atributos y comportamientos comunes

public abstract class EnemigoBase : MonoBehaviour
{
    // =========================================================
    // ENCAPSULACIÓN:
    // Campos privados/protegidos ocultos al exterior pero visibles en Unity.
    // =========================================================
    
    [Header("Atributos Base")]
    [SerializeField] private string nombreEnemigo = "Enemigo Base";
    [SerializeField] private float saludMaxima = 100f;
    [SerializeField] protected float velocidad = 3f; // Protected si los hijos necesitan alterarla

    [Header("Puntos de Movimiento")]
    [SerializeField] private Transform puntoA;
    [SerializeField] private Transform puntoB;
    [SerializeField] private float distanciaMinima = 0.1f;

    // Estado interno (Nadie desde fuera debe modificar esto directamente)
    private float saludActual;
    private Vector3 destinoActual;

    // =========================================================
    // PROPIEDADES PÚBLICAS (GETTERS):
    // Permiten LEER los datos desde fuera, pero NO MODIFICARLOS.
    // =========================================================
    
    public string NombreEnemigo => nombreEnemigo;
    public float SaludMaxima => saludMaxima;
    public float SaludActual => saludActual;
    public float PorcentajeSalud => saludActual / saludMaxima; // Propiedad calculada útil para barras de vida

    protected virtual void Start()
    {
        saludActual = saludMaxima;

        // Establecemos el primer destino hacia el Punto A si están asignados
        if (puntoA != null)
        {
            destinoActual = puntoA.position;
        }
    }

    protected virtual void Update()
    {
        Moverse();
    }

    // Comportamiento de movimiento de un punto a otro
    protected virtual void Moverse()
    {
        if (puntoA == null || puntoB == null) return;

        // Desplazar al enemigo hacia el destino actual
        transform.position = Vector3.MoveTowards(transform.position, destinoActual, velocidad * Time.deltaTime);

        // Opcional: Rotar hacia la dirección del movimiento
        Vector3 direccion = (destinoActual - transform.position).normalized;
        if (direccion != Vector3.zero)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 10f);
        }

        // Comprobar si ha llegado al punto actual
        if (Vector3.Distance(transform.position, destinoActual) <= distanciaMinima)
        {
            CambiarDestino();
        }
    }

    protected virtual void CambiarDestino()
    {
        // Alternar el destino entre Punto A y Punto B
        if (destinoActual == puntoA.position)
        {
            destinoActual = puntoB.position;
        }
        else
        {
            destinoActual = puntoA.position;
        }
    }

    // =========================================================
    // ENCAPSULACIÓN DE LÓGICA DE NEGOCIO:
    // La única forma de alterar 'saludActual' es llamando a este método validado.
    //Abstracto: Cada tipo de enemigo implementará su propia versión del método Atacar.
    // =========================================================
    public virtual void RecibirDano(float cantidad)
    {
        if (cantidad <= 0) return; // Validación previa de datos

        saludActual -= cantidad;
        saludActual = Mathf.Clamp(saludActual, 0f, saludMaxima); // Evitamos valores negativos o desbordamientos

        Debug.Log($"{nombreEnemigo} recibió {cantidad} de daño. Salud restante: {saludActual}");

        if (saludActual <= 0)
        {
            Morir();
        }
    }

    //Polimorfismo: Cada tipo de enemigo implementará su propia versión del método Atacar
    public abstract void Atacar();

    protected virtual void Morir()
    {
        Destroy(gameObject);
    }
}