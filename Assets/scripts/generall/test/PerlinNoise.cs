using UnityEngine.UI;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    [SerializeField] private RawImage image;

    private bool gradient;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (image.enabled)
            {
                image.enabled = false;
            }
            else
            {
                DrawNoise();
                image.enabled = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            gradient = !gradient;
            if (image.enabled)
            {
                DrawNoise();
            }
        }
    }

    private void DrawNoise()
    {
        int width = Screen.width;
        int height = Screen.height;
        float scale = Camera.main.orthographicSize * 0.2f;
        //float scale = 10;
        //float offsetX = WorldGeneration.instance.offset.x + Camera.main.transform.position.x - width / 2;
        //float offsetY = WorldGeneration.instance.offset.y + Camera.main.transform.position.y - height / 2;
        Vector2 pos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Debug.Log(pos);
        Chunk chunk = Globals.GetChunk(pos);
        int chunkSize = WorldGeneration.chunkSize;
        float offsetX = (chunk.x * chunkSize - chunkSize / 2 + chunk.TilePos(pos).x + WorldGeneration.instance.offset.x) / (Camera.main.orthographicSize * 2);
        float offsetY = (chunk.y * chunkSize - chunkSize / 2 + chunk.TilePos(pos).y  + WorldGeneration.instance.offset.y) / (Camera.main.orthographicSize * 2);

        Debug.Log(width);
        Debug.Log(scale);

        Texture2D noiseTexture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = ((float)x / height + offsetX) * scale;
                float yCoord = ((float)y / height + offsetY) * scale;
                float noiseValue = Mathf.PerlinNoise(xCoord, yCoord);
                if (gradient)
                {
                    SetPixelGradient(noiseTexture, x, y, noiseValue);
                }
                else
                {
                    SetPixelStep(noiseTexture, x, y, noiseValue);
                }
            }
        }

        noiseTexture.Apply();
        image.texture = noiseTexture;

        void SetPixelGradient(Texture2D texture, int x, int y, float noiseValue)
        {
            Color color = new Color(noiseValue, noiseValue, noiseValue);
            texture.SetPixel(x, y, color);
        }
        void SetPixelStep(Texture2D texture, int x, int y, float noiseValue)
        {
            Color color;
            if (noiseValue > 0.45)
            {
                color = new Color(1, 1, 1);
            }
            else if (noiseValue > 0.3)
            {
                color = new Color(0.5f, 0.5f, 0.5f);
            }
            else
            {
                color = new Color(0, 0, 0);
            }
            texture.SetPixel(x, y, color);
        }
    }
}
