using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPiece : MonoBehaviour
{
    public const float DISTANCE_COEFFICIENT = 10f;

    [SerializeField] float positionTolerance = 1f;

    public bool CheckIsCorrect(Transform pieceTransform)
	{
		float distance = Vector3.Distance(transform.position, pieceTransform.position);
		return ((distance * DISTANCE_COEFFICIENT) < positionTolerance);
	}
}
