using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour, IPool
{
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private float _textSize = 0.25f;
    [SerializeField] private Color _normaleDamageColor;
    [SerializeField] private Color _echoDamageColor;
    private Camera _mainCamera;

    public void Initialize(int id, Vector3 position, Quaternion textInfoAsQuaternion)
    {
        if(_mainCamera == null)
            _mainCamera = Camera.main;

        var distance = Vector3.Distance(position, _mainCamera.transform.position);
        var scale = _textSize * distance * (Vector3.up + Vector3.right) + Vector3.forward;

        transform.position = position;
        transform.localScale = scale;
        transform.LookAt(_mainCamera.transform);

        Text.text = textInfoAsQuaternion.x.ToString();

        // we can make the damage types indexed as int and passed here and make a switch case
        if (textInfoAsQuaternion.y == 1f)
            Text.color = _normaleDamageColor;
        else
            Text.color = _echoDamageColor;


    }

    public void ResetInst()
    {
        Text.text = default;
    }
}
