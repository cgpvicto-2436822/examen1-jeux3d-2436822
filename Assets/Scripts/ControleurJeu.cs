using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Contrôleur de jeu qui gère l'état global du jeu et fournit un accès centralisé aux contrôles et aux autres systèmes.
/// </summary>
public class ControleurJeu : MonoBehaviour
{
    /// <summary>
    /// Instance singleton du contrôleur de jeu.
    /// </summary>
    public static ControleurJeu Instance { get; private set; }

    [field:SerializeField, Tooltip("Référence aux contrôles du joueur.")]
    public PlayerInput Controles { get; private set; }

    public Image[] chargesUI;

    public bool estEnJeu { get; set; } = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (var charge in chargesUI)
        {
            charge.enabled = false;
        }
    }

    public void UpdateChargeUI(int nbCharges)
    {
        if(nbCharges == 0)
        {
            foreach (var charge in chargesUI)
            {
                charge.enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < chargesUI.Length; i++)
            {
                chargesUI[i].enabled = i < nbCharges;
            }
        }
    }
}
