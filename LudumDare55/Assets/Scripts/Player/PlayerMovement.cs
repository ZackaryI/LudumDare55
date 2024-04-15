using System.Collections;
using UnityEngine;

public class PlayerMovement
{
    public void Movement(Rigidbody2D playerRigidbody, PlayerAttributes playerAttributes)
    {
        Vector2 movementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        playerRigidbody.velocity = movementVector * playerAttributes.playerMoveSpeed;
    }

    public void RotatePlayer(Camera cam, Transform playerTransform)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = cam.ScreenToWorldPoint(mousePos);

        Vector2 direction = mousePos - playerTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle - 90);
        playerTransform.rotation = targetRotation;
    }
}
