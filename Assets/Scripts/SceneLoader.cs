using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public RoomsManager roomManager;
    public int indexARscene, indexNonARscene;

    [SerializeField] UiCustomButton loadNonAR;
    [SerializeField] UiCustomButton loadAR;

    private void Start()
    {
        loadNonAR.OnClickCallback += LoadNonARScene;
        loadAR.OnClickCallback += LoadARScene;
    }

    public void LoadARScene()
    {
        Debug.Log("Loaded AR Scene");
        SceneManager.LoadScene(indexARscene);

        return;
    }

    public void LoadNonARScene()
    {
        Debug.Log("Loaded Non-AR Scene");
        SceneManager.LoadScene(indexNonARscene);

        return;
    }
}
