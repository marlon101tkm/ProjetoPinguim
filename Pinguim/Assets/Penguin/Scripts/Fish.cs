using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [Tooltip("A velocide de nado")]
    public float fishSpeed;

    private float randomizedSpeed = 0f;
    private float nextActionTime = -1f;
    private Vector3 targetPosition;

    /// <summary>
    /// Chamado a cada etapa de tempo
    /// </summary>
    private void FixedUpdate()
    {
        if (fishSpeed > 0f)
        {
            Swim();
        }
    }

    /// <summary>
    /// Nada entre posições aleatorias
    /// </summary>
    private void Swim()
    {
        
        //Se for a hora para proxima ação, pega uma nova velocidade e destino
        //Senão nada para o destino
        
        if (Time.fixedTime >= nextActionTime)
        {
            
            //Velocidade aleatoria
            randomizedSpeed = fishSpeed * UnityEngine.Random.Range(.5f, 1.5f);

            
            //Pega um algo aleatorio
            targetPosition = PenguinArea.ChooseRandomPosition(transform.parent.position, 100f, 260f, 2f, 13f);

            
            //Rotaciona em direção ao alvo
            transform.rotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);

            
            //Calcula o tempo para chegar lá
            float timeToGetThere = Vector3.Distance(transform.position, targetPosition) / randomizedSpeed;
            nextActionTime = Time.fixedTime + timeToGetThere;
        }
        else
        {
            
            //Garante que o peixe não nade alem do alvo
            Vector3 moveVector = randomizedSpeed * transform.forward * Time.fixedDeltaTime;
            if (moveVector.magnitude <= Vector3.Distance(transform.position, targetPosition))
            {
                transform.position += moveVector;
            }
            else
            {
                transform.position = targetPosition;
                nextActionTime = Time.fixedTime;
            }
        }
    }
}
