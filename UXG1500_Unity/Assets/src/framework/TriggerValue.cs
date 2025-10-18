public struct TriggerValue
{
    private bool val;
    public TriggerValue(bool val) { this.val = val; }
    public bool Get()
    {
        if (val) { val = false; return true; }
        else { return false; }
    }
    public void Trigger() { val = true; }
}
