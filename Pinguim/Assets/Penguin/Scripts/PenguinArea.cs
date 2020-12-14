using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using TMPro;

public class PenguinArea : MonoBehaviour
{
    [Tooltip("O agente dentro da Area  ")]
    public PenguinAgent penguinAgent;

    [Tooltip("O bebe pinguim dentro da area")]
    public GameObject penguinBaby;

    [Tooltip("O texto em TextMeshPro que mostra a recompensa cumulativa do agente ")]
    public TextMeshPro cumulativeRewardText;

    [Tooltip(" O Prefab do Peixe vivo ")]
    public Fish fishPrefab;

    private List<GameObject> fishList;

    public void ResetArea()
    {
        RemoveAllFish();
        PlacePenguin();
        PlaceBaby();
        SpawnFish(4, .5f);
    }

    /// <summary>
    /// Remove um peixe especifico da arena quando ele for comido
    /// </summary>
    /// <param name="fishObject">The fish to remove</param>
    public void RemoveSpecificFish(GameObject fishObject)
    {
        fishList.Remove(fishObject);
        Destroy(fishObject);
    }

    /// <summary>
        /// Numero de peixes restantes
    /// </summary>
    public int FishRemaining
    {
        get { return fishList.Count; }
    }

    /// <summary>
    /// Escolhe um  posição no plano X-Z dentro da forma  de meio dunut
    /// </summary>
    /// <param name="center">The center of the donut</param>
    /// <param name="minAngle">Minimum angle of the wedge</param>
    /// <param name="maxAngle">Maximum angle of the wedge</param>
    /// <param name="minRadius">Minimum distance from the center</param>
    /// <param name="maxRadius">Maximum distance from the center</param>
    /// <returns>A position falling within the specified region</returns>
    public static Vector3 ChooseRandomPosition(Vector3 center, float minAngle, float maxAngle, float minRadius, float maxRadius)
    {
        float radius = minRadius;
        float angle = minAngle;

        if (maxRadius > minRadius)
        {
            
            //Pega um raio aleatorio
            radius = UnityEngine.Random.Range(minRadius, maxRadius);
        }

        if (maxAngle > minAngle)
        {
            
            //Pega um angulo aleatorio
            angle = UnityEngine.Random.Range(minAngle, maxAngle);
        }

        
        //Posição Central + vetor de em frente rotacionado no eixo Y por "angulo" em graus,multiplicado pelo "raio"
        return center + Quaternion.Euler(0f, angle, 0f) * Vector3.forward * radius;
    }

    /// <summary>
    /// Remove todos os peixes da area 
    /// </summary>
    private void RemoveAllFish()
    {
        if (fishList != null)
        {
            for (int i = 0; i < fishList.Count; i++)
            {
                if (fishList[i] != null)
                {
                    Destroy(fishList[i]);
                }
            }
        }

        fishList = new List<GameObject>();
    }


    /// <summary>
    /// Coloca um pinguin na area
    /// </summary>
    private void PlacePenguin()
    {
        Rigidbody rigidbody = penguinAgent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        penguinAgent.transform.position = ChooseRandomPosition(transform.position, 0f, 360f, 0f, 9f) + Vector3.up * .5f;
        penguinAgent.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
    }

    /// <summary>
    /// Coloca um bebe na area
    /// </summary>
    private void PlaceBaby()
    {
        Rigidbody rigidbody = penguinBaby.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        penguinBaby.transform.position = ChooseRandomPosition(transform.position, -45f, 45f, 4f, 9f) + Vector3.up * .5f;
        penguinBaby.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    /// <summary>
    /// Instancia alguns peixes na area e determina suas velocidades
    /// </summary>
    /// <param name="count">The number to spawn</param>
    /// <param name="fishSpeed">The swim speed</param>
    private void SpawnFish(int count, float fishSpeed)
    {
        for (int i = 0; i < count; i++)
        {
            
            //Instancia e coloca o peixe
            GameObject fishObject = Instantiate<GameObject>(fishPrefab.gameObject);
            fishObject.transform.position = ChooseRandomPosition(transform.position, 100f, 260f, 2f, 13f) + Vector3.up * .5f;
            fishObject.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

            
            // Determina que o peixe é pai do transform da area
            fishObject.transform.SetParent(transform);

            
            //Mantem o rastreamento do peixe
            fishList.Add(fishObject);

            //Determina a velocidade do peixe
            fishObject.GetComponent<Fish>().fishSpeed = fishSpeed;
        }
    }

    /// <summary>
    /// Chamado quando o jogo começa
    /// </summary>
    private void Start()
    {
        ResetArea();
    }

    /// <summary>
    /// Chamado a cada frame
    /// </summary>
    private void Update()
    {
    
        //Atualiza o texto da recompensa cumulativa
        cumulativeRewardText.text = penguinAgent.GetCumulativeReward().ToString("0.00");
    }
}
