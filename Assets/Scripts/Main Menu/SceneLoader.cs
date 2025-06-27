using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int indexEmptyScene, indexScannedScene;

    [SerializeField] UiCustomButton loadEmpty;
    [SerializeField] UiCustomButton loadScanned;

    private void Start()
    {
        loadEmpty.OnClickCallback += LoadEmptyScene;
        loadScanned.OnClickCallback += LoadScannedScene;
    }

    public void LoadEmptyScene()
    {
        Debug.Log("Loaded Empty Scene");
        SceneManager.LoadScene(indexEmptyScene);

        return;
    }

    public void LoadScannedScene()
    {
        Debug.Log("Loaded Scanned Scene");
        SceneManager.LoadScene(indexScannedScene);

        return;
    }
}
