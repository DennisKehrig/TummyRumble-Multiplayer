using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Health : NetworkBehaviour {
    public RectTransform healthBar;

    public const int maxHealth = 100;
    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    private Vector3 spawnPosition;

    public override void OnStartLocalPlayer() {
        spawnPosition = transform.position;
    }

    public void TakeDamage(int amount) {
        if (!isServer) return;

        currentHealth -= amount;
        if (currentHealth <= 0) {
            currentHealth = maxHealth;
            RpcRespawn();
        }
    }

    void OnChangeHealth(int health) {
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcRespawn() {
        if (isLocalPlayer) {
            transform.position = spawnPosition;
        }
    }
}
