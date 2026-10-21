
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Move : MonoBehaviour
{
    [Header("Kamera")]
    [SerializeField] private Transform kamera;

    [Header("Hareket Ayarlari")]
    [SerializeField] private float yurumeHizi = 4f;
    [SerializeField] private float kosmaHizi = 7f;
    [SerializeField] private float yercekimi = -9.81f;
    [SerializeField] private float ziplamaYuksekligi = 1.5f;
    [SerializeField] private float donusHizi = 720f;

    private CharacterController cc;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    private float dikeyHiz;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();

        moveAction = InputSystem.actions.FindAction("Player/Move");
        jumpAction = InputSystem.actions.FindAction("Player/Jump");
        sprintAction = InputSystem.actions.FindAction("Player/Sprint");
    }

    private void Update()
    {
        if (kamera == null ||
            moveAction == null ||
            jumpAction == null ||
            sprintAction == null)
        {
            return;
        }

        float dt = Time.deltaTime;

        Vector2 girdi = moveAction.ReadValue<Vector2>();

        Vector3 ileri = kamera.forward;
        ileri.y = 0f;
        ileri.Normalize();

        Vector3 sag = kamera.right;
        sag.y = 0f;
        sag.Normalize();

        Vector3 yon = ileri * girdi.y + sag * girdi.x;
        yon = Vector3.ClampMagnitude(yon, 1f);

        float hiz = sprintAction.IsPressed()
            ? kosmaHizi
            : yurumeHizi;

        if (cc.isGrounded && dikeyHiz < 0f)
        {
            dikeyHiz = -2f;
        }

        if (cc.isGrounded && jumpAction.WasPressedThisFrame())
        {
            dikeyHiz = Mathf.Sqrt(
                2f * -yercekimi * ziplamaYuksekligi
            );
        }

        dikeyHiz += yercekimi * dt;

        Vector3 hareket =
            (yon * hiz + Vector3.up * dikeyHiz) * dt;

        cc.Move(hareket);

        if (yon.sqrMagnitude > 0.001f)
        {
            Quaternion hedefDonus = Quaternion.LookRotation(yon);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                hedefDonus,
                donusHizi * dt
            );
        }
    }
}