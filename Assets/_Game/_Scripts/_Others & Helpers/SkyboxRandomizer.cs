using Sirenix.OdinInspector;
using UnityEngine;

public class SkyboxRandomizer : MonoBehaviour
{
    [SerializeField] private bool _ChangeMaterialOnStart = true;

    [SerializeField] private Material _skyboxMaterial;
    [SerializeField] private Color[] _colorButtom;
    [SerializeField] private Color[] _colorTop;




    void Start()
    {
        if (_ChangeMaterialOnStart)
        {
            _skyboxMaterial.SetColor("_Color2", _colorButtom[Random.Range(0, _colorButtom.Length)]);
            _skyboxMaterial.SetColor("_Color1", _colorTop[Random.Range(0, _colorTop.Length)]);
        }
    }


    [Button("Randomize")]
    void RandomizedSkybox()
    {
        _skyboxMaterial.SetColor("_Color2", _colorButtom[Random.Range(0, _colorButtom.Length)]);
        _skyboxMaterial.SetColor("_Color1", _colorTop[Random.Range(0, _colorTop.Length)]);
    }



}
