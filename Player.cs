using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody; 
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audio;
    [SerializeField] private Text coinsUI;
    [SerializeField] private Image menu;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera finishCamera;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private LineRenderer linePath;
    [SerializeField] private int coins = 0;   
    public List<Vector3> path = new List<Vector3>();  
    private Vector3 temp;
    private Vector3 selfDistance;
    public Vector3 pointDistance;
    public bool isMove = false;
    private int numberCheckpoint = 0;
    public float distance = 0f;

    void Start()
    {
        numberCheckpoint = 0;
        isMove = false;
        menu.gameObject.SetActive(false);
        coins = PlayerPrefs.GetInt("Coins", 0);
        coinsUI.text = coins.ToString();
    } 

    void Update()
    {
       if (Input.touchCount > 0)
       {
           Touch touch = Input.GetTouch(0);
       
           switch (touch.phase)
           {
               //case TouchPhase.Moved:
               //    Ray ray = Camera.main.ScreenPointToRay(touch.position);
               //    Physics.Raycast(ray, out RaycastHit hit); ;
               //    temp = hit.point;
               //    path.Add(temp);
               //    break;
       
               case TouchPhase.Ended:                   
                   animator.SetFloat("MoveSpeed", 1);
                   isMove = true;
                   break;                   
           }
       }

        if (isMove)
        {
            selfDistance = new Vector3(transform.position.x, 0, transform.position.z);
            pointDistance = new Vector3(path[numberCheckpoint].x, 0, path[numberCheckpoint].z);
            distance = Vector3.Distance(selfDistance, pointDistance);
            if (distance < 0.5f && numberCheckpoint < path.Count)
            {
                numberCheckpoint++;              
            }

            if (numberCheckpoint == path.Count)
            {
                animator.SetFloat("MoveSpeed", 0);               
                isMove = false;
                numberCheckpoint = 0;
            }
        }     
    }


    private void FixedUpdate()
    {
        if (isMove)
        {          
            agent.SetDestination(path[numberCheckpoint]);
        }
        
    }      

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Coin":
                Destroy(collision.gameObject);
                audio.Play();
                coins++;
                coinsUI.text = coins.ToString();
                break;

            case "Finish":
                animator.Play("Dance");
                finishCamera.gameObject.SetActive(true);
                mainCamera.gameObject.SetActive(false);
                Invoke("Finish",5);
                isMove = false;
                break;
        }
    }

    private void Finish()
    {
        menu.gameObject.SetActive(true);       
    }

    public void NextLevel(int level)
    {
        SceneManager.LoadScene(level);
        PlayerPrefs.SetInt("Coins", coins);
    }

    public void RestartLevel(int level)
    {

        SceneManager.LoadScene(level);
        coins = PlayerPrefs.GetInt("Coins", 0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

}
