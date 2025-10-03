using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class RoadmanScript : MonoBehaviour
{

    public float speed = 5f; // Speed of the roadman
    public int health = 100; // Health of the roadman
    public int damage = 10; // Damage dealt by the roadman
    public Transform player; // Reference to the player transform
    public float attackRange = 2f; // Range within which the roadman can attack
    public float detectionRange = 10f; // Range within which the roadman can detect the player
    public float attackCooldown = 1f; // Cooldown time between attacks

    private Animator anim;
    private enum State { Idle, Chasing, Attacking, Recovering}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
