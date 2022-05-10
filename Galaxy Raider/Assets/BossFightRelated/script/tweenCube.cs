using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class tweenCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(Vector3.up, 4).SetRelative().SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
