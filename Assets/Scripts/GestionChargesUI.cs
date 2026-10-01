using UnityEngine;
using UnityEngine.UI;

public class GestionChargesUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Image[] chargesUI;

    private void Awake()
    {
        foreach (var charge in chargesUI)
        {
            charge.enabled = false;
        }
    }

    public void UpdateChargeUI(int nbCharges)
    {
        if (nbCharges == 0)
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
