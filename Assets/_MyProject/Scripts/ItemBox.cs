using UnityEngine;

public class ItemBox : MonoBehaviour
{
    private int itemsPlaced = 0;
    private int score = 0;
    [SerializeField] private GameUI gameUI;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Crop"))
            return;

        Crop crop = other.GetComponent<Crop>();

        if (crop == null)
            return;

        itemsPlaced++;

        if (!crop.GetIsRotten())
        {
            gameUI.AddScore(crop.GetScoreValue());
        }

        Debug.Log("Crops placed: " + itemsPlaced);
        Debug.Log("Score: " + score);

        gameUI.UpdateCropsPlaced(itemsPlaced);

        Destroy(crop.gameObject);
    }

}
