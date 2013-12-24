using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Locomotion 的逻辑
/// </summary>
public class Locomotion : MonoBehaviour
{
    private Animator m_Animator = null;
    private Int32 m_SpeedId = 0;
    private Int32 m_AngularSpeedId = 0;
    private Int32 m_DirectionId = 0;

    public float m_SpeedDampTime = 0.1f;
    public float m_AnguarSpeedDampTime = 0.25f;
    public float m_DirectionResponseTime = 0.2f;

    // DotNet 4.0 框架不适用
    //private static readonly Lazy<Locomotion> _instance = new Lazy<Locomotion>(() => new Locomotion(Anim));

    private static readonly Locomotion _instance = new Locomotion(Anim);

    public static Animator Anim
    {
        get;
        set;
    }

    public static Locomotion Instance
    {
        get
        {
            return _instance;
        }
    }

    static Locomotion()
    {
        
    }

    private Locomotion(Animator animator)
    {
        m_Animator = animator;

        // 将 游戏中 附加到Player的 Animator的Controller 中设置的 Parameters 转换成一个 Id
        m_SpeedId = Animator.StringToHash("Speed");
        m_AngularSpeedId = Animator.StringToHash("AngularSpeed");
        m_DirectionId = Animator.StringToHash("Direction");
    }

    public void Do(Single speed, Single direction)
    {
        // Base Layer 的 StateInformation
        AnimatorStateInfo state = m_Animator.GetCurrentAnimatorStateInfo(0);

        Boolean inTransition = m_Animator.IsInTransition(0);
        // 判断 Animator State Machine的 活动状态的 Name和给定的 name匹配 
        Boolean inIdle = state.IsName("Locomotion.Idle");
        Boolean inTurn = state.IsName("Locomotion.TurnOnSpot") || state.IsName("Locomotion.PlantNTurnRight") || state.IsName("Locomotion.PlantNTurnLeft");
        Boolean inWalkRun = state.IsName("Locomotion.WalkRun");

        Single speedDampTime = inIdle ? 0 : m_SpeedDampTime;
        Single angularSpeedDampTime = inWalkRun || inTransition ? m_AnguarSpeedDampTime : 0;
        Single directionDampTime = inTurn || inTransition ? 1000000 : 0;

        // AngularSpeed 是一个 Parameter
        Single angularSpeed = direction / m_DirectionResponseTime;

        // 设置 Animator State Machine中 Parameters 的值
        m_Animator.SetFloat(m_SpeedId, speed, speedDampTime, Time.deltaTime);
        m_Animator.SetFloat(m_AngularSpeedId, angularSpeed, angularSpeedDampTime, Time.deltaTime);
        m_Animator.SetFloat(m_DirectionId, direction, directionDampTime, Time.deltaTime);
    }
}
