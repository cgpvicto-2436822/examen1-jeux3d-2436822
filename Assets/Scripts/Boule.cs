using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Objet représentant une boule contrôlée par le joueur.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Boule : MonoBehaviour
{
    [SerializeField, Tooltip("La cible pour le suvi de la caméra")]
    private Transform cibleCamera;

    [SerializeField, Tooltip("Force de déplacement de la boule.")]
    private float forceDeplacement;

    // Force appliquée à la boule pour le déplacement à chaque frame.
    [SerializeField, Tooltip("La force appliquée à la boule.")]
    private Vector3 forceAppliquee;

    // Référence au Rigidbody de la boule pour appliquer la physique.
    private Rigidbody rigidbody;

    private PlayerInput controles;

    /// <summary>
    /// Obtient la vélocité actuelle de la boule.
    /// </summary>
    public Vector3 Velocite => rigidbody.linearVelocity;

    [SerializeField, Tooltip("Nombre de charges disponibles pour la boule.")]
    private int nbCharges = 0;

    [SerializeField, Tooltip("Indique si la boule est actuellement en charge.")]
    private bool enCharge = false;

    [SerializeField, Tooltip("Chronomètre pour gérer la durée de la charge.")]
    private float chrono = 0.0f;

    [SerializeField, Tooltip("Force appliquée lors de la charge.")]
    private float forceCharge = 15.0f;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        controles = ControleurJeu.Instance.Controles;
        if(rigidbody != null)
        {
            rigidbody.useGravity = false;
        }
        controles.actions.FindAction("Commencer").performed += CommencerJeu;
    }

    private void OnDestroy()
    {
        if (ControleurJeu.Instance == null)
            return;

        if (controles == null) 
            return;

        controles.actions.FindAction("Diriger").performed -= CommencerDirection;
        controles.actions.FindAction("Diriger").canceled -= ArreterDirection;
    }

    private void Update()
    {
        if (cibleCamera != null)
        {
            cibleCamera.position = rigidbody.position;
        }

        if(enCharge)
        {
            chrono += Time.deltaTime;
        }

        if (chrono > 1.0f)
        {
            chrono = 0.0f;
            forceAppliquee -= forceCharge * Vector3.forward;
            enCharge = false;
        }
    }

    private void FixedUpdate()
    {
        Diriger();
    }

    private void CommencerDirection(InputAction.CallbackContext contexte)
    {
        forceAppliquee += contexte.ReadValue<float>() * forceDeplacement * Vector3.right;
    }

    private void ArreterDirection(InputAction.CallbackContext contexte)
    {
        forceAppliquee = contexte.ReadValue<float>() * forceDeplacement * Vector3.right;
    }

    private void Diriger()
    {
        if(!Mathf.Approximately(forceAppliquee.sqrMagnitude, 0.0f))
        {
            rigidbody.AddForce(forceAppliquee, ForceMode.Force);
        }
    }

    private void CommencerJeu(InputAction.CallbackContext contexte)
    {
        if(ControleurJeu.Instance != null)
        {
            if(!ControleurJeu.Instance.estEnJeu)
            {
                ControleurJeu.Instance.estEnJeu = true;
                rigidbody.useGravity = true;
                controles.actions.FindAction("Diriger").performed += CommencerDirection;
                controles.actions.FindAction("Diriger").canceled += ArreterDirection;
                controles.actions.FindAction("Charge").performed += Charge;
            }
        }
    }

    private void Charge(InputAction.CallbackContext contexte)
    {
        if (nbCharges > 0 && !enCharge)
        {
            enCharge = true;
            forceAppliquee += forceCharge * Vector3.forward;
            nbCharges--;
            ControleurJeu.Instance.UpdateChargeUI(nbCharges);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Charge"))
        {
            AjouterCharge();
            Destroy(other.gameObject);
        }
    }

    public void AjouterCharge()
    {
        if(nbCharges < 3)
        {
            nbCharges++;
            ControleurJeu.Instance.UpdateChargeUI(nbCharges);
        }
    }
}
