using UnityEngine;

class FloorData : MonoBehaviour
{
    public Vector3Int m_GridPosition { get; private set; }
    public int m_Index;
    public void Init(Vector3Int pos) => m_GridPosition = pos;
    public void Init(Vector3 pos) => m_GridPosition = new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z);
    public void Init(int x, int y, int z) => m_GridPosition = new Vector3Int(x, y, z);

    public void UpdateGridPosition(Vector3Int pos, float floorDistance)
    {
        Init(pos);
        transform.position = new Vector3(
            m_GridPosition.x * floorDistance,
            m_GridPosition.y * floorDistance,
            m_GridPosition.z * floorDistance);
    }
}