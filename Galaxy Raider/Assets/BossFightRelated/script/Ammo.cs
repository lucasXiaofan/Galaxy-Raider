using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ammo : MonoBehaviour
{
    [SerializeField] AmmoSlot[] ammoslots;
    [System.Serializable]
    private class AmmoSlot
    {
        public ammoType ammot;
        public int ammoAmount;
    }
    public int getCurrentAmmo(ammoType ammot)
    {
        return GetAmmoSlot(ammot).ammoAmount;
    }

    public void IncreaseAmmo(ammoType ammot, int ammoAmount)
    {
        GetAmmoSlot(ammot).ammoAmount += ammoAmount;
    }

    public void descreaseAmmo(ammoType ammot)
    {
        GetAmmoSlot(ammot).ammoAmount--;
    }

    private AmmoSlot GetAmmoSlot(ammoType ammoType)
    {
        foreach (AmmoSlot slot in ammoslots)
        {
            if (slot.ammot == ammoType)
            {
                return slot;
            }
        }
        return null;
    }


}
