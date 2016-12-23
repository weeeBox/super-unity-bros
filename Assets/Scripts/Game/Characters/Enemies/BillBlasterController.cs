using UnityEngine;
using System.Collections;

using LunarCore;

public class BillBlasterController : BaseBehaviour
{
    [SerializeField]
    BulletBillController m_BulletBill;

    [SerializeField]
    float m_ShootingTimeMin = 3;

    [SerializeField]
    float m_ShootingTimeMax = 10;

    protected override void OnStart()
    {
        assert.IsNotNull(m_BulletBill);
        StartCoroutine(ShootingLoop());
    }

    IEnumerator ShootingLoop()
    {
        while (true)
        {
            float time = Random.Range(m_ShootingTimeMin, m_ShootingTimeMax);
            yield return new WaitForSeconds(time);

            Shoot();
        }
    }

    void Shoot()
    {
        BulletBillController bulletBill = Instantiate(m_BulletBill) as BulletBillController;
        bulletBill.transform.parent = transform;
        bulletBill.transform.localPosition = Vector3.zero;
        bulletBill.direction = -1; // FIXME: check mario position
    }
}
