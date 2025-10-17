using UnityEngine;
using UnityEngine.UI;
public class Heart : MonoBehaviour
{
    public Sprite _fullHeart, _oneFourthsHeart, _twoFourthsHeart, _threeFourthsHeart, _emptyHeart;
    Image _heartImage;

    private void Awake()
    {
        _heartImage = GetComponent<Image>();
    }

    public void SetHeartImage(HeartStatus status)
    {
        switch (status)
        {
            case HeartStatus.Empty:
                _heartImage.sprite = _emptyHeart;
                break;
            case HeartStatus.oneFourths:
                _heartImage.sprite = _oneFourthsHeart;
                break;
            case HeartStatus.twoFourths:
                _heartImage.sprite = _twoFourthsHeart;
                break;
            case HeartStatus.ThreeFourths:
                _heartImage.sprite = _threeFourthsHeart;
                break;
            case HeartStatus.Full:
                _heartImage.sprite = _fullHeart;
                break;

        }
    }

}

public enum HeartStatus
{
    Empty = 0,
    Full = 4,
    ThreeFourths = 3,
    twoFourths = 2,
    oneFourths = 1,
}
