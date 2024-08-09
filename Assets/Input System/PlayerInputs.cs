using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerInputs : MonoBehaviour
{
	[Header("Character Input Values")]
	public Vector2 move;
	public Vector2 look;
	public bool jump;
	public bool lightAttack;
    public bool swap;
    public bool heavyAttack;
	public bool dash;
	[SerializeField]
	private float DamageAfterTime;
	[SerializeField]
	private float StrongDamageAfterTime;
	[SerializeField]
	private int Damage;

	[Header("Movement Settings")]
	public bool analogMovement;

	[Header("Mouse Cursor Settings")]
	public bool cursorLocked = true;
	public bool cursorInputForLook = true;

	public void OnMove(InputValue value)
	{
		MoveInput(value.Get<Vector2>());
	}

	public void OnLook(InputValue value)
	{
		if(cursorInputForLook)
		{
			LookInput(value.Get<Vector2>());
		}
	}

	public void OnJump(InputValue value)
	{
		JumpInput(value.isPressed);
	}

	public void OnLightAttack(InputValue value)
	{
		LightAttackInput(value.isPressed);
		StartCoroutine("Hit", false);
	}

    public void OnSwap(InputValue value)
    {
        SwapInput(value.isPressed);
    }

    public void OnHeavyAttack(InputValue value)
	{
		HeavyAttackInput(value.isPressed);
		StartCoroutine("Hit", true);
	}

	public void OnDash(InputValue value)
	{
		DashInput(value.isPressed);
	}

	public void MoveInput(Vector2 newMoveDirection)
	{
		move = newMoveDirection;
	} 

	public void LookInput(Vector2 newLookDirection)
	{
		look = newLookDirection;
	}

	public void JumpInput(bool newJumpState)
	{
		jump = newJumpState;
	}

	public void LightAttackInput(bool newLightAttackState)
	{
		lightAttack = newLightAttackState;
	}

    public void SwapInput(bool newSwapState)
    {
        swap = newSwapState;
    }

    public void HeavyAttackInput(bool newHeavyAttackState)
	{
		heavyAttack = newHeavyAttackState;
	}

	public void DashInput(bool newDashState)
	{
		dash = newDashState;
	}
	
    private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(cursorLocked);
	}

	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
	private IEnumerator Hit(bool strong) 
	{
		yield return null;
	}
}
