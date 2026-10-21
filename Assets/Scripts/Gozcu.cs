
using UnityEngine;

public class Gozcu : MonoBehaviour
{
    [Header("Oyuncu")]
    [SerializeField] private Transform oyuncu;

    [Header("Gorus Ayarlari")]
    [SerializeField] private float menzil = 15f;
    [SerializeField] private float yatayGorusAcisi = 90f;
    [SerializeField] private float dikeyYariAci = 30f;
    [SerializeField] private float gozYuksekligi = 0.6f;

    [Header("Tarama (+4)")]
    [SerializeField] private float taramaSiniri = 60f;
    [SerializeField] private float taramaHizi = 30f;

    [Header("Takip (+3)")]
    [SerializeField] private float takipHizi = 120f;
    [SerializeField] private float takipBirakmaSuresi = 2f;

    [Header("Engeller")]
    [SerializeField] private LayerMask engeller = 1;

    private Renderer govde;

    private float baslangicYonu;
    private float taramaYon = 1f;

    private bool takipte;
    private float sonGorulenZaman;
    private Vector3 sonGorulenKonum;

    private void Awake()
    {
        govde = GetComponent<Renderer>();
        baslangicYonu = transform.eulerAngles.y;
    }

    private void Update()
    {
        if (oyuncu == null || govde == null)
            return;

        Vector3 goz =
            transform.position + Vector3.up * gozYuksekligi;

        Vector3 hedef = oyuncu.position;

        bool goruyor =
            GorusKonisinde(goz, hedef) &&
            GorusHattiAcik(goz, hedef);

        govde.material.color =
            goruyor ? Color.red : Color.green;

        if (goruyor)
        {
            takipte = true;
            sonGorulenZaman = Time.time;
            sonGorulenKonum = hedef;

            OyuncuyaDon(hedef, takipHizi);
        }
        else if (
            takipte &&
            Time.time - sonGorulenZaman < takipBirakmaSuresi
        )
        {
            // Oyuncuyu kaybedince son görülen konuma dön.
            OyuncuyaDon(sonGorulenKonum, takipHizi);
        }
        else
        {
            // Oyuncu 2 saniyedir görülmüyorsa taramaya dön.
            takipte = false;
            Tara();
        }
    }

    private void Tara()
    {
        float hedefYonu =
            baslangicYonu + taramaYon * taramaSiniri;

        if (
            Mathf.Abs(
                Mathf.DeltaAngle(
                    transform.eulerAngles.y,
                    hedefYonu
                )
            ) < 1f
        )
        {
            taramaYon *= -1f;
            hedefYonu =
                baslangicYonu + taramaYon * taramaSiniri;
        }

        Quaternion hedefDonus =
            Quaternion.Euler(0f, hedefYonu, 0f);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            hedefDonus,
            taramaHizi * Time.deltaTime
        );
    }

    private void OyuncuyaDon(Vector3 hedef, float hiz)
    {
        Vector3 yon = hedef - transform.position;

        // Gözcü yatay eksende döner; yukarý veya aþaðý eðilmez.
        yon.y = 0f;

        if (yon.sqrMagnitude < 0.001f)
            return;

        Quaternion hedefDonus = Quaternion.LookRotation(yon);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            hedefDonus,
            hiz * Time.deltaTime
        );
    }

    private bool GorusKonisinde(Vector3 goz, Vector3 hedef)
    {
        Vector3 fark = hedef - goz;
        float uzaklik = fark.magnitude;

        if (uzaklik > menzil)
            return false;

        if (uzaklik < 0.001f)
            return true;

        Vector3 yon = fark / uzaklik;

        // Yönü gözcünün kendi koordinat sistemine çevir.
        Vector3 yerelYon =
            transform.InverseTransformDirection(yon);

        // Yatay açý: sað ve sol sýnýr.
        float yatayAci = Mathf.Atan2(
            yerelYon.x,
            yerelYon.z
        ) * Mathf.Rad2Deg;

        if (Mathf.Abs(yatayAci) > yatayGorusAcisi * 0.5f)
            return false;

        // Dikey açý: yukarý ve aþaðý sýnýr.
        float yatayBilesen = new Vector2(
            yerelYon.x,
            yerelYon.z
        ).magnitude;

        float dikeyAci = Mathf.Atan2(
            yerelYon.y,
            yatayBilesen
        ) * Mathf.Rad2Deg;

        if (Mathf.Abs(dikeyAci) > dikeyYariAci)
            return false;

        return true;
    }

    private bool GorusHattiAcik(Vector3 goz, Vector3 hedef)
    {
        return !Physics.Linecast(
            goz,
            hedef,
            engeller,
            QueryTriggerInteraction.Ignore
        );
    }

    private void OnDrawGizmos()
    {
        Vector3 goz =
            transform.position + Vector3.up * gozYuksekligi;

        // Görüþ menzili.
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(goz, menzil);

        // Yatay görüþ sýnýrlarý.
        Vector3 sagKenar =
            Quaternion.AngleAxis(
                yatayGorusAcisi * 0.5f,
                Vector3.up
            ) * transform.forward;

        Vector3 solKenar =
            Quaternion.AngleAxis(
                -yatayGorusAcisi * 0.5f,
                Vector3.up
            ) * transform.forward;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(goz, goz + sagKenar * menzil);
        Gizmos.DrawLine(goz, goz + solKenar * menzil);

        // Dikey görüþ sýnýrlarýný gösteren çizgiler.
        Vector3 yatayYon = transform.forward;

        Vector3 yukariKenar =
            Quaternion.AngleAxis(
                -dikeyYariAci,
                transform.right
            ) * yatayYon;

        Vector3 asagiKenar =
            Quaternion.AngleAxis(
                dikeyYariAci,
                transform.right
            ) * yatayYon;

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(goz, goz + yukariKenar * menzil);
        Gizmos.DrawLine(goz, goz + asagiKenar * menzil);
    }
}
