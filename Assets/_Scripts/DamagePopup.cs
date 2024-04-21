using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class DamagePopup : MonoBehaviour, IPool
{
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private float _textSize = 0.25f;
    [SerializeField] private Color _normaleDamageColor;
    [SerializeField] private Color _echoDamageColor;
    [SerializeField] private float _damagePopupLifeTimeInSec = 0.5f;
    [SerializeField] private Vector3 _textConstOffset;
    [SerializeField] private Vector3 _textRandomOffset;
    private Camera _mainCamera;
    private RectTransform rectTransform;
    private PoolInstance _PopupPool;


    public void Initialize(int id, Vector3 position, Quaternion textInfoAsQuaternion)
    {
        if (_PopupPool == null)
            _PopupPool = PoolManager.PoolInst.DamagePopup;
        
        if (_mainCamera == null)
            _mainCamera = Camera.main;

        _PopupPool.Return(this.gameObject, _damagePopupLifeTimeInSec);

        SetTransform(position);
        SetText(textInfoAsQuaternion);
    }

    private void SetTransform(Vector3 position)
    {
        var distance = Vector3.Distance(position, _mainCamera.transform.position);
        var scale = _textSize * (distance * Vector3.one);
        var offset = new Vector3(Random.Range(-_textRandomOffset.x, _textRandomOffset.x),
            Random.Range(-_textRandomOffset.y, _textRandomOffset.y), 0f);


        transform.position = position + _textConstOffset + offset;
        transform.LookAt(_mainCamera.transform);
        transform.localScale = scale;
    }

    private void SetText(Quaternion textInfoAsQuaternion)
    {
        Text.text = textInfoAsQuaternion.x.ToString();

        // we can make the damage types indexed as int and passed here and make a switch case
        if (textInfoAsQuaternion.y == 0f)
            Text.color = _normaleDamageColor;
        else
            Text.color = _echoDamageColor;
    }

    public void ResetInst()
    {
        Text.text = default;
    }
}
