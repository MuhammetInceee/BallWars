using TMPro;
using UnityEngine;

public class RouletteText : MonoBehaviour
{
    private ScoreManager ScoreManager;
    private TextMeshPro _text;
    private void Start()
    {
        ScoreManager = ScoreManager.Instance;
        _text = GetComponent<TextMeshPro>();
        PointChecker();
    }

    private void PointChecker()
    {
        switch (transform.parent.name)
        {
            case "Center":
                _text.text = "+" + ScoreManager.centerPointScore;
                break;
            case "Green":
                _text.text = "+" + ScoreManager.greenPointScore;
                break;
            case "Red":
                _text.text = "+" + ScoreManager.redPointScore;
                break;
            case "Orange":
                _text.text = "+" + ScoreManager.orangePointScore;
                break;
            default:
                _text.text = _text.text;
                break;
        }
    }
}
