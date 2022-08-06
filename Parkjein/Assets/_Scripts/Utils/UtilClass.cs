using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilClass
{
    public static IEnumerator EnableDampingEndFrame(CinemachineVirtualCamera vcam)
    {
        float xDamping = vcam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping;
        float yDamping = vcam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping;
        float zDamping = vcam.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping;

        vcam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0f;
        vcam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0f;
        vcam.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 0f;

        yield return null;

        vcam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = xDamping;
        vcam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = yDamping;
        vcam.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = zDamping;
    }
}
