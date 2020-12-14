using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PenguinAgent : Agent
{
    [Tooltip("O quão rapido o agente se move para frente ")]
    public float moveSpeed = 5f;

    [Tooltip(" O quão rapido o agente vira ")]
    public float turnSpeed = 180f;

    [Tooltip("Um Prefab de coração que aparece quando o bebe é alimentado ")]
    public GameObject heartPrefab;

    [Tooltip(" Um Prefab de peixe regurgitado que aparece quando o bebe é alimentado ")]
    public GameObject regurgitatedFishPrefab;

    private PenguinArea penguinArea;
    new private Rigidbody rigidbody;
    private GameObject baby;
    private bool isFull; //  Se Verdadeiro , o pinguim está de estomago cheio

    /// <summary>
    
    /// Setup Inicial , chamado quando o agente é instanciado
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();
        penguinArea = GetComponentInParent<PenguinArea>();
        baby = penguinArea.penguinBaby;
        rigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Realiza ações baseado num vetor de numeros 
    /// </summary>
    /// <param name="actionBuffers">The struct of actions to take</param>
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {

        //Converte a primeira ação para um movimento para frente
        float forwardAmount = actionBuffers.DiscreteActions[0];

        //Converte a segunda ação para virar para esquerda ou direita
        float turnAmount = 0f;
        if (actionBuffers.DiscreteActions[1] == 1f)
        {
            turnAmount = -1f;
        }
        else if (actionBuffers.DiscreteActions[1] == 2f)
        {
            turnAmount = 1f;
        }

        // Aplica Movimento
        rigidbody.MovePosition(transform.position + transform.forward * forwardAmount * moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(transform.up * turnAmount * turnSpeed * Time.fixedDeltaTime);

        //  Aplica uma pequena recompensa negativa a cada etapa para encorajar a ação
        if (MaxStep > 0) AddReward(-1f / MaxStep);
    }


    /// <summary>
    ///
    /// Lê os inputs do teclado e converte para uma lista de ações
    /// Isso é chamado quando o Jogador quiser controlar o agente e parametrizou
    /// o Comportamento como "Heuristic Only" no inspetor de de Parametros de comportamento
    /// </summary>
    /// <returns>A vectorAction array of floats that will be passed into <see cref="AgentAction(float[])"/></returns>
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forwardAction = 0;
        int turnAction = 0;
        if (Input.GetKey(KeyCode.W))
        {

            //move para frente
            forwardAction = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            
            // vira para esquerda
            turnAction = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            
            //vira para direita
            turnAction = 2;
        }

        
        //Põe as ações num vetor
        actionsOut.DiscreteActions.Array[0] = forwardAction;
        actionsOut.DiscreteActions.Array[1] = turnAction;
    }

    /// <summary>
    
    /// Quando o novo episodio começa , reinicia o agente e a area
    /// </summary>
    public override void OnEpisodeBegin()
    {
        isFull = false;
        penguinArea.ResetArea();
    }


    /// <summary>
    /// Coleta todas as observações não Raycast
    /// </summary>
    /// <param name="sensor">The vector sensor to add observations to</param>
    public override void CollectObservations(VectorSensor sensor)
    {
        
        //Se o pinguim comeu um peixe (1 float = 1 valor)
        sensor.AddObservation(isFull);

        
        //Distancia do bebe (1 float = 1 valor)
        sensor.AddObservation(Vector3.Distance(baby.transform.position, transform.position));

        
        //Direção do bebe (1 Vector3 = 3 valores)
        sensor.AddObservation((baby.transform.position - transform.position).normalized);

        
        //Direção para onde o pinguim esta virado (1 Vector3 = 3 valores)
        sensor.AddObservation(transform.forward);

        // 1 + 1 + 3 + 3 = 8 valores totais 
    }

    /// <summary>
    /// Quando o Agente colidir com algo , tomar uma ação
    /// </summary>
    /// <param name="collision">The collision info</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("fish"))
        {
            // Try to eat the fish
            //Tenta comer um peixe
            EatFish(collision.gameObject);
        }
        else if (collision.transform.CompareTag("baby"))
        {
            // Try to feed the baby
            //Tenta alimentar o bebe
            RegurgitateFish();
        }
    }

    /// <summary>
    /// Checa se o agente esta cheio,se não come um peixe e recebe recompensa
    /// </summary>
    /// <param name="fishObject">The fish to eat</param>
    private void EatFish(GameObject fishObject)
    {
        if (isFull) return; // Não pode comer outro peixe equanto estiver cheio
        isFull = true;

        penguinArea.RemoveSpecificFish(fishObject);

        AddReward(1f);
    }

    /// <summary>
    /// Checa se o agente esta cheio, se sim, alimenta o bebe
    /// </summary>
    private void RegurgitateFish()
    {
        if (!isFull) return; //Nada Para regurgitar
        isFull = false;

        
        //Instancia um peixe regurgitado
        GameObject regurgitatedFish = Instantiate<GameObject>(regurgitatedFishPrefab);
        regurgitatedFish.transform.parent = transform.parent;
        regurgitatedFish.transform.position = baby.transform.position;
        Destroy(regurgitatedFish, 4f);

        
        //Instancia um coração
        GameObject heart = Instantiate<GameObject>(heartPrefab);
        heart.transform.parent = transform.parent;
        heart.transform.position = baby.transform.position + Vector3.up;
        Destroy(heart, 4f);

        AddReward(1f);

        if (penguinArea.FishRemaining <= 0)
        {
            EndEpisode();
        }
    }
}
