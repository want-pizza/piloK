using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform objTransform;

    [Header("Fleep rotation stats")]
    [SerializeField] private float flipRotationTime = 0.5f;

    private Coroutine turnCorotine;
    private CharacterFacing facing;
    private bool isFacingRight;

    private void Awake()
    {
        facing = objTransform.GetComponent<CharacterFacing>();
        isFacingRight = facing.IsFacingRight;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = objTransform.position;
    }

    public void CallTurn()
    {
        // turnCorotine = StartCoroutine(FlipYLerp());
        LeanTween.rotateY(gameObject, DeterminedEndRotation(), flipRotationTime).setEaseInOutSine();
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotation = DeterminedEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0;
        while (elapsedTime < flipRotationTime) {
            elapsedTime += Time.deltaTime;

            yRotation = Mathf.Lerp(startRotation, endRotation,(elapsedTime/flipRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }
    }

    private float DeterminedEndRotation()
    {
        isFacingRight = !isFacingRight;

        if (isFacingRight)
            return 0f;
        else
            return 180f;
    }
}
