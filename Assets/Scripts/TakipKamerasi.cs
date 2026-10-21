
using UnityEngine;
using UnityEngine.InputSystem;

public class TakipKamerasi : MonoBehaviour
{
    [Header("Takip Ayarlari")]
    [SerializeField] private Transform hedef;
    [SerializeField] private float mesafe = 6f;
    [SerializeField] private float odakYuksekligi = 0.6f;

    [Header("Fare Ayarlari")]
    [SerializeField] private float hassasiyet = 0.15f;
    [SerializeField] private float minPitch = -20f;
    [SerializeField] private float maksPitch = 60f;
    [SerializeField] private float keskinlik = 10f;

    private InputAction look;
    private float yaw;
    private float pitch = 20f;

    private void Awake()
    {
        look = InputSystem.actions.FindAction("Player/Look");

        if (hedef != null)
        {
            yaw = hedef.eulerAngles.y;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        if (hedef == null || look == null)
        {
            return;
        }

        Vector2 fark = look.ReadValue<Vector2>();

        yaw += fark.x * hassasiyet;

        pitch = Mathf.Clamp(
            pitch - fark.y * hassasiyet,
            minPitch,
            maksPitch
        );

        Quaternion donus = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 odak =
            hedef.position + Vector3.up * odakYuksekligi;

        Vector3 istenenKonum =
            odak - donus * Vector3.forward * mesafe;

        float t = 1f - Mathf.Exp(-keskinlik * Time.deltaTime);

        transform.position = Vector3.Lerp(
            transform.position,
            istenenKonum,
            t
        );

        transform.rotation = Quaternion.LookRotation(
            odak - transform.position
        );
    }
}
