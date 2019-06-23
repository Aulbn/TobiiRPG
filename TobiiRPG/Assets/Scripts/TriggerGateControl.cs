using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGateControl : MonoBehaviour
{
    public GameObject[] triggerplates;
    public Material untriggered_mat;
    public Material triggered_mat;
    public bool isUnlocked = false;
    public GameObject gate;
    public GameObject[] braziers;
    private Animator animator;

    public int TriggeredPlates
    {
        get
        {
            int a = 0;
            foreach (GameObject g in triggerplates)
                if (g.GetComponent<TriggerPlate>().isTriggered)
                    a++;
            return a;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = gate.GetComponent<Animator>();
        for (int i = 0; i < triggerplates.Length; i++)
        {
            triggerplates[i].GetComponent<Renderer>().material = untriggered_mat;
        }
    }

    // Update is called once per frame
    void Update()
    {

        animator.SetBool("Open", isUnlocked);
    }

    public void Triggered(GameObject plate)
    {
        plate.GetComponent<Renderer>().material = triggered_mat;
        Unlocked();

    }

    public void Unlocked()
    {
        for (int i = 0; i < triggerplates.Length; i++)
        {
            if (triggerplates[i].GetComponent<TriggerPlate>().GetTrigger() == false)
                return;
        }
        LitBraziers();
        isUnlocked = true;
        animator.SetBool("Open", isUnlocked);
    }

    private void LitBraziers()
    {
        for (int i = 0; i < braziers.Length; i++)
        {
            braziers[i].GetComponent<Brazier_control>().SetIsLit(true);
        }

    }
}
