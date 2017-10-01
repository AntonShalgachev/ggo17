﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public event Action onTreasureCollected;
	public event Action onSpawnReached;

	Movement movement;

	private void Awake()
	{
		movement = GetComponent<Movement>();
	}

	private void Update()
	{
		movement.SetDirection(GetCurrentDirection());
	}

	Vector2 GetCurrentDirection()
	{
		Vector2 dir = Vector2.zero;

		if (Input.GetKey(KeyCode.W))
			dir += new Vector2(0.0f, 1.0f);
		if (Input.GetKey(KeyCode.S))
			dir += new Vector2(0.0f, -1.0f);
		if (Input.GetKey(KeyCode.A))
			dir += new Vector2(-1.0f, 0.0f);
		if (Input.GetKey(KeyCode.D))
			dir += new Vector2(1.0f, 0.0f);
		
		return dir;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Treasure"))
		{
			Destroy(collision.gameObject);
			if (onTreasureCollected != null)
				onTreasureCollected.Invoke();
		}
		
		if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerSpawn"))
		{
			if (onSpawnReached != null)
				onSpawnReached.Invoke();
		}
	}
}