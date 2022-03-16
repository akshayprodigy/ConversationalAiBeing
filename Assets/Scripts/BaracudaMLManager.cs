using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.UI;

public class BaracudaMLManager : MonoBehaviour
{
    const int IMAGE_SIZE = 64;//48;
    const string INPUT_NAME = "Input3";//"input_1";
    const string OUTPUT_NAME = "Plus692_Output_0";//"predictions";//"dense";
    [SerializeField]
    RawImage final, intermediate;


    public CameraView CameraView;
    public Preprocess preprocess;
    public NNModel modelFile;
    string[] labels = {"neutral", "happiness", "surprise",
            "sadness", "anger", "disgust", "fear", "contempt" };//{"angry", "disgust", "scared","happy", "sad", "surprised", "neutral" };
    IWorker worker;

    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        var model = ModelLoader.Load(modelFile);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
        LoadLabels();
        var webCamInput = GetComponent<WebCamInput>();
        webCamInput.OnTextureUpdate.AddListener(OnTextureUpdate);
    }

    void LoadLabels()
    {
        //get only items in quotes
        //var stringArray = labelAsset.text.Split('"').Where((item, index) => index % 2 != 0);
        //get every other item
        //labels = stringArray.Where((x, i) => i % 2 != 0).ToArray();
    }

    // Update is called once per frame
    /*
    void Update()
    {
        WebCamTexture webCamTexture = CameraView.GetCamImage();
        if (webCamTexture.didUpdateThisFrame && webCamTexture.width > 100)
        {
            preprocess.ScaleAndCropImage(webCamTexture, IMAGE_SIZE, RunModel);
        }
    }
    */

    private void OnTextureUpdate(Texture texture)
    {

        preprocess.ScaleAndCropTexture(texture, IMAGE_SIZE, RunModel);

        /*
        WebCamTexture webCamTexture = texture;
        if (webCamTexture.didUpdateThisFrame && webCamTexture.width > 100)
        {
            preprocess.ScaleAndCropImage(webCamTexture, IMAGE_SIZE, RunModel);
        }
        */
    }

    void RunModel(byte[] pixels)
    {
        if(this!=null)
            StartCoroutine(RunModelRoutine(pixels));
    }


    IEnumerator RunModelRoutine(byte[] pixels)
    {

        Tensor tensor = TransformInput(pixels);

        var inputs = new Dictionary<string, Tensor> {
            { INPUT_NAME, tensor }
        };

        worker.Execute(inputs);
        Tensor outputTensor = worker.PeekOutput(OUTPUT_NAME);

        //get largest output
        List<float> temp = outputTensor.ToReadOnlyArray().ToList();
        float max = temp.Max();
        int index = temp.IndexOf(max);

        //set UI text
        //uiText.text = labels[index];
        text.text = labels[index];
        Debug.Log("Output: " + labels[index]);
        //dispose tensors
        tensor.Dispose();
        outputTensor.Dispose();
        yield return null;
    }

    //transform from 0-255 to -1 to 1
    Tensor TransformInput(byte[] pixels)
    {
        /*
        float[] transformedPixels = new float[pixels.Length];

        for (int i = 0; i < pixels.Length; i++)
        {
            transformedPixels[i] = (pixels[i] - 127f) / 128f;
        }
        return new Tensor(1, IMAGE_SIZE, IMAGE_SIZE, 1, transformedPixels);*/
        /*
        Texture2D tex = new Texture2D(IMAGE_SIZE, IMAGE_SIZE, TextureFormat.RGB24,false);

        tex.LoadRawTextureData(pixels);
        tex.Apply();
        intermediate.texture = tex;

        */

        float[] singleChannel = new float[IMAGE_SIZE * IMAGE_SIZE];
        for (int i = 0; i < singleChannel.Length; i++)
        {
            Color color = new Color32(pixels[i * 3 + 0], pixels[i * 3 + 1], pixels[i * 3 + 2], 255);
            singleChannel[i] = color.grayscale * 255;//
        }
        /*
                Texture2D texfinal = new Texture2D(IMAGE_SIZE, IMAGE_SIZE, TextureFormat.RGB24, false);

                texfinal.LoadRawTextureData(singleChannel);
                texfinal.Apply();
                final.texture = texfinal;*/
        return new Tensor(1, IMAGE_SIZE, IMAGE_SIZE, 1, singleChannel);
    }
}
