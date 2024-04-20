using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField]AIScriptableObjects _stats;

    public AIScriptableObjects stats => _stats;

    GameObject _player;
    GameObject _playerCollider;

    public GameObject player => _player;

    [SerializeField]Transform _spawn;
    public Transform spawn => _spawn;

    FlashLight _flashLight;

    public FlashLight flashLight => _flashLight;    

    Camera _cam;

    public Camera cam => _cam;

    [SerializeField]Animator _anim;

    public Animator anim => _anim;

    [Header ("Tags")]
    [SerializeField] TagScriptableObject playerTag;
    [SerializeField] TagScriptableObject playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        _player = TagExtensions.FindWithTag(gameObject, playerTag);
        _playerCollider = TagExtensions.FindWithTag(gameObject, playerCollider);

        _flashLight = player.GetComponentInChildren<FlashLight>();

        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerIsSafe();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (TagExtensions.HasTag(collision.gameObject, playerCollider))
        {
            GameManager.GameManagerInstance.GameOver();
        }

    }

    void PlayerIsSafe()
    {
        if (player.GetComponent<PlayerController>().safe == true) 
        {
            this.transform.position = _spawn.position;
            this.gameObject.GetComponent<AIStateManager>().enabled = false;
        }
        else
        {
            this.gameObject.GetComponent<AIStateManager>().enabled = true;
        }
    }
}
