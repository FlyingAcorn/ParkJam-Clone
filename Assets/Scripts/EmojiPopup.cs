using System.Collections;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EmojiPopup : MonoBehaviour
{
    [SerializeField] private Sprite[] emojiList;
     [CanBeNull] private Image _myImage;

    private void Awake()
    {
        _myImage = GetComponent<Image>();
        _myImage.sprite = emojiList[Random.Range(0, emojiList.Length)];
        StartCoroutine(EmojiSequence());
    }

    private IEnumerator EmojiSequence()
    {
        transform.DOLocalMoveY(transform.localPosition.y + 50,2);
        _myImage.DOFade(0, 2);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        
    }
}
