using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    public Animator animator;

    private string levelToLoad;

    //activates the trigger to start fading
    //sets the level string
    public void FadeToLevel (string level)
    {
        levelToLoad = level;
        animator.SetTrigger("FadeOut");
    }

    //calls the scene manager to change screen
    public void OnFadeComplete()
    {
        FindObjectOfType<Manager>().ChangeScreen(levelToLoad);
    }
}
