﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    // Player jumps when joyful
    public float happyJumpTimer = 1f;
    private float mJumpCooldown = 0f;
    private bool mAlternateFoot = false;

    private GameObject mMoodEnvironmentEffects;

    public override void Update()
    {
        base.Update();

        if (mJumpCooldown > 0f)
        {
            mJumpCooldown -= Time.deltaTime;
        }

        mAnimator.SetBool("AlternateFoot", mAlternateFoot);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal") , Input.GetAxis("Vertical"));
        if (mEmotion != Emotion.JOYFUL)
        {
            DoDirectionalMovement(inputVector);
        }
        else if (mEmotion == Emotion.JOYFUL)
        {
            // totally different movement if happy and jumping
            if (CanJump())
            {
                Vector3 jumpForce = Vector3.up * jumpAccel;
                mRigidbody.velocity = Vector3.zero;
                mRigidbody.AddForce(jumpForce);
                mJumpCooldown = happyJumpTimer;
                mAlternateFoot = !mAlternateFoot;

                DoDirectionalMovement(inputVector, true);
                Debug.Log("jump V - " + mRigidbody.velocity.ToString());
            }
        }
    }

    private bool ShouldJump()
    {
        return mEmotion == Emotion.JOYFUL;
    }

    private bool CanJump()
    {
        return !mInAir && mRigidbody.velocity.y <= 0f && mJumpCooldown <= 0f;
    }

    public override void SetEmotion(Emotion e)
    {
        base.SetEmotion(e);
        if (mEmotion == Emotion.JOYFUL)
        {
            mJumpCooldown = happyJumpTimer;
        }

        // environmental vfx
        if (mMoodEnvironmentEffects != null)
        {
            mMoodEnvironmentEffects.SetActive(false);
        }

        string moodTag = mEmotion.GetMoodTag();
        if (moodTag != null)
        {
            GameObject rootObj = GameObject.FindWithTag(moodTag);
            if (rootObj != null)
            {
                mMoodEnvironmentEffects = rootObj.transform.GetChild(0).gameObject;
                mMoodEnvironmentEffects.SetActive(true);
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        BreakIfBreakableAndAngry(collision.gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        BreakIfBreakableAndAngry(other.gameObject);
    }

    private void BreakIfBreakableAndAngry(GameObject other)
    {
        if (mEmotion == Emotion.ANGRY)
        {
            Breakable breakable = other.GetComponent<Breakable>();
            if (breakable != null)
            {
                breakable.Break();
            }
        }
    }

    public void StompGround()
    {
        if (mEmotion != Emotion.ANGRY) return;

        Debug.Log("STOMP");

        Camera camera = Camera.main;
        CameraShake shake = camera.gameObject.GetComponent<CameraShake>();
        StartCoroutine(shake.Shake(0.15f, 0.025f));
    }
}
