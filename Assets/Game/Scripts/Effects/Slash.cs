using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tweeners;


public class Effect : MonoBehaviour
{
	void OnEnable ()
	{
		SetupTweeners();
	}

	public virtual void Create (Vector3 pos)
	{
		
	}

	public virtual void Create (Vector3 pos, Vector3 dir)
	{

	}

	public virtual void Play ()
	{
		this.gameObject.SetActive(true);
	}

	public virtual void SetupTweeners ()
	{
	}

	public virtual void OnEntryEnd ()
	{
	}

	public virtual void OnExitEnd ()
	{
		GameObject.DestroyImmediate(this.gameObject);
	}
}

public class Slash : Effect 
{
	public float scaleSpeed;
	public Vector3 size;

	public float fadeTime;

	// Scales up to show, fades out to hide
	[SerializeField] private Scaler scaler;
	[SerializeField] private Fader fader;

	public override void Create (Vector3 pos)
	{
		Create(pos, Vector3.zero);
	}

	public override void Create (Vector3 pos, Vector3 dir)
	{
		Slash newSlash = Instantiate(this) as Slash;
		newSlash.transform.SetParent(this.transform.parent);
		newSlash.transform.position = pos;
		newSlash.Play();

		SpriteRenderer spr = GetComponentInChildren<SpriteRenderer>();
		spr.flipX = (dir.x > 0f);
		spr.flipY = (dir.y > 0f);
	}

	public override void SetupTweeners ()
	{
		scaler.SetSpeed(scaleSpeed);
		scaler.StartScaling(Vector3.zero, size);
		scaler.SetFinishedListener(OnEntryEnd);

		fader.SetFadeTime(fadeTime);
	}

	public override void OnEntryEnd ()
	{
		base.OnEntryEnd ();
		fader.Play();
		fader.SetFinishedListener(OnExitEnd);
	}

}
