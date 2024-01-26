using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int multiplier;
    [SerializeField] protected GameObject[] _root;
    [SerializeField] protected ParticleSystem _collectParticalSystem;
    [SerializeField] protected TextMeshPro[] _multiplierText;
    [SerializeField] private float _speed;
    [SerializeField] private bool _startMovingDown = true;

    public bool StartMovingDown { get => _startMovingDown; set => _startMovingDown = value; }
    public int Multiplier { get => multiplier; set => multiplier = value; }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        int evenNumber = Random.Range(2, 9);
        Multiplier = evenNumber / 2 * 2;
        // Debug.Log($" multiplier Equal : {Multiplier}");
        for (int i = 0; i < _multiplierText.Length; i++)
        {
            _multiplierText[i].text = $"x{Multiplier}";
        }
    }

    public void Collect()
    {
        //_multiplierText[0].gameObject.SetActive(false);
        for (int i = 0; i < _root.Length; i++)
        {
            Destroy(_root[i]);
        }
        if (_root.Length == 0)
        {
            Destroy(gameObject);
        }
        if (_collectParticalSystem) _collectParticalSystem.Play();
        Destroy(gameObject, 0.5f);
    }
    private void Update()
    {
        if (StartMovingDown)
            transform.position += Vector3.down * _speed * Time.deltaTime * 50;
    }
    private void MovingDown()
    {
        StartMovingDown = true;
    }
}
