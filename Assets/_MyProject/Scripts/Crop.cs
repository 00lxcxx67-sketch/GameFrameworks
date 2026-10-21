using UnityEngine;

public class Crop : MonoBehaviour
{
    [SerializeField] private float rotTime = 10f;
    [SerializeField] private Color rottenColor = Color.red;
    [SerializeField] public int scoreValue = 100;
    private bool isRotten = false;
    private Renderer cropRenderer;
    private float timer;

    void Start()
    {
        timer = rotTime;
        cropRenderer = GetComponentInChildren<Renderer>();

    }

    void Update()
    {
        if (isRotten)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Rot();
        }
    }

    void Rot()
    {
        isRotten = true;

        if (cropRenderer != null)
        {
            cropRenderer.material.color = rottenColor;
        }

        Debug.Log(gameObject.name + " has rotted!");
    }

    public bool GetIsRotten()
    {
        return isRotten;
    }

    public int GetScoreValue()
    {
        return scoreValue;
    }

}
