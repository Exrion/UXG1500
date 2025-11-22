using UnityEngine;

public class InteractablePsychController : IInteractable
{
    public Animator animator;
    public GameObject heldObj; //object which we pick up
    public Transform holdPos;
    public GameObject ControllerScript; //when player equips the controller, they will be able to navigate the projector

    protected override void Start()
    {
        base.Start();
        animator = heldObj.GetComponent<Animator>();
        ControllerScript.SetActive(false);
    }
    public override void OnInteracted()
    {
        heldObj.transform.SetParent(holdPos, false);
        ControllerScript.SetActive(true);
    }
}
