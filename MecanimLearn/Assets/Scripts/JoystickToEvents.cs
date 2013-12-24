using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// 根据 Joystick 的输入 得到 Player 运动的速度和方向
/// </summary>
public class JoystickToEvents : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 接受 用户输入 ，计算出 速率和方向 的 数值，通过数值来控制动画播放
    /// </summary>
    /// <param name="root">Player 的 transform</param>
    /// <param name="camera">Camera 的 transform</param>
    /// <param name="speed">Player 的 Speed（用来控制Player的Animation 是Idle，Walk还是Run）</param>
    /// <param name="direction">Player 的 direction（用来控制 Player的 Animation 如何转弯）</param>
    public static void Do(Transform root, Transform camera, ref Single speed, ref Single direction)
    {
        // 得到 Joystick的 方向
        Single horizontal = Input.GetAxis("Horizontal");
        Single vertical = Input.GetAxis("Vertical");

        Vector3 stickDirection = new Vector3(horizontal, 0, vertical);

        // 取 Player和Camera的 正前方的 direction
        Vector3 rootDirection = root.forward;
        Vector3 cameraDirection = camera.forward;
        // 把 Camera在y轴 方向的位置 设置为零，得到 在 xy面上的 向量
        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, cameraDirection);

        // 将 Joystick的输入 转换成 世界坐标系中 坐标
        Vector3 moveDirection = referentialShift * stickDirection;

        // Joystick 输入的值 作为 Player 在 xy平面上的 速度的方向
        Vector2 speedVec = new Vector2(horizontal, vertical);
        speed = Mathf.Clamp(speedVec.magnitude, 0, 1);

        // 判断是否为死区
        if (speed > 0.01f)
        {
            Vector3 axis = Vector3.Cross(rootDirection, moveDirection);
            direction = Vector3.Angle(rootDirection, moveDirection) / 180.0f * (axis.y < 0 ? -1 : 1);
        }
        else
        {
            direction = 0.0f;
        }
    }
}
