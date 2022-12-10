using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    private Rigidbody _rigitdbody;
    public Rigidbody Rigidbody => _rigitdbody;

    private void Start()
    {
        _rigitdbody = GetComponent<Rigidbody>();
    }

    public void TorqueBall(float yDir,float xDir)
    {
        _rigitdbody.AddTorque(new Vector3(-xDir, yDir));
    }

    public void StopTorqueBall()
    {
        _rigitdbody.angularVelocity = Vector3.zero;
    }

    public void BreakTorqueBall()
    {
        _rigitdbody.angularVelocity /= 2f;
    }

    public void UseGravityBall()
    {
        _rigitdbody.useGravity = true;
    }
}
