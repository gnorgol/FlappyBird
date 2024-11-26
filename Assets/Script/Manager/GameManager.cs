using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}
public class GameManager : MonoBehaviour
{
    private GameState m_GameState;
    [SerializeField]
    private Camera Camera;
    [SerializeField]
    private GameObject m_bird;
    [SerializeField]
    private GameObject m_Background;
    public int sizeDoor = 2;
    public int DistanceBetweenPipes = 6;
    public int PipeSpeed = 1;
    private int m_Score = 0;
    private List<GameObject> m_Pipes = new List<GameObject>();
    [SerializeField]
    private Bird m_Bird;
    public static GameManager instance;
    public GameObject detectPoint;
    public GameObject topPipe;
    public GameObject bottomPipe;


    private int BackgroundHeight;

    public Button StartGameButton;

    public GameState GameState
    {
        get { return m_GameState; }
        set { m_GameState = value; }
    }
    public bool IsGameOver { get { return m_GameState == GameState.GameOver; } }

    public void GameOver()
    {
        m_GameState = GameState.GameOver;
        m_Bird.DisableMovement();
        EventManager.instance.TriggerGameOver();

    }
    private void Awake()
    {
        instance = this;
        StartGameButton.onClick.AddListener(GameStart);
    }
    private void Start()
    {
        BackgroundHeight = (int)m_Background.GetComponent<MeshRenderer>().bounds.size.y;
        int BackgroundWidth = (int)m_Background.GetComponent<MeshRenderer>().bounds.size.x;
        int numberOfPipes = BackgroundWidth / DistanceBetweenPipes + 2;
        //Spawn pipes with cube half the size of the background
        for (int i = 0; i < numberOfPipes; i++)
        {
            GameObject topPipe, bottomPipe;
            (topPipe, bottomPipe) = CreatePipe(BackgroundHeight);
            float TopPipeHeight = topPipe.transform.localScale.y;
            float BottomPipeHeight = bottomPipe.transform.localScale.y;
            m_Pipes.Add(topPipe);
            m_Pipes.Add(bottomPipe);
            topPipe.transform.position = new Vector3(0 + i * DistanceBetweenPipes, BackgroundHeight / 2 - TopPipeHeight/2, 0);
            bottomPipe.transform.position = new Vector3(0 + i * DistanceBetweenPipes, -BackgroundHeight / 2 + BottomPipeHeight / 2, 0);
            //Add beetwen the two pipes a invisible cube to detect if the bird pass the pipes and gain a point
            detectPoint = GameObject.Instantiate(detectPoint);
            detectPoint.name = "DetectPoint";
            //Set the detectPoint position between the two pipes
            detectPoint.transform.position = new Vector3(0 + i * DistanceBetweenPipes, topPipe.transform.position.y - TopPipeHeight / 2 - sizeDoor / 2, 0);
            //Set the detectPoint scale to the size of the door
            detectPoint.transform.localScale = new Vector3(1, sizeDoor, 1);

            //Set the detectPoint to be trigger
            detectPoint.GetComponent<BoxCollider>().isTrigger = true;

            //Set the detectPoint to be child of the topPipe
            detectPoint.transform.parent = topPipe.transform;
            //remove mesh renderer from the detectPoint
            Destroy(detectPoint.GetComponent<MeshRenderer>());

        }
    }
    private void GameStart()
    {
        m_GameState = GameState.Playing;
        StartGameButton.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (isBirdExitingArena() && !IsGameOver && m_GameState == GameState.Playing)
        {
            GameOver();
        }
        else if (!IsGameOver && m_Pipes.Count != 0 && m_GameState == GameState.Playing)
        {
            MovePipe();
            Vector3 position = Camera.WorldToViewportPoint(m_Pipes[0].transform.position);
            if (position.x < 0)
            {
                //Destroy the first pipe
                Destroy(m_Pipes[0]);
                m_Pipes.RemoveAt(0);
                //Destroy the second pipe
                Destroy(m_Pipes[0]);
                m_Pipes.RemoveAt(0);
                //Create a new pipe
                GameObject topPipe, bottomPipe;
                (topPipe, bottomPipe) = CreatePipe((int)m_Background.GetComponent<MeshRenderer>().bounds.size.y);
                float TopPipeHeight = topPipe.transform.localScale.y;
                float BottomPipeHeight = bottomPipe.transform.localScale.y;
                topPipe.transform.position = new Vector3(m_Pipes[m_Pipes.Count - 1].transform.position.x + DistanceBetweenPipes, BackgroundHeight / 2 - TopPipeHeight / 2, 0);
                bottomPipe.transform.position = new Vector3(m_Pipes[m_Pipes.Count - 1].transform.position.x + DistanceBetweenPipes, -BackgroundHeight / 2 + BottomPipeHeight / 2, 0);
                m_Pipes.Add(topPipe);
                m_Pipes.Add(bottomPipe);
                //Add beetwen the two pipes a invisible cube to detect if the bird pass the pipes and gain a point
                detectPoint = GameObject.Instantiate(detectPoint);


                //Set the detectPoint position between the two pipes
                detectPoint.transform.position = new Vector3(m_Pipes[m_Pipes.Count - 1].transform.position.x, topPipe.transform.position.y - TopPipeHeight / 2 - sizeDoor / 2, 0);
                //Set the detectPoint scale to the size of the door
                detectPoint.transform.localScale = new Vector3(1, sizeDoor, 1);
                //Set the detectPoint to be trigger
                detectPoint.GetComponent<BoxCollider>().isTrigger = true;
                //Add the script DetectPoint to the detectPoint

                //Set the detectPoint to be child of the topPipe
                detectPoint.transform.parent = topPipe.transform;
                //remove mesh renderer from the detectPoint
                Destroy(detectPoint.GetComponent<MeshRenderer>());


            }
        }
    }

    public (GameObject, GameObject) CreatePipe(int BackgroundHeight)
    {
        GameObject topPipeClone = GameObject.Instantiate(topPipe);
        bottomPipe = GameObject.Instantiate(bottomPipe);
        topPipeClone.name = "TopPipe";
        bottomPipe.name = "BottomPipe";
        topPipeClone.tag = "Pipe";
        bottomPipe.tag = "Pipe";
        //Add green material to the pipes
        topPipeClone.GetComponent<Renderer>().material.color = Color.green;
        bottomPipe.GetComponent<Renderer>().material.color = Color.green;
        int randomTopOrBootom = Random.Range(0, 2);
        topPipeClone.transform.position = new Vector3(BackgroundHeight / 2, 0, 0);
        bottomPipe.transform.position = new Vector3(BackgroundHeight / 2, 0, 0);
        int randomHeight = (int)Random.Range(1, BackgroundHeight / 2);
        if (randomTopOrBootom == 0)
        {
            topPipeClone.transform.localScale = new Vector3(1, randomHeight, 1);
            int bottomPipeHeight = BackgroundHeight - sizeDoor - randomHeight;
            bottomPipe.transform.localScale = new Vector3(1, bottomPipeHeight, 1);
        }
        else
        {
            bottomPipe.transform.localScale = new Vector3(1, randomHeight, 1);
            int topPipeHeight = BackgroundHeight - sizeDoor - randomHeight;
            topPipeClone.transform.localScale = new Vector3(1, topPipeHeight, 1);
        }


        return (topPipeClone, bottomPipe);
    }
   
    private void MovePipe()
    {
        
        foreach (GameObject pipe in m_Pipes)
        {
            pipe.transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * PipeSpeed;
        }
    }
    private bool isBirdExitingArena()
    {
        //get the position of the bird
        Vector3 position = Camera.WorldToViewportPoint(m_bird.transform.position);
        //if the bird is outside the arena
        if (position.x < 0 || position.x > 1 || position.y < 0 || position.y > 1)
        {
            return true;
        }
        return false;
    }
    private void OnEnable()
    {
        EventManager.OnGainPoint += GainPoint;
    }
    private void OnDisable()
    {
        EventManager.OnGainPoint -= GainPoint;
    }
    private void GainPoint()
    {
        m_Score++;
        EventManager.instance.TriggerScore(m_Score);
    }
    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
