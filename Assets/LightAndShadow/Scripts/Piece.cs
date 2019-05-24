using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
	public const float MOVEMENT_INCREMENT = 0.1f;

    [SerializeField] private TargetPiece targetPiece = default;

    public bool IsCorrect { get; private set; }

    private void Start()
    {
        this.IsCorrect = false;
    }

    private void Update()
	{
		if (!this.IsCorrect)
        {
            this.CheckIsCorrect();
        }
	}

    private void CheckIsCorrect()
	{
        if (targetPiece.CheckIsCorrect(this.transform))
        {
            this.IsCorrect = true;
            StartCoroutine(this.LerpTowardTarget(targetPiece.transform));
        }
	}

    private IEnumerator LerpTowardTarget(Transform targetTransform)
	{
		float t = 0f;
		Vector3 startPosition = this.transform.position;

		while (t < 1f)
		{
			this.transform.position = Vector3.Lerp(startPosition, targetTransform.position, t);
			t += Time.deltaTime;
			yield return null;
		}
	}

}
