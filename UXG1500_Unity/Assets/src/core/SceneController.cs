using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] Transform m_PlayerTransform;
    [SerializeField] Transform m_SceneProps;
    [SerializeField] float m_PropDistance;
    [SerializeField] float m_PropFollowIncrement;
    [SerializeField] Transform m_FloorParent;
    [SerializeField] float m_GenerationDistance;
    [SerializeField] GameObject m_FloorPrefab;
    [SerializeField] float m_FloorDistance;
    [SerializeField] int m_ChunkPoolSize = 256;

    GameObject[] m_ChunkPool;
    List<int> m_ChunkPoolFree = new();
    Queue<GameObject> m_InactiveChunks = new();
    Vector3Int m_PreviousPlayerPosition;
    Vector3 m_PreviousPlayerPositionFloat;

    void Start()
    {
        m_PreviousPlayerPosition = new(0, -1, 0);
        InitPool();
    }

    void Update()
    {
        // fUCKK IT WE BALL
        PropsFollow();

        if (!UpdatePlayerPosition()) return;
        GenerateChunks();
        ClearChunks();
    }

    void PropsFollow()
    {
        if (Vector3.Distance(m_SceneProps.position, m_PlayerTransform.position) > m_PropDistance)
        {
            Vector3 direction = (m_PlayerTransform.position - m_SceneProps.position).normalized;
            direction.y = 0;
            m_SceneProps.position += m_PropFollowIncrement * direction;
        }
        m_PreviousPlayerPositionFloat = m_PlayerTransform.position;
    }

    void InitPool()
    {
        m_ChunkPool = new GameObject[m_ChunkPoolSize];
        GameObject tmp;
        for (int i = 0; i < 256; i++)
        {
            tmp = Instantiate(m_FloorPrefab, m_FloorParent);
            tmp.SetActive(false);
            m_InactiveChunks.Enqueue(tmp);
            m_ChunkPoolFree.Add(i);
        }
    }

    bool UpdatePlayerPosition()
    {
        Vector3Int curPos = GetPlayerPosition();
        if (m_PreviousPlayerPosition == curPos)
            return false;
        else
            m_PreviousPlayerPosition = curPos;
        return true;
    }

    void ClearChunks()
    {
        Vector3Int curPos = GetPlayerPosition();
        for (int i = 0; i < m_ChunkPool.Length; i++)
        {
            if (Vector3Int.Distance(GetFloorPosition(m_ChunkPool[i]), curPos) > m_GenerationDistance / m_FloorDistance)
                SetInactiveInPool(m_ChunkPool[i]);
        }
    }

    void GenerateChunks()
    {
        Vector3Int curPos = GetPlayerPosition();
        float diameter = m_GenerationDistance * 2 + m_FloorDistance;
        int diameterInt = (int)(diameter / m_FloorDistance);
        int baseX = diameterInt / 2 * -1; // Begins Negative, Add Index
        int baseZ = diameterInt / 2 * 1; // Begins Positive, Subtract Index
        for (int x = 0; x < diameterInt; x++)
        {
            for (int z = 0; z < diameterInt; z++)
            {
                Vector3Int tilePos = new(
                    curPos.x + (baseX + x),
                    0,
                    curPos.z + (baseZ - z));
                bool isNew = true;
                for (int i = 0; i < m_ChunkPool.Length; i++)
                {
                    if (m_ChunkPool[i] == null) continue;
                    if (tilePos == GetFloorPosition(m_ChunkPool[i]))
                    {
                        isNew = false;
                        break;
                    }
                }
                if (isNew) SetActiveInPool(tilePos);
            }
        }
    }

    void SetActiveInPool(Vector3Int floorPosition)
    {
        if (m_InactiveChunks.Count <= 0)
        {
            Logger.Log("Floor Object Pool has run out of inactive members!",
                Logger.SEVERITY_LEVEL.ERROR,
                Logger.LOGGER_OPTIONS.VERBOSE,
                MethodBase.GetCurrentMethod());
            return;
        }
        int idx = PopFreeIndex();
        m_ChunkPool[idx] = m_InactiveChunks.Dequeue();
        m_ChunkPool[idx].GetComponent<FloorData>().m_Index = idx;
        SetFloorPosition(m_ChunkPool[idx], floorPosition);
        m_ChunkPool[idx].SetActive(true);
    }

    void SetInactiveInPool(GameObject obj)
    {
        if (obj == null) return;
        obj.SetActive(false);
        m_InactiveChunks.Enqueue(obj);
        m_ChunkPool[obj.GetComponent<FloorData>().m_Index] = null;
        AppendFreeIndex(obj.GetComponent<FloorData>().m_Index);
    }

    int PopFreeIndex()
    {
        int idx = m_ChunkPoolFree.FirstOrDefault();
        m_ChunkPoolFree.Remove(idx);
        return idx;
    }

    void AppendFreeIndex(int idx) => m_ChunkPoolFree.Add(idx);

    Vector3Int GetPlayerPosition() => new Vector3Int(
        Mathf.FloorToInt(m_PlayerTransform.position.x / m_FloorDistance),
        0,
        Mathf.FloorToInt(m_PlayerTransform.position.z / m_FloorDistance));

    Vector3Int GetFloorPosition(GameObject floor)
    {
        if (floor == null) return Vector3Int.zero;
        return floor.GetComponent<FloorData>() == null ?
            Vector3Int.zero :
            floor.GetComponent<FloorData>().m_GridPosition;
    }

    void SetFloorPosition(GameObject floor, Vector3Int floorPosition) => floor.GetComponent<FloorData>().
        UpdateGridPosition(floorPosition, m_FloorDistance);
}
