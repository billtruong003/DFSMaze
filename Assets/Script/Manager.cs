using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Manager : MonoBehaviour
{
    public static Manager Instance;

    public GameObject playerPrefab;  // Tham chiếu đến prefab của player
    public SO_DFS descriptionDFS;
    public Tilemap StartEnd;  // Tilemap chứa điểm bắt đầu và kết thúc
    public Tile StartPointTile;  // Tile của điểm bắt đầu
    public Image ImgEnd;
    public Image Chest;
    public GameObject ChangeSlide1;
    public GameObject ChangeSlide2;
    public GameObject ReloadBtn;
    public TextMeshProUGUI Description;
    public bool ChestOpened;
    private int DfsIndex = 0;
    private void Awake()
    {
        descriptionDFS.ResetCheck();
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    public void TriggerChest()
    {
        ImgEnd.gameObject.SetActive(true);
    }
    public void OpenChest()
    {
        if (ChestOpened)
            return;
        Chest.GetComponent<Animator>().SetTrigger("OpenChest");
        Chest.transform.GetChild(0).gameObject.SetActive(false);
        ChestOpened = true;
        InitDescription();
    }

    public void SpawnPlayerAtStart()
    {
        Vector3 SpawnPoint = new Vector3(2.5f, 2.5f);
        playerPrefab.transform.position = SpawnPoint;
    }
    public void NextDFSDescription()
    {
        if (!ChestOpened)
            return;

        DfsIndex++;
        if (DfsIndex >= descriptionDFS.DfsDes.Count)
            DfsIndex = 0;

        descriptionDFS.DfsDes[DfsIndex].Check();
        Debug.Log(DfsIndex);
        descriptionDFS.FullFill();
        string description = descriptionDFS.DfsDes[DfsIndex].GetDescription();
        Description.GetComponent<Animator>().SetTrigger("ShowUp");
        Description.text = description;

    }
    public void InitDescription()
    {
        descriptionDFS.DfsDes[DfsIndex].Check();
        descriptionDFS.FullFill();
        ChangeSlide1.gameObject.SetActive(true);
        ChangeSlide2.gameObject.SetActive(true);
        Description.fontSize = 19;
        string description = descriptionDFS.DfsDes[DfsIndex].GetDescription();
        Description.GetComponent<Animator>().SetTrigger("ShowUp");
        Description.text = description;
    }
    public void BackDFSDescription()
    {
        if (!ChestOpened)
            return;

        DfsIndex--;
        if (DfsIndex < 0)
            DfsIndex = descriptionDFS.DfsDes.Count - 1;

        descriptionDFS.DfsDes[DfsIndex].Check();
        descriptionDFS.FullFill();
        string description = descriptionDFS.DfsDes[DfsIndex].GetDescription();
        Description.GetComponent<Animator>().SetTrigger("ShowUp");
        Description.text = description;
    }
    public void ReloadBtnAppear()
    {
        if (ReloadBtn.activeSelf)
            return;
        ReloadBtn.SetActive(true);
    }
    public void ReloadGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
