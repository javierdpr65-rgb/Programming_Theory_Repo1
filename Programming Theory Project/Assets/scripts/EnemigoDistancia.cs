using UnityEngine;

public class EnemigoDistancia : EnemigoBase
{
    // Variables encapsuladas específicas del tirador
    [Header("Ajustes a Distancia")]
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private float cadenciaDisparo = 2f;

    // Propiedad pública para consultar la cadencia si la interfaz de usuario u otro script lo requiere
    public float CadenciaDisparo => cadenciaDisparo;

    public override void Atacar()
    {
        if (proyectilPrefab != null && puntoDisparo != null)
        {
            Instantiate(proyectilPrefab, puntoDisparo.position, puntoDisparo.rotation);
        }
    }
}