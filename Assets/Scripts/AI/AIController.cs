using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField]AIScriptableObject _stats;

    public AIScriptableObject stats => _stats;

    GameObject _player;

    public GameObject player => _player;

    [SerializeField]Transform _spawn;
    public Transform spawn => _spawn;

    FlashLight _flashLight;

    public FlashLight flashLight => _flashLight;    

    [SerializeField]Animator _anim;

    public Animator anim => _anim;

    [Header("References")]
    [SerializeField] ReferenceScriptableObject playerReference;

    [Header ("Tags")]
    [SerializeField] TagScriptableObject playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        _player = ReferenceManager.ReferenceManagerInstance.GetReference(playerReference);

        _flashLight = player.GetComponentInChildren<FlashLight>();
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
            this.gameObject.GetComponent<AIStateController>().enabled = false;
        }
        else
        {
            this.gameObject.GetComponent<AIStateController>().enabled = true;
        }
    }
}
