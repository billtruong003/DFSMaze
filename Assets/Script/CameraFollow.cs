using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Nhân vật mà camera sẽ theo dõi
    public float smoothSpeed = 0.125f;  // Tốc độ mượt mà của camera khi di chuyển
    public Vector3 offset;  // Khoảng cách giữa camera và nhân vật

    private float initialZ;

    private void Start()
    {
        initialZ = transform.position.z;  // Lưu trị giá trục Z ban đầu
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target không được định nghĩa cho CameraFollow.");
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        smoothedPosition.z = initialZ;  // Giữ nguyên trục Z
        transform.position = smoothedPosition;
    }
}


