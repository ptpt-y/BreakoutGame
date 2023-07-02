using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ball")
        {
            Ball ball = other.GetComponent<Ball>();
            BallsManager.Instance.Balls.Remove(ball);
            ball.Die();
        }
    }
}
