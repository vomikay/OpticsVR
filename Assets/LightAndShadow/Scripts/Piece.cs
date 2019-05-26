using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent( typeof( Interactable ) )]
public class Piece : MonoBehaviour
{
	public const float MOVEMENT_INCREMENT = 0.1f;

    [SerializeField] private TargetPiece targetPiece = default;
    private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & ( ~Hand.AttachmentFlags.SnapOnAttach ) & (~Hand.AttachmentFlags.DetachOthers) & (~Hand.AttachmentFlags.VelocityMovement);
    private Interactable interactable;

    public bool IsCorrect { get; private set; }

    private void Start()
    {
        this.IsCorrect = false;
        interactable = this.GetComponent<Interactable>();
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
            Debug.Log("Correct Piece");
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

    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes startingGrabType = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(this.gameObject);

        if (this.interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
        {
            hand.HoverLock(this.interactable);
            hand.AttachObject(this.gameObject, startingGrabType, attachmentFlags);
        }
        else if (isGrabEnding)
        {
            hand.DetachObject(this.gameObject);
            hand.HoverUnlock(this.interactable);
        }
    }

}
