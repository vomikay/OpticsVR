using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent( typeof( Interactable ) )]
public class Piece : MonoBehaviour
{
    [SerializeField] private TargetPiece targetPiece = default;

	private const float MOVEMENT_INCREMENT = 0.1f;
    private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & ( ~Hand.AttachmentFlags.SnapOnAttach ) & (~Hand.AttachmentFlags.DetachOthers) & (~Hand.AttachmentFlags.VelocityMovement);
    private Interactable interactable = default;

    public bool IsCorrect { get; private set; }

    private void Start()
    {
        IsCorrect = false;
        interactable = GetComponent<Interactable>();
    }

    private void Update()
	{
		if (!IsCorrect)
        {
            CheckIsCorrect();
        }
	}

    private void CheckIsCorrect()
	{
        if (targetPiece.CheckIsCorrect(transform))
        {
            IsCorrect = true;
            Debug.Log("Correct Piece");
            StartCoroutine(LerpTowardTarget(targetPiece.transform));
        }
	}

    private IEnumerator LerpTowardTarget(Transform targetTransform)
	{
		float t = 0f;
		Vector3 startPosition = transform.position;

		while (t < 1f)
		{
			transform.position = Vector3.Lerp(startPosition, targetTransform.position, t);
			t += Time.deltaTime;
			yield return null;
		}
	}

    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes startingGrabType = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(gameObject);

        if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
        {
            hand.HoverLock(interactable);
            hand.AttachObject(gameObject, startingGrabType, attachmentFlags);
        }
        else if (isGrabEnding)
        {
            hand.DetachObject(gameObject);
            hand.HoverUnlock(interactable);
        }
    }
}
