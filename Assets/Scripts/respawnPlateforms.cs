using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnPlateforms : MonoBehaviour
{
    public float disableDuration = 3f; // Durée pendant laquelle la plateforme est désactivée
    public float respawnDelay = 5f; // Délai avant que la plateforme réapparaisse
    private bool playerIsOnPlatform = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerIsOnPlatform = true;
            // Désactiver la plateforme au contact du joueur
            gameObject.SetActive(false);
            // Planifier la réapparition après le délai de réapparition
            Invoke("Respawn", respawnDelay);
        }
    }

    private void Respawn()
    {
        // Activer la plateforme
        gameObject.SetActive(true);
        playerIsOnPlatform = false;
    }

    private void Update()
    {
        // Si le joueur a quitté la plateforme, planifier la désactivation
        if (!playerIsOnPlatform)
        {
            Invoke("DisablePlatform", disableDuration);
        }
    }

    private void DisablePlatform()
    {
        // Désactiver la plateforme
        gameObject.SetActive(false);
        // Planifier la réapparition après le délai de réapparition
        Invoke("Respawn", respawnDelay);
    }
}