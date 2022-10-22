using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneDonor : MonoBehaviour
{
    public Transform m_TargetRig;
    public List<Transform> m_DonorRigs = new List<Transform>();
    private SkinnedMeshRenderer m_DonorSkinnedMeshRender;
    private Transform[] m_DonorBones;
    private SkinnedMeshRenderer m_TargetSkinnedMeshRenderer;
    private Transform[] m_TargetBones;

    void Start()
    {

        TransferSMRList();
    }

    private void TransferSMRList()
    {

        for (int r = 0; r < m_DonorRigs.Count; r++)
        {
            m_DonorRigs[r].position = m_TargetRig.position;
            m_DonorRigs[r].rotation = m_TargetRig.rotation;
            m_DonorSkinnedMeshRender = m_DonorRigs[r].GetComponentInChildren<SkinnedMeshRenderer>();
            m_DonorBones = m_DonorSkinnedMeshRender.bones;
            m_TargetSkinnedMeshRenderer = m_TargetRig.GetComponentInChildren<SkinnedMeshRenderer>();
            m_TargetBones = m_TargetSkinnedMeshRenderer.bones;
            for (int i = 0; i < m_DonorBones.Length; i++)
            {
                string boneName = m_DonorBones[i].name;
                for (int j = 0; j < m_TargetBones.Length; j++)
                {
                    if (m_TargetBones[j].name == boneName)
                    {
                        m_DonorBones[i] = m_TargetBones[j];
                    }
                }
            }
            m_DonorSkinnedMeshRender.bones = m_DonorBones;
            m_DonorSkinnedMeshRender.transform.parent = m_TargetRig;
        }

        for (int i = 0; i < m_DonorRigs.Count; i++)
        {
            Destroy(m_DonorRigs[i].gameObject);
        }
        Destroy(this.gameObject);
    }
}