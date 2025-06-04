using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform player;
    [SerializeField] private Vector2 minMaxPlayerSizeToDistanceRatio;
    private float playerSizeToDistanceRatio;
    CinemachineTransposer transposer;
    Vector3 targetCameraOffset;

    // Start is called before the first frame update
    void Start()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraPosition();
    }

    public void UpdateCameraPosition()
    {
        float currentPlayerSizePercent = player.GetComponent<Player>().GetSizePercent();
        playerSizeToDistanceRatio = Mathf.Lerp(minMaxPlayerSizeToDistanceRatio.x, minMaxPlayerSizeToDistanceRatio.y, currentPlayerSizePercent);

        targetCameraOffset = new Vector3(0, player.localScale.x * playerSizeToDistanceRatio, -player.localScale.x * playerSizeToDistanceRatio);

        if (targetCameraOffset != transposer.m_FollowOffset)
            SetCamOffset(Vector3.Lerp(transposer.m_FollowOffset, targetCameraOffset, 0.1f));
    }

    private void SetCamOffset(Vector3 offset)
    {
        transposer.m_FollowOffset = offset;
     }
}
