using UnityEngine;

public class EnemigoCuerpoACuerpo : EnemigoBase
{
    // =========================================================
    // ENCAPSULACIÓN:
    // Variables privadas/protegidas ocultas del exterior pero
    // editables de forma segura desde el Inspector de Unity.
    // =========================================================
    [Header("Ajustes Cuerpo a Cuerpo")]
    [SerializeField] private float danoMelee = 15f;
    [SerializeField] private float rangoAtaque = 1.5f;
    [SerializeField] private float tiempoEntreAtaques = 1.2f;

    // Estado interno (completamente oculto fuera de esta clase)
    private float tiempoSiguienteAtaque = 0f;

    // =========================================================
    // PROPIEDADES PÚBLICAS (GETTERS):
    // Acceso de solo lectura para consultar estadísticas desde la UI o Managers.
    // =========================================================
    public float DanoMelee => danoMelee;
    public float RangoAtaque => rangoAtaque;

    protected override void Start()
    {
        base.Start(); // Inicializa salud y movimiento de la clase padre
    }

    // =========================================================
    // LÓGICA DE ATAQUE ENCAPSULADA:
    // El control del tiempo entre golpes se gestiona internamente.
    // =========================================================
    public override void Atacar()
    {
        // Validamos la cadencia internamente sin depender de scripts externos
        if (Time.time >= tiempoSiguienteAtaque)
        {
            EjecutarAtaqueMelee();
            tiempoSiguienteAtaque = Time.time + tiempoEntreAtaques;
        }
    }

    // Método auxiliar privado: Oculta los detalles del golpe físico/animación
    private void EjecutarAtaqueMelee()
    {
        Debug.Log($"{NombreEnemigo} ataca con espada causando {danoMelee} de daño dentro de un rango de {rangoAtaque}m.");
        
        // Aquí iría la detección del jugador (por ejemplo, Physics.OverlapSphere)
    }

    // Dibujamos el rango en la escena para fácil depuración en Unity
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }
}