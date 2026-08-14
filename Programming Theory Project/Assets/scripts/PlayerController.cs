using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // =========================================================
    // ENCAPSULACIÓN:
    // Variables privadas expuestas al Inspector de forma segura
    // =========================================================
    [Header("Movimiento")]
    [SerializeField] private float velocidadMovimiento = 5f;
    [SerializeField] private float velocidadRotacion = 10f;

    [Header("Ataque")]
    [SerializeField] private float danoAtaque = 25f;
    [SerializeField] private float rangoAtaque = 2f;
    [SerializeField] private Transform puntoAtaque;
    [SerializeField] private LayerMask capaEnemigos;
    [SerializeField] private float cadenciaAtaque = 0.5f;

    // Estado interno
    private float tiempoSiguienteAtaque = 0f;
    private CharacterController characterController;

    // Propiedades de solo lectura para otros sistemas (ej. UI)
    public float DanoAtaque => danoAtaque;

    private void Awake()
    {
        // Obtenemos o añadimos el CharacterController
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            characterController = gameObject.AddComponent<CharacterController>();
        }
    }

    private void Update()
    {
        Mover();
        ProcesarAtaque();
    }

    // =========================================================
    // ABSTRACCIÓN Y ENCAPSULACIÓN DE MOVIMIENTO
    // =========================================================
    private void Mover()
    {
        float movimientoH = Input.GetAxisRaw("Horizontal");
        float movimientoV = Input.GetAxisRaw("Vertical");

        Vector3 direccion = new Vector3(movimientoH, 0f, movimientoV).normalized;

        if (direccion.magnitude >= 0.1f)
        {
            // Rotar hacia la dirección del movimiento
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);

            // Mover al personaje usando CharacterController
            characterController.Move(direccion * velocidadMovimiento * Time.deltaTime);
        }
    }

    // =========================================================
    // POLIMORFISMO Y ABSTRACCIÓN DE ATAQUE
    // =========================================================
    private void ProcesarAtaque()
    {
        if (Time.time >= tiempoSiguienteAtaque)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
            {
                Atacar();
                tiempoSiguienteAtaque = Time.time + cadenciaAtaque;
            }
        }
    }

    private void Atacar()
    {
        Transform origenAtaque = puntoAtaque != null ? puntoAtaque : transform;

        // Detectar colisionadores en el área de ataque
        Collider[] enemigosImpactados = Physics.OverlapSphere(origenAtaque.position, rangoAtaque, capaEnemigos);

        foreach (Collider colisionador in enemigosImpactados)
        {
            // POLIMORFISMO: Buscamos la CLASE BASE abstracta
            EnemigoBase enemigo = colisionador.GetComponent<EnemigoBase>();

            if (enemigo != null)
            {
                // ABSTRACCIÓN: No nos importa si es Melee, Distancia o Rápido;
                // solo llamamos a RecibirDano() y el enemigo gestiona su vida y muerte.
                enemigo.RecibirDano(danoAtaque);
            }
        }
    }

    // Dibujar el rango del ataque en el Editor de Unity
    private void OnDrawGizmosSelected()
    {
        Transform origenAtaque = puntoAtaque != null ? puntoAtaque : transform;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origenAtaque.position, rangoAtaque);
    }
}