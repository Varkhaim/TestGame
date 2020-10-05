using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody rbody = null;
    [SerializeField] FollowingCamera followingCamera = null;
    [SerializeField] Animator animator = null;

    [Header("Parameters")]
    [SerializeField] float movementSpeed = 1f;
    [SerializeField] float backwardMovementSpeed = 0.3f;
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] float jumpPower = 1f;
    [SerializeField] float jumpMaxDistance = 1f;
    [SerializeField] float deadlyFallingSpeed = 5f;

    [Header("Controls")]
    [SerializeField] KeyCode leftKey = KeyCode.A;
    [SerializeField] KeyCode rightKey = KeyCode.D;
    [SerializeField] KeyCode backKey = KeyCode.S;
    [SerializeField] KeyCode forwardKey = KeyCode.W;
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    private bool deathOnFall = false;
    private bool isDead = false;
    private bool isJumpingUp = false;

    void Update()
    {
        HandleMovement();
        CheckFalling();
    }

    #region Movement
    private void CheckFalling()
    {
        if (rbody.position.y < GameManager.Instance.DeathZoneHeight)
        {
            Death();
        }

        if (deathOnFall || isDead) return;
        if (rbody.velocity.y <= -deadlyFallingSpeed)
        {
            deathOnFall = true;
            animator.SetTrigger("DeathFall");
        }
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, jumpMaxDistance);
    }

    private void HandleMovement()
    {
        PlayerControls();

        if (rbody.velocity.y < 0)
        {
            if (isJumpingUp)
            {
                isJumpingUp = false;
                animator.SetBool("isJumpingUp", false);
                animator.SetBool("isFalling", true);
            }
        }
    }

    private void MoveForward()
    {
        Vector3 vec = transform.forward * movementSpeed;
        rbody.velocity = new Vector3(vec.x, rbody.velocity.y, vec.z);
    }

    private void MoveBackwards()
    {
        Vector3 vec = -transform.forward * backwardMovementSpeed;
        rbody.velocity = new Vector3(vec.x, rbody.velocity.y, vec.z);
    }

    private void PlayerControls()
    {
        if (deathOnFall || isDead) return;

        if (Input.GetKey(leftKey))
        {
            transform.Rotate(new Vector3(0, -rotationSpeed * Time.deltaTime, 0));
            rbody.rotation = transform.rotation;
        }
        if (Input.GetKey(rightKey))
        {
            transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
            rbody.rotation = transform.rotation;
        }
        if (Input.GetKey(forwardKey))
        {
            MoveForward();
        }
        if (Input.GetKey(backKey))
        {
            MoveBackwards();
        }

        if (Input.GetKeyDown(forwardKey))
            animator.SetBool("isRunning", true);
        if (Input.GetKeyUp(forwardKey))
            animator.SetBool("isRunning", false);
        if (Input.GetKeyDown(backKey))
            animator.SetBool("isBackwardWalking", true);
        if (Input.GetKeyUp(backKey))
            animator.SetBool("isBackwardWalking", false);

        if (Input.GetKeyDown(jumpKey))
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (!IsGrounded()) return;

        isJumpingUp = true;
        Vector3 vel = new Vector3(rbody.velocity.x, jumpPower, rbody.velocity.z);
        rbody.velocity = vel;
        animator.SetBool("isJumpingUp", true);
    }

    #endregion

    #region Death
    IEnumerator deathDelay()
    {
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(GameManager.Instance.DeathTime);
        Death();
    }

    private void Respawn()
    {
        isDead = false;
        GameManager.Instance.RespawnPlayer(transform);
        rbody.velocity = Vector3.zero;
        animator.Rebind();
        followingCamera.ResetPosition();
    }

    private void SetUpDeath()
    {
        if (!isDead)
        {
            isDead = true;
            StartCoroutine(deathDelay());
        }
    }

    private void Death()
    {
        deathOnFall = false;
        GameManager.Instance.AddDeath();
        Respawn();
    }
    #endregion

    #region Collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trap"))
        {
            SetUpDeath();
        }
        if (other.CompareTag("FinishBox"))
        {
            if (!GameManager.Instance.isGameFinished)
            {
                GameManager.Instance.FinishGame();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((rbody.velocity.y <= 0f) && (IsGrounded()))
        {
            if (deathOnFall)
            {
                deathOnFall = false;
                animator.SetTrigger("Impact");
                SetUpDeath();
            }
            animator.SetBool("isFalling", false);
        }
    }
    #endregion


}
