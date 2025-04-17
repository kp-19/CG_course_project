using UnityEngine;

public class TreasureController : MonoBehaviour
{
    public PlayerUIController playerUIController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (playerUIController != null)
            {
                playerUIController.AddTreasure(50);
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (playerUIController != null)
            {
                playerUIController.AddTreasure(50);
                Destroy(gameObject);
            }
        }
    }
}
