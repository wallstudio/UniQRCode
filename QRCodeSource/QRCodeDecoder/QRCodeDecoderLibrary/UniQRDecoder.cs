using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRCodeDecoderLibrary;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

[ExecuteAlways]
public class UniQRDecoder : MonoBehaviour
{
    public Texture QRTexture
    {
        get => m_RQTexture;
        set { m_RQTexture = value; OnValidate(); }
    }
    [SerializeField] private Texture m_RQTexture;

    string m_DecodedText;

    /// <summary>
    /// Decode QR code from Texture
    /// </summary>
    /// <param name="texture">NOT need readable</param>
    /// <returns></returns>
    public static async Task<string> Decode(Texture texture)
    {
        if (texture == null) return "Texture is null";

        var rt = new RenderTexture(texture.width, texture.height, 0, GraphicsFormat.R8G8B8A8_UNorm);
        try
        {
            Graphics.Blit(texture, rt);

            var taskCS = new TaskCompletionSource<NativeArray<Color32>>();
            AsyncGPUReadback.Request(rt, 0, req =>
            {
                if (req.hasError)
                {
                    taskCS.SetException(new System.Exception("GPU readback error detected."));
                }
                else
                {
                    taskCS.SetResult(req.GetData<Color32>());
                }
            });
            using var rgba = await taskCS.Task;

            var rgb = new (byte r, byte g, byte b)[rgba.Length];
            for (int j = 0; j < rt.height; j++)
            {
                for (int i = 0; i < rt.width ; i++)
                {
                    var index = i + (rt.height - j - 1) * rt.width;
                    var c = rgba[index];
                    rgb[i + j * rt.width] = (c.r, c.g, c.b);
                }
            }
            var result = new QRDecoder().ImageDecoder(new Bitmap(rgb, rt.width));
            return string.Join("\n", result.Select(x => Encoding.UTF8.GetString(x.DataArray)));
        }
        finally
        {
            if(Application.isPlaying)
                Destroy(rt);
            else
                DestroyImmediate(rt);
        }
    }

    void OnGUI()
    {
        using var area = new GUILayout.AreaScope(new Rect(10, 10, Screen.width - 20, Screen.height -20));
        GUILayout.Label(m_DecodedText);
    }

    void OnValidate() => Decode(m_RQTexture).ContinueWith(t => m_DecodedText = t.Result);

}
