using UnityEngine.Events;

class Interactable_Event : IInteractable
{
    public UnityEvent m_OnInteracted;

    public override void OnInteracted()
    {
        m_OnInteracted?.Invoke();
    }
}