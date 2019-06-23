using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPractice : MonoBehaviour
{
    private static TargetPractice _instance;
    public GameObject[] targets;
    public GameObject[] braziers;
    [HideInInspector]
    public bool isUnlocked = false;
    public GameObject gate;
    private Animator animator;
  

    public int DeadTargets { get {
            int a = 0;
            foreach (GameObject g in targets)
                if (g.GetComponent<DummyEnemy>().GetIsDead())
                    a++;
            return a;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
        animator = gate.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Open", isUnlocked);
    }

    public static void Triggered()
    {
        for (int i = 0; i < _instance.targets.Length; i++)
        {
            if (_instance.targets[i].GetComponent<DummyEnemy>().GetIsDead() == false)
                return;
        }

        _instance.LitBraziers();
        _instance.isUnlocked = true;

        for (int i = 0; i < _instance.targets.Length; i++)
        {
            _instance.targets[i].GetComponent<DummyEnemy>().RiseTargetDummyRise();
        }
    }

    private void LitBraziers()
    {
        for (int i = 0; i < braziers.Length; i++)
        {
            braziers[i].GetComponent<Brazier_control>().SetIsLit(true);
        }

    }

}
