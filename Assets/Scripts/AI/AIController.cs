using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public AIScriptableObjects stats;

    public GameObject player;

    Vector2 spawn;

    public Animator anim;

    [SerializeField] TagScriptableObject playerTag;

    // Start is called before the first frame update
    void Start()
    {
        player = TagExtensions.FindWithTag(gameObject, playerTag);

        spawn = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerIsSafe();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            Manager.GameManager.GameManagerInstance.GameOver();
        }

    }

    void PlayerIsSafe()
    {
        if (player.GetComponent<Player.PlayerController>().safe == true) 
        {
            this.transform.position = spawn;
            this.gameObject.GetComponent<AIStateManager>().enabled = false;
        }
        else
        {
            this.gameObject.GetComponent<AIStateManager>().enabled = true;
        }
    }
}
