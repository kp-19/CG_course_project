using UnityEngine;

public class ShipIslandCollisionHandler : MonoBehaviour
{
    public Rigidbody shipRigidbody;
    public ShipController shipController;
    public float bounceForce = 500f;
    public float bounceDuration = 1.5f;

    private bool isBouncing = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Island") && !isBouncing)
        {
            Debug.Log("Ship collided with an island. Bouncing back!");

            isBouncing = true;

            // Disable movement
            shipController.BlockMovement(true);

            // Stop current movement
            shipRigidbody.linearVelocity = Vector3.zero;

            // Apply bounce force
            Vector3 bounceDirection = -transform.forward;
            shipRigidbody.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);

            // Re-enable movement after a short delay
            Invoke(nameof(ResetBounce), bounceDuration);
        }
    }

    private void ResetBounce()
    {
        shipController.BlockMovement(false);
        isBouncing = false;
    }
}
