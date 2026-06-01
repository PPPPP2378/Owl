using UnityEngine;

public class Owlaction_n : MonoBehaviour
{
    public float detectRange = 3f;

    public AudioSource audioSource;
    public AudioClip owlCry;

    private bool mysteryDetected = false;

    void Update()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            detectRange
        );

        bool foundMystery = false;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Mystery"))
            {
                foundMystery = true;

                if (!mysteryDetected)
                {
                    audioSource.PlayOneShot(owlCry);
                    mysteryDetected = true;
                }

                break;
            }
        }

        if (!foundMystery)
        {
            mysteryDetected = false;
        }
    }

    // SceneÉrÉÖÅ[Ç≈îÕàÕï\é¶
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position,
            detectRange
        );
    }
}
