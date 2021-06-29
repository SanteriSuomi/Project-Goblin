using UnityEngine;

// This class is just for testing bow animation and will be removed
public class BowAnimTest : MonoBehaviour
{

    [SerializeField]
    Animator bowAnimator;
    [SerializeField]
    Animator playerAnimator;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            bowAnimator.SetTrigger("Shoot");
            playerAnimator.SetTrigger("Shoot");
        }
    }
}
